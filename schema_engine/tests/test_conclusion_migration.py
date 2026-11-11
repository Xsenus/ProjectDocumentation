import unittest
from pathlib import Path
from xml.etree import ElementTree as ET

from schema_engine.migration import migrate_xml
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

    def test_older_versions_are_not_silently_reinterpreted(self):
        for root in ('Conclusion', 'ExplanatoryNote'):
            for version in ('01.00', '01.01', '01.02'):
                with self.subTest(root=root, version=version):
                    with self.assertRaisesRegex(ValueError, f'Версия {version} .* не поддерживается'):
                        migrate_xml(f'<{root} SchemaVersion="{version}"/>')
