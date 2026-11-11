import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.migration import migrate_xml, normalize_legacy_conclusion
from schema_engine.schema import validate_document


FIXTURE = Path(__file__).with_name('fixtures') / 'conclusion-01-04.xml'
MARITIME_ADDRESS = 'Тестовый участок в территориальном море Российской Федерации'


class ConclusionMigrationTests(unittest.TestCase):
    def setUp(self):
        self.current = ET.fromstring(FIXTURE.read_text(encoding='utf-8'))
        self.current.find('ExaminationObject/ExaminationForm').text = '1'

    def assert_valid(self, xml):
        result = validate_document(xml)
        self.assertTrue(result['valid'], result['errors'])

    def test_maritime_address_exports_to_valid_legacy_code_and_keeps_description(self):
        address = self.current.find('Object/Address')
        address.clear()
        ET.SubElement(address, 'SeaRussianAddress').text = MARITIME_ADDRESS
        xml = ET.tostring(self.current, encoding='unicode')
        self.assert_valid(xml)
        exported = migrate_xml(xml, '01.03')
        self.assert_valid(exported['xml'])
        old = ET.fromstring(exported['xml'])
        self.assertEqual(old.findtext('Object/Address/Region'), '00')
        self.assertEqual(old.findtext('Object/Address/Note'), MARITIME_ADDRESS)
        self.assertTrue(any('/Object/Address' in warning and 'морской адрес' in warning
                            for warning in exported['warnings']))

    def test_explicit_legacy_country_distinguishes_maritime_and_foreign_object_addresses(self):
        old = ET.fromstring(migrate_xml(ET.tostring(self.current, encoding='unicode'), '01.03')['xml'])
        address = old.find('Object/Address')
        address.find('Region').text = '00'
        address.find('Note').text = MARITIME_ADDRESS
        country = ET.Element('Country')
        address.insert(0, country)
        for country_name, variant in (('Российская Федерация', 'SeaRussianAddress'), ('Беларусь', 'ForeignAddress')):
            with self.subTest(country=country_name):
                country.text = country_name
                xml = ET.tostring(old, encoding='unicode')
                self.assert_valid(xml)
                migrated = migrate_xml(xml)
                modern_address = ET.fromstring(migrated['xml']).find('Object/Address')
                self.assertEqual([child.tag for child in modern_address], [variant])
                if variant == 'SeaRussianAddress':
                    self.assertEqual(modern_address.findtext(variant), MARITIME_ADDRESS)
                else:
                    self.assertEqual(modern_address.findtext(f'{variant}/Country'), country_name)
                    self.assertEqual(modern_address.findtext(f'{variant}/Note'), MARITIME_ADDRESS)

    def test_ambiguous_legacy_code_reports_the_choice_and_keeps_address_description(self):
        old = ET.fromstring(migrate_xml(ET.tostring(self.current, encoding='unicode'), '01.03')['xml'])
        address = old.find('Object/Address')
        address.find('Region').text = '00'
        address.find('Note').text = MARITIME_ADDRESS
        xml = ET.tostring(old, encoding='unicode')
        self.assert_valid(xml)
        migrated = migrate_xml(xml)
        modern_address = ET.fromstring(migrated['xml']).find('Object/Address')
        self.assertIsNone(modern_address.find('SeaRussianAddress'))
        self.assertIsNone(modern_address.find('ForeignAddress/Country'))
        self.assertEqual(modern_address.findtext('ForeignAddress/Note'), MARITIME_ADDRESS)
        self.assertTrue(any('/Object/Address' in warning and 'Уточните тип адреса' in warning
                            for warning in migrated['warnings']))

    def test_address_fields_missing_in_legacy_schema_are_preserved_in_its_note(self):
        address = self.current.find('Object/Address/RussianAddress')
        postcode = ET.Element('PostIndex')
        postcode.text = '630000'
        address.insert(1, postcode)
        address.remove(address.find('Note'))
        ET.SubElement(address, 'City').text = 'Новосибирск'
        ET.SubElement(address, 'Landmark').text = 'Тестовый ориентир'
        xml = ET.tostring(self.current, encoding='unicode')
        self.assert_valid(xml)
        exported = migrate_xml(xml, '01.03')
        self.assert_valid(exported['xml'])
        old = ET.fromstring(exported['xml']).find('Object/Address')
        self.assertEqual(old.findtext('City'), 'Новосибирск')
        self.assertEqual(old.findtext('Note'), 'Почтовый индекс: 630000, Ориентир: Тестовый ориентир')
        self.assertTrue(any('/Object/Address/PostIndex' in warning for warning in exported['warnings']))

    def test_finance_owner_wraps_and_preserves_order_and_registration(self):
        finance = self.current.find('Finance')
        finance.find('FinanceType').text = '3'
        ET.SubElement(finance, 'FinanceSize').text = '40.5'
        owner = ET.SubElement(finance, 'FinanceOwner')
        owner.extend(ET.fromstring(ET.tostring(self.current.find('ExpertOrganization'))))
        current = ET.tostring(self.current, encoding='unicode')
        self.assert_valid(current)
        exported = migrate_xml(current, '01.03')
        self.assert_valid(exported['xml'])
        old = ET.fromstring(exported['xml'])
        self.assertEqual(old.findtext('Finance/FinanceSize'), '40.5')
        self.assertEqual(old.findtext('Finance/FinanceOwner/Organization/OrgOGRN'), '0000000000000')
        modern = ET.fromstring(migrate_xml(exported['xml'])['xml'])
        self.assertEqual(modern.findtext('Finance/FinanceOwner/OGRN'), '0000000000000')
        self.assertEqual(modern.findtext('Finance/FinanceOwner/Address/RussianAddress/RegionCode'), '54')
        self.assertIsNone(modern.find('Finance/FinanceOwner/Organization'))

    def test_unsupported_versions_are_not_silently_reinterpreted(self):
        for root, versions in (('Conclusion', ('01.01',)), ('ExplanatoryNote', ('01.00', '01.01', '01.02'))):
            for version in versions:
                with self.subTest(root=root, version=version):
                    with self.assertRaisesRegex(ValueError, f'Версия {version} .* не поддерживается'):
                        migrate_xml(f'<{root} SchemaVersion="{version}"/>')

    def test_legacy_summary_normalizes_only_complete_statements(self):
        xml = '''<Conclusion SchemaVersion="01.02"><Summary>
            <EngineeringSurveySummary>Результаты инженерных изысканий
                соответствуют требованиям технических регламентов</EngineeringSurveySummary>
            <ProjectDocumentsSummary>Соответствует</ProjectDocumentsSummary>
        </Summary></Conclusion>'''
        migrated = migrate_xml(xml)
        summary = ET.fromstring(migrated['xml']).find('Summary')
        self.assertEqual(summary.findtext('EngineeringSurveySummary'),
                         'Результаты инженерных изысканий соответствуют требованиям технических регламентов.')
        self.assertEqual(summary.findtext('ProjectDocumentsSummary'), 'Соответствует')
        self.assertTrue(any('/ProjectDocumentsSummary:' in warning for warning in migrated['warnings']))

    def test_legacy_date_placeholder_is_absent_instead_of_an_invalid_date_block(self):
        xml = '''<Conclusion SchemaVersion="01.00"><Summary>
            <EngineeringSurveySummaryDate>01.01.2020</EngineeringSurveySummaryDate>
            <ProjectDocumentsSummaryDate> — </ProjectDocumentsSummaryDate>
        </Summary></Conclusion>'''
        migrated = migrate_xml(xml)
        summary = ET.fromstring(migrated['xml']).find('Summary')
        self.assertEqual(summary.findtext('EngineeringSurveySummaryDate/Date'), '2020-01-01')
        self.assertIsNone(summary.find('ProjectDocumentsSummaryDate'))
        self.assertTrue(any('/ProjectDocumentsSummaryDate:' in warning and 'дата не указана' in warning
                            for warning in migrated['warnings']))

    def test_short_legacy_survey_summary_requires_explicit_primary_survey_scope(self):
        for stage, scope, legacy_summary, expected in (
            ('1', '1', 'Соответствует.', 'Результаты инженерных изысканий соответствуют требованиям технических регламентов.'),
            ('2', '1', 'Соответствует.', 'Соответствует.'),
            ('1', '2', 'Соответствует.', 'Соответствует.'),
            ('1', '1', 'Не соответствует.', 'Не соответствует.'),
        ):
            with self.subTest(stage=stage, scope=scope, legacy_summary=legacy_summary):
                xml = f'''<Conclusion SchemaVersion="01.00">
                    <ExaminationObject>
                        <ExaminationStage>{stage}</ExaminationStage><ExaminationType>{scope}</ExaminationType>
                    </ExaminationObject>
                    <Summary><EngineeringSurveySummary>{legacy_summary}</EngineeringSurveySummary></Summary>
                </Conclusion>'''
                result = ET.fromstring(migrate_xml(xml)['xml'])
                self.assertEqual(result.findtext('Summary/EngineeringSurveySummary'), expected)

    def test_normalization_corrects_classic_serializer_order(self):
        xml = ET.tostring(self.current, encoding='unicode')
        legacy = ET.fromstring(migrate_xml(xml, '01.03')['xml'])
        organization = legacy.find('ExpertOrganization')
        legacy[:] = list(reversed(legacy))
        organization[:] = list(reversed(organization))
        unordered = ET.tostring(legacy, encoding='unicode')
        self.assertFalse(validate_document(unordered)['valid'])

        normalized = normalize_legacy_conclusion(unordered)
        self.assert_valid(normalized)
        result = ET.fromstring(normalized)
        self.assertEqual(result.get('ConclusionGUID'), legacy.get('ConclusionGUID'))
        self.assertEqual(result.findtext('ExpertOrganization/OrgFullName'), organization.findtext('OrgFullName'))
