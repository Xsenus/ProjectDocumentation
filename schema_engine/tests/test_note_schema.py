import unittest
from pathlib import Path
from uuid import UUID
from xml.etree import ElementTree as ET

from schema_engine.preview import render_preview
from schema_engine.schema import form_schema, new_document, validate_document


FIXTURE = Path(__file__).with_name('fixtures') / 'explanatorynote-01-07.xml'


class ExplanatoryNoteSchemaTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.current = FIXTURE.read_text(encoding='utf-8')

    def test_official_schema_preserves_uniqueness_constraints(self):
        result = validate_document(self.current)
        self.assertTrue(result['valid'], result['errors'])
        self.assertEqual(result['validationBasis'], 'official-xsd')
        duplicate = self.current.replace('<LandCategory>2</LandCategory>', '<LandCategory>2</LandCategory>' * 2)
        result = validate_document(duplicate)
        self.assertFalse(result['valid'])
        self.assertTrue(any('NonIndustrialObjectLandCategoryUnique' in error['message'] for error in result['errors']))

    def test_profile_and_new_document_use_note_root_and_version(self):
        profile = form_schema('explanatorynote')
        self.assertEqual(profile['root']['name'], 'ExplanatoryNote')
        self.assertEqual(profile['version'], '01.07')
        self.assertEqual(form_schema('explanatorynote', '01.05')['version'], '01.05')
        attributes = profile['types']['tNonIndustrialObject']['attributes']
        self.assertTrue(any(attribute['name'] == 'ObjectID' and attribute['required'] for attribute in attributes))

        root = ET.fromstring(new_document('explanatorynote'))
        self.assertEqual(root.tag, 'ExplanatoryNote')
        self.assertEqual(root.get('SchemaVersion'), '01.07')
        self.assertEqual(UUID(root.get('id')).version, 4)
        self.assertEqual(len(root), 0)

    def test_legacy_wire_provenance_and_missing_version_with_modern_schema_hint(self):
        result = validate_document('<ExplanatoryNote/>')
        self.assertEqual(result['version'], '01.03')
        self.assertEqual(result['validationBasis'], 'legacy-wire')
        hinted = '<ExplanatoryNote xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' \
                 'xsi:noNamespaceSchemaLocation="explanatorynote-01-07.xsd"/>'
        result = validate_document(hinted)
        self.assertFalse(result['valid'])
        self.assertIsNone(result['version'])

    def test_official_preview_contains_note_content_and_csp(self):
        html = render_preview(self.current)
        self.assertIn('Тестовый объект записки', html)
        self.assertIn('Content-Security-Policy', html)
        with self.assertRaises(ValueError):
            render_preview('<ExplanatoryNote SchemaVersion="01.03"/>')
