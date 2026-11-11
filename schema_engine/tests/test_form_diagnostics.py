import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.preview import render_preview
from schema_engine.schema import new_document, validate_document


FIXTURES = Path(__file__).with_name('fixtures')


class FormDiagnosticsTests(unittest.TestCase):
    def test_empty_and_partial_documents_identify_required_fields(self):
        cases = (
            ('conclusion', '/Conclusion/ExpertOrganization'),
            ('explanatorynote', '/ExplanatoryNote/ExplanatoryNoteNumber'),
        )
        for family, path in cases:
            with self.subTest(family=family):
                result = validate_document(new_document(family))
                self.assertFalse(result['valid'])
                self.assertEqual(result['errors'][0]['path'], path)
                self.assertEqual(result['errors'][0]['code'], 'required-element')

        xml = (FIXTURES / 'explanatorynote-01-07.xml').read_text(encoding='utf-8')
        xml = xml.replace('<Name>Тестовый объект записки</Name>', '')
        xml = xml.replace(' ObjectID="object-test"', '').replace('Placement="1"', 'Placement="0"')
        result = validate_document(xml)
        self.assertFalse(result['valid'])
        errors = {error['path']: error for error in result['errors']}
        self.assertEqual(errors['/ExplanatoryNote/NonIndustrialObject/Name']['code'], 'required-element')
        self.assertEqual(errors['/ExplanatoryNote/NonIndustrialObject/@ObjectID']['code'], 'required-attribute')
        self.assertIn('допустимое', errors['/ExplanatoryNote/NonIndustrialObject/@Placement']['message'])

    def test_invalid_value_in_second_document_keeps_its_index(self):
        xml = (FIXTURES / 'explanatorynote-01-07.xml').read_text(encoding='utf-8')
        fragment = xml.split('<ProjectInitialDocuments>', 1)[1].split('</ProjectInitialDocuments>', 1)[0]
        xml = xml.replace('</ProjectInitialDocuments>', fragment.replace('2026-01-02', '2026-02-31') + '</ProjectInitialDocuments>')
        result = validate_document(xml)
        self.assertFalse(result['valid'])
        self.assertTrue(any(error['path'] == '/ExplanatoryNote/ProjectInitialDocuments/Document[2]/DocDate'
                            for error in result['errors']), result['errors'])

    def test_year_error_explains_the_numeric_limit_at_the_field(self):
        root = ET.parse(FIXTURES / 'explanatorynote-01-07.xml').getroot()
        for year, message in (('2020', 'не меньше 2021'), ('10000', 'не больше 9999')):
            with self.subTest(year=year):
                root.find('ExplanatoryNoteYear').text = year
                errors = validate_document(ET.tostring(root, encoding='unicode'))['errors']
                self.assertTrue(any(error['path'] == '/ExplanatoryNote/ExplanatoryNoteYear'
                                    and message in error['message'] for error in errors), errors)

    def test_desktop_preview_removes_the_negative_margin_only_from_the_generated_wrapper(self):
        for name, value in (('conclusion-01-04.xml', 'Проверка сдвига'),
                            ('explanatorynote-01-07.xml', 'Проверка полей')):
            with self.subTest(name=name):
                root = ET.parse(FIXTURES / name).getroot()
                if root.tag == 'Conclusion':
                    root.find('ExaminationObject/Name').text = value
                else:
                    root.find('NonIndustrialObject/Name').text = value
                xml = ET.tostring(root, encoding='unicode')
                original = render_preview(xml)
                desktop = render_preview(xml, rich_edit=True)
                self.assertIn('margin-left: -400px', original)
                self.assertNotIn('margin-left: -400px', desktop)
                self.assertIn(value, desktop)
                if root.tag == 'ExplanatoryNote':
                    self.assertIn('<div class="frame">', original)
                    self.assertNotIn('<div class="frame">', desktop)

    def test_missing_snils_points_to_the_required_field_before_expert_type(self):
        root = ET.parse(FIXTURES / 'conclusion-01-04.xml').getroot()
        expert = root.find('Experts/Expert')
        expert.remove(expert.find('SNILS'))
        errors = validate_document(ET.tostring(root, encoding='unicode'))['errors']
        self.assertTrue(any(error.get('code') == 'required-element' and error['path'] == '/Conclusion/Experts/Expert/SNILS'
                            for error in errors), errors)

    def test_missing_conditional_fields_point_to_the_editable_xml_path(self):
        xml = '<Conclusion SchemaVersion="01.04"><ExaminationObject><ExaminationType>1</ExaminationType></ExaminationObject></Conclusion>'
        errors = validate_document(xml)['errors']
        paths = {error['path'] for error in errors if error.get('code') == 'xsd-assertion'}
        self.assertIn('/Conclusion/ExaminationObject/EngineeringSurveysIM', paths)
        self.assertIn('/Conclusion/EngineeringSurveyAddress', paths)

        xml = (FIXTURES / 'conclusion-01-04.xml').read_text(encoding='utf-8')
        xml = xml.replace('<ExaminationStage>1</ExaminationStage>', '<ExaminationStage>2</ExaminationStage>')
        errors = validate_document(xml)['errors']
        self.assertTrue(any(error.get('code') == 'xsd-assertion' and error['path'] == '/Conclusion/PreviousConclusions'
                            for error in errors), errors)

    def test_preview_accepts_large_image_inside_xml_size_limit(self):
        xml = (FIXTURES / 'explanatorynote-01-07.xml').read_text(encoding='utf-8')
        data = 'A' * 10_000_004
        xml = xml.replace('</DesignerAssurance>', f'<Image Type="png"><ImageData>{data}</ImageData></Image></DesignerAssurance>')
        html = render_preview(xml)
        self.assertIn(data, html)
