import unittest

from xmlschema.validators import XsdElement, XsdGroup

from schema_engine.documents import DOCUMENTS
from schema_engine.schema import XSD, _compiled_schema, form_schema


class FormContractTests(unittest.TestCase):
    def test_form_profiles_represent_every_reachable_xsd_field(self):
        for family, document in DOCUMENTS.items():
            for version in document.versions:
                with self.subTest(family=family, version=version):
                    self.check_profile(family, version, document.root)

    def check_profile(self, family, version, root_name):
        profile = form_schema(family, version)
        compiled = _compiled_schema(family, version)
        self.assertEqual((profile['family'], profile['version']), (family, version))
        pending = []
        self.check_particle(compiled.elements[root_name], profile['root'], pending)
        seen = {}
        while pending:
            component, key = pending.pop()
            if key in seen:
                self.assertIs(seen[key], component)
                continue
            seen[key] = component
            entry = profile['types'][key]
            if not component.is_complex():
                self.assertEqual(entry['kind'], 'simple')
                self.assertEqual(entry['base'], component.root_type.local_name)
                self.assertIs(entry['allowsEmpty'], component.is_valid(''))
                enumeration = component.get_facet(XSD + 'enumeration')
                if enumeration is not None:
                    self.assertEqual([value['value'] for value in entry['enumeration']],
                                     [node.get('value') for node in enumeration])
                else:
                    self.assertNotIn('enumeration', entry)
                continue

            self.assertEqual(entry['kind'], 'complex')
            attributes = component.attributes.values()
            self.assertEqual(len(attributes), len(entry['attributes']))
            for attribute, field in zip(attributes, entry['attributes']):
                self.assertEqual(field['name'], attribute.local_name)
                self.assertEqual(field['required'], attribute.use == 'required')
                self.check_declared_values(attribute, field)
                pending.append((attribute.type, field['type']))

            if isinstance(component.content, XsdGroup):
                self.check_particle(component.content, entry['particle'], pending)
            elif component.has_simple_content():
                pending.append((component.content, entry['textType']))
        self.assertEqual(set(seen), set(profile['types']))

    def check_particle(self, component, field, pending):
        self.assertEqual((field['min'], field['max']), (component.min_occurs, component.max_occurs))
        if isinstance(component, XsdElement):
            self.assertEqual((field['kind'], field['name']), ('element', component.local_name))
            self.assertEqual(field['nillable'], component.nillable)
            self.check_declared_values(component, field)
            pending.append((component.type, field['type']))
            return
        self.assertIsInstance(component, XsdGroup)
        self.assertEqual(field['kind'], component.model)
        self.assertEqual(len(field['children']), len(component))
        for child, child_field in zip(component, field['children']):
            self.check_particle(child, child_field, pending)

    def check_declared_values(self, component, field):
        for name in ('default', 'fixed'):
            self.assertEqual(field.get(name), getattr(component, name))
