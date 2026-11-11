import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.migration import migrate_xml
from schema_engine.note_migration import migrate_note
from schema_engine.schema import validate_document


FIXTURES = Path(__file__).with_name('fixtures')


class NoteMigrationTests(unittest.TestCase):
    def test_legacy_wire_upgrade_preserves_data_and_reports_missing_requisites(self):
        original = (FIXTURES / 'explanatorynote-migration-01-03.xml').read_text(encoding='utf-8')
        self.assertTrue(validate_document(original)['valid'])
        source = ET.fromstring(original)
        snapshot = ET.tostring(source)
        upgraded, warnings = migrate_note(source, '01.03', '01.07')
        self.assertEqual(ET.tostring(source), snapshot)
        self.assertEqual(upgraded.get('id'), source.get('id'))
        self.assertEqual(upgraded.findtext('IssueAuthor/Organization/FullName'), 'Тестовый филиал')
        self.assertEqual(upgraded.findtext('IssueAuthor/Organization/Address/ForeignAddress/Country'), 'Беларусь')
        self.assertEqual(upgraded.findtext('Developer/Person/PostAddress/RussianPostAddress/RegionCode'), '54')
        self.assertIsNone(upgraded.find('Developer/Person/PostAddress/RussianPostAddress/OKTMOCode'))
        self.assertIsNone(upgraded.find('Signers/Signer/SNILS'))
        self.assertIsNone(upgraded.find('Signers/ChiefProjectEngineer'))
        self.assertEqual(upgraded.findtext('FinanceSources/StateCustomerSource/Owner/FullName'), 'Тестовый заказчик')
        project = upgraded.find('IndustrialObject/ProjectDocumentation')
        self.assertEqual(project.find('Section2/SectionContent/Document').get('ObjectIDREFS'), 'test-object test-part')
        self.assertEqual(project.findtext('Section2/SectionContent/SubSection/Document/DocNumber'), 'Т-3')
        self.assertEqual(project.findtext('Section12/SectionContent/ObjectEstimates/Document/DocNumber'), 'С-1')
        self.assertIsNone(project.find('Section12/SectionContent/LocalEstimates'))
        self.assertEqual([child.tag for child in upgraded.find('DesignerAssurance')], ['Text', 'SubTitle', 'Text'])
        for warning in ('/RAFP', '/@NOPRIZNumber', '/@role', '/Signers/Signer/SNILS',
                        '/Signers/ChiefProjectEngineer', '/OKTMOCode', 'двух частей',
                        '/ObjectLocalEstimates/Document', '/DesignerAssurance/Signer'):
            self.assertTrue(any(warning in message for message in warnings), (warning, warnings))

    def test_0105_upgrade_copies_per_document_participants_and_warns_about_iul(self):
        original = '''<ExplanatoryNote SchemaVersion="01.05">
          <Signers><ChiefProjectEngineer><FamilyName>Тестов</FamilyName><FirstName>Тест</FirstName>
            <SNILS>000-000-000 00</SNILS><NOPRIZ>П-000000</NOPRIZ>
          </ChiefProjectEngineer></Signers>
          <EngineeringSurveyDocuments><Document>
            <DocName>Отчёт об изысканиях</DocName><DocNumber>Т-4</DocNumber><DocDate>2026-01-01</DocDate>
          </Document></EngineeringSurveyDocuments>
          <NonIndustrialObject ObjectID="test">
            <ProjectDocumentation><Section2><SectionContent><Document ObjectIDREFS="test">
              <DocName>Тестовый документ</DocName><DocNumber>Т-3</DocNumber>
              <ProjectDocParticipants><ProjectDocParticipant role="Разработка">
                <FamilyName>Тестов</FamilyName><FirstName>Тест</FirstName><SNILS>000-000-000 00</SNILS>
                <NOPRIZ>П-000000</NOPRIZ>
              </ProjectDocParticipant></ProjectDocParticipants>
              <IULFile><FileName>test-iul.pdf</FileName></IULFile>
              <File><FileName>test.pdf</FileName></File>
            </Document></SectionContent></Section2></ProjectDocumentation>
          </NonIndustrialObject>
        </ExplanatoryNote>'''
        result = migrate_xml(original)
        self.assertEqual(result['sourceVersion'], '01.05')
        self.assertEqual(result['targetVersion'], '01.07')
        upgraded = ET.fromstring(result['xml'])
        self.assertEqual(upgraded.findtext('Signers/ChiefProjectEngineer/NOPRIZNumber'), 'П-000000')
        self.assertIsNone(upgraded.find('Signers/ChiefProjectEngineer').get('role'))
        survey = upgraded.find('EngineeringSurveyDocuments/Document')
        self.assertEqual(survey.findtext('DocNumber'), 'Т-4')
        self.assertIsNone(survey.find('SurveyDocParticipants'))
        document_path = 'NonIndustrialObject/ProjectDocumentation/Section2/SectionContent/Document'
        document = upgraded.find(document_path)
        participant = document.find('ProjectDocParticipants/DocParticipant')
        self.assertEqual(participant.get('role'), 'Разработка')
        self.assertEqual(participant.findtext('Surname'), 'Тестов')
        self.assertEqual(participant.findtext('NOPRIZNumber'), 'П-000000')
        self.assertEqual(document.findtext('File/FileName'), 'test.pdf')
        self.assertIsNone(document.find('IULFile'))
        for warning in ('/Signers/ChiefProjectEngineer/@role', '/SurveyDocParticipants', '/IULFile'):
            self.assertTrue(any(warning in message for message in result['warnings']), warning)
        downgraded = ET.fromstring(migrate_xml(result['xml'], '01.05')['xml'])
        participant = downgraded.find(f'{document_path}/ProjectDocParticipants/ProjectDocParticipant')
        self.assertEqual(participant.get('role'), 'Разработка')
        self.assertEqual(participant.findtext('NOPRIZ'), 'П-000000')

    def test_modern_fixture_downgrades_to_valid_0105_and_reports_legacy_wire_gaps(self):
        original = (FIXTURES / 'explanatorynote-01-07.xml').read_text(encoding='utf-8')
        self.assertTrue(validate_document(original)['valid'])
        result = migrate_xml(original, '01.05')
        validation = validate_document(result['xml'])
        self.assertTrue(validation['valid'], validation['errors'])
        old = ET.fromstring(result['xml'])
        self.assertEqual(old.findtext('Signers/ChiefProjectEngineer/NOPRIZ'), 'П-000000')
        self.assertEqual(old.findtext('NonIndustrialObject/Address/Region'), '54')
        self.assertIsNone(old.find('NonIndustrialObject').get('Unique'))
        for warning in ('/Organization/@NOPRIZNumber', '/OKTMOCode', '/@Unique'):
            self.assertTrue(any(warning in message for message in result['warnings']), warning)

        result = migrate_xml(original, '01.03')
        old = ET.fromstring(result['xml'])
        self.assertEqual(old.findtext('Signers/Signer/FamilyName'), 'Тестов')
        self.assertEqual(old.findtext('DesignerAssurance/Assurance/Text'), 'Тестовое заверение проектировщика')
        self.assertIsNone(old.find('NonIndustrialObject/ConstructionDuration'))
        self.assertIsNone(old.find('NonIndustrialObject/EnergyEfficiency/EnergyEfficiencyClass'))
        for warning in ('Официальная XSD 01.03', '/ChiefProjectEngineer', '/EnergyEfficiencyClass',
                        '/ConstructionDuration', '/Section2/NotDeveloped'):
            self.assertTrue(any(warning in message for message in result['warnings']), warning)

    def test_legacy_estimate_subsections_and_single_land_category(self):
        original = '''<ExplanatoryNote SchemaVersion="01.07">
          <IndustrialObject ObjectID="test">
            <LandCategory>1</LandCategory><LandCategory>2</LandCategory>
            <ProjectDocumentation><Section12><SectionContent>
              <ObjectEstimates><Document><DocNumber>С-1</DocNumber></Document></ObjectEstimates>
              <LocalEstimates><Document><DocNumber>С-2</DocNumber></Document></LocalEstimates>
            </SectionContent></Section12></ProjectDocumentation>
          </IndustrialObject>
        </ExplanatoryNote>'''
        result = migrate_xml(original, '01.03')
        downgraded = ET.fromstring(result['xml'])
        self.assertEqual([node.text for node in downgraded.findall('IndustrialObject/LandCategory')], ['1'])
        self.assertTrue(any('/LandCategory[2]' in warning for warning in result['warnings']))
        section_path = 'IndustrialObject/ProjectDocumentation/Section12'
        subsections = downgraded.findall(f'{section_path}/ObjectLocalEstimates/SubSection')
        self.assertEqual([node.get('Name') for node in subsections], ['Объектные сметы', 'Локальные сметы'])
        upgraded = ET.fromstring(migrate_xml(result['xml'])['xml'])
        for category, number in (('ObjectEstimates', 'С-1'), ('LocalEstimates', 'С-2')):
            self.assertEqual(upgraded.findtext(f'{section_path}/SectionContent/{category}/Document/DocNumber'), number)
