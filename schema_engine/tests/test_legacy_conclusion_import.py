import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.documents import SCHEMA_ASSETS, source_versions
from schema_engine.migration import migrate_xml
from schema_engine.schema import form_schema, new_document, validate_document


FIXTURE = Path(__file__).with_name('fixtures') / 'conclusion-01-04.xml'
SURVEY_RESULT = ('Результаты инженерных изысканий, выполненных для подготовки проектной документации, '
                 'соответствуют требованиям технических регламентов.')


class LegacyConclusionImportTests(unittest.TestCase):
    def legacy_document(self, version):
        source = FIXTURE.read_text(encoding='utf-8')
        root = ET.fromstring(migrate_xml(source, '01.03')['xml'])
        root.set('SchemaVersion', version)
        root.remove(root.find('EcologyExpertise'))
        if version == '01.00':
            examination = root.find('ExaminationObject')
            examination.remove(examination.find('EngineeringSurveysIM'))
            survey = ET.Element('ExpertEngineeringSurveys', {'EngineeringSurveyType': '1'})
            ET.SubElement(survey, 'EngineeringSurveyConditions').text = 'Тестовые условия'
            summary = root.find('Summary')
            root.insert(list(root).index(summary), survey)
            survey_date = ET.Element('EngineeringSurveySummaryDate')
            survey_date.text = '01.02.2026'
            summary.insert(1, survey_date)
        summary = root.find('Summary/ExaminationSummary')
        summary.clear()
        summary.text = SURVEY_RESULT
        return root

    def test_both_archived_versions_are_validated_by_the_official_xsds(self):
        for version in ('01.00', '01.02'):
            with self.subTest(version=version):
                original = self.legacy_document(version)
                xml = ET.tostring(original, encoding='unicode')
                result = validate_document(xml)
                self.assertTrue(result['valid'], result['errors'])
                self.assertEqual(result['validationBasis'], 'official-xsd')
                self.assertEqual(result['version'], version)
                self.assertIn(f'conclusion-{version.replace(".", "-")}.xsd', SCHEMA_ASSETS)
                migrated = migrate_xml(xml)
                target = ET.fromstring(migrated['xml'])
                self.assertEqual(migrated['sourceVersion'], version)
                self.assertEqual(migrated['targetVersion'], '01.04')
                self.assertEqual(target.get('ConclusionGUID'), original.get('ConclusionGUID'))
                self.assertEqual(target.findtext('Object/FunctionsClass'), '12.01.006.001')
                self.assertEqual(target.findtext('Summary/ExaminationSummary/EngineeringSurveysResultsSummary'),
                                 SURVEY_RESULT)
                if version == '01.00':
                    self.assertEqual(target.findtext('Summary/EngineeringSurveySummaryDate/Date'), '2026-02-01')
                self.assertIsNone(target.find('EcologyExpertise'))
                self.assertIsNone(target.find('Experts/Expert/SNILS'))
                self.assertIsNone(target.find('Object/Address/RussianAddress/OKTMOCode'))

    def test_archived_versions_are_import_sources_only(self):
        self.assertEqual(source_versions('conclusion'), ('01.00', '01.02', '01.03', '01.04'))
        for version in ('01.00', '01.02'):
            with self.subTest(version=version):
                with self.assertRaises(ValueError):
                    new_document('conclusion', version)
                with self.assertRaises(ValueError):
                    form_schema('conclusion', version)
                with self.assertRaisesRegex(ValueError, 'можно преобразовать в 01.04'):
                    migrate_xml(f'<Conclusion SchemaVersion="{version}"/>', '01.03')

    def test_literal_functions_code_is_retained_and_free_description_requires_review(self):
        root = self.legacy_document('01.02')
        functions = root.find('Object/FunctionsClass')
        functions.tag = 'Functions'
        xml = ET.tostring(root, encoding='unicode')
        self.assertTrue(validate_document(xml)['valid'])
        migrated = migrate_xml(xml)
        self.assertEqual(ET.fromstring(migrated['xml']).findtext('Object/FunctionsClass'), '12.01.006.001')
        self.assertTrue(any('/Object/FunctionsClass' in message and 'Код перенесён' in message
                            for message in migrated['warnings']))

        functions.text = 'Тестовое назначение объекта без кода'
        migrated = migrate_xml(ET.tostring(root, encoding='unicode'))
        self.assertIsNone(ET.fromstring(migrated['xml']).find('Object/FunctionsClass'))
        self.assertTrue(any('/Object/Functions:' in message and 'доступен в оригинале' in message
                            for message in migrated['warnings']))

    def test_empty_legacy_address_fields_do_not_replace_filled_fields(self):
        root = self.legacy_document('01.02')
        address = root.find('Object/Address')
        city = ET.Element('City')
        city.text = 'Новосибирск'
        address.insert(1, city)
        address.find('Note').text = '  '
        ET.SubElement(address, 'Room')
        migrated = migrate_xml(ET.tostring(root, encoding='unicode'))
        russian_address = ET.fromstring(migrated['xml']).find('Object/Address/RussianAddress')
        self.assertEqual(russian_address.findtext('City'), 'Новосибирск')
        self.assertIsNone(russian_address.find('Note'))
        self.assertIsNone(russian_address.find('Room'))

    def test_unmatched_summary_narrative_never_turns_into_a_conclusion(self):
        root = self.legacy_document('01.00')
        summary = root.find('Summary/ExaminationSummary')
        summary.text = 'Сведения о результатах приведены в исходном заключении.'
        migrated = migrate_xml(ET.tostring(root, encoding='unicode'))
        converted = ET.fromstring(migrated['xml']).find('Summary/ExaminationSummary')
        self.assertEqual(len(converted), 0)
        self.assertTrue(any('/Summary/ExaminationSummary:' in message and 'Полный текст' in message
                            for message in migrated['warnings']))
