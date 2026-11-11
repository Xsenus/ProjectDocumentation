import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.migration import migrate_xml
from schema_engine.preview import render_preview
from schema_engine.schema import form_schema, new_conclusion, validate_conclusion


FIXTURE = Path(__file__).with_name('fixtures') / 'conclusion-01-04.xml'


class ConclusionSchemaTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.current = FIXTURE.read_text(encoding='utf-8')

    def test_current_valid_fixture_and_official_interfield_rules(self):
        result = validate_conclusion(self.current)
        self.assertTrue(result['valid'], result['errors'])
        changed = self.current.replace('<ExaminationStage>1</ExaminationStage>', '<ExaminationStage>2</ExaminationStage>')
        result = validate_conclusion(changed)
        self.assertFalse(result['valid'])
        self.assertTrue(any(
            error.get('code') == 'xsd-assertion' and 'ранее выданных заключениях' in error['message']
            for error in result['errors']
        ), result['errors'])

    def test_legacy_export_and_upgrade_preserve_identity_and_unmapped_data_is_reported(self):
        legacy = migrate_xml(self.current, '01.03')
        self.assertEqual(legacy['targetVersion'], '01.03')
        self.assertTrue(any('OKTMOCode' in warning for warning in legacy['warnings']))
        legacy_validation = validate_conclusion(legacy['xml'])
        self.assertTrue(legacy_validation['valid'], legacy_validation['errors'])

        upgraded = migrate_xml(legacy['xml'])
        old_root = ET.fromstring(legacy['xml'])
        root = ET.fromstring(upgraded['xml'])
        self.assertEqual(root.get('ConclusionGUID'), old_root.get('ConclusionGUID'))
        self.assertEqual(root.findtext('Object/Name'), old_root.findtext('Object/Name'))
        self.assertEqual(root.findtext('ExpertOrganization/Address/RussianAddress/RegionCode'), '54')
        self.assertIsNone(root.find('ExpertOrganization/Address/RussianAddress/OKTMOCode'))
        self.assertTrue(any('OKTMOCode' in warning for warning in upgraded['warnings']))
        self.assertFalse(validate_conclusion(upgraded['xml'])['valid'])

    def test_new_document_does_not_insert_unconfirmed_values(self):
        root = ET.fromstring(new_conclusion())
        self.assertEqual(root.attrib['SchemaVersion'], '01.04')
        self.assertIn('ConclusionGUID', root.attrib)
        self.assertEqual(len(root), 0)

    def test_form_profile_reaches_reference_types(self):
        profile = form_schema()
        self.assertEqual(profile['root']['name'], 'Conclusion')
        root = profile['types'][profile['root']['type']]
        self.assertIn('Object', {child.get('name') for child in root['particle']['children']})
        self.assertIn('tRussianAddress', profile['types'])
        attributes = profile['types']['tObject']['attributes']
        self.assertTrue(any(attr['name'] == 'Unique' and attr['required'] for attr in attributes))

    def test_rejects_dtd_before_validation_and_conversion(self):
        xml = '<!DOCTYPE Conclusion [<!ENTITY value "test">]><Conclusion SchemaVersion="01.03">&value;</Conclusion>'
        result = validate_conclusion(xml)
        self.assertFalse(result['valid'])
        self.assertIn('DTD', result['errors'][0]['message'])
        with self.assertRaises(ValueError):
            migrate_xml(xml)

    def test_official_preview_uses_the_current_document(self):
        result = render_preview(self.current)
        self.assertIn('Тестовый объект', result)
        self.assertIn('Content-Security-Policy', result)
