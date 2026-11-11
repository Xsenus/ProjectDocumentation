from functools import lru_cache
from itertools import islice
from pathlib import Path
from uuid import uuid4
from xml.etree import ElementTree as ET

from xmlschema import XMLSchema11
from xmlschema.validators import XsdAssert, XsdElement, XsdGroup

from .xmlio import parse_document


CURRENT_VERSION = '01.04'
SCHEMA_DIRECTORY = Path(__file__).with_name('schemas')
CONCLUSION_SCHEMAS = {
    '01.03': 'conclusion-01-03.xsd',
    '01.04': 'conclusion-01-04.xsd',
}
XSD = '{http://www.w3.org/2001/XMLSchema}'
MAX_VALIDATION_ERRORS = 200


@lru_cache(maxsize=len(CONCLUSION_SCHEMAS))
def conclusion_schema(version):
    return XMLSchema11(SCHEMA_DIRECTORY / CONCLUSION_SCHEMAS[version], allow='local', defuse='always')


def new_conclusion():
    root = ET.Element('Conclusion', {'ConclusionGUID': str(uuid4()), 'SchemaVersion': CURRENT_VERSION})
    return ET.tostring(root, encoding='utf-8', xml_declaration=True).decode('utf-8')


def validate_conclusion(xml):
    try:
        root = parse_document(xml)
    except ET.ParseError as error:
        return {'valid': False, 'version': None, 'errors': [
            {'path': '/', 'message': f'Ошибка синтаксиса XML: {error}', 'line': error.position[0]},
        ]}
    except (ValueError, UnicodeError) as error:
        return {'valid': False, 'version': None, 'errors': [{'path': '/', 'message': str(error)}]}

    version = root.get('SchemaVersion')
    if root.tag != 'Conclusion':
        return {'valid': False, 'version': version, 'errors': [
            {'path': '/', 'message': 'Ожидается корневой элемент Conclusion без пространства имён.'},
        ]}
    if version not in CONCLUSION_SCHEMAS:
        return {'valid': False, 'version': version, 'errors': [
            {'path': '/Conclusion/@SchemaVersion', 'message': 'Укажите версию заключения 01.03 или 01.04.'},
        ]}

    validation_errors = conclusion_schema(version).iter_errors(root, use_defaults=False)
    errors = [_validation_error(error) for error in islice(validation_errors, MAX_VALIDATION_ERRORS + 1)]
    if len(errors) > MAX_VALIDATION_ERRORS:
        errors[MAX_VALIDATION_ERRORS:] = [
            {'path': '/Conclusion', 'message': 'Показаны первые 200 ошибок. После исправления запустите проверку ещё раз.'},
        ]
    return {'valid': not errors, 'version': version, 'errors': errors}


def _documentation(component):
    if component.annotation is None:
        return ''
    return '\n'.join(
        text for node in component.annotation.documentation
        if (text := ' '.join(''.join(node.itertext()).split()))
    )


def _validation_error(error):
    result = {'path': error.path or '/Conclusion', 'message': error.reason or error.message}
    if isinstance(error.validator, XsdAssert):
        result['code'] = 'xsd-assertion'
        result['message'] = _documentation(error.validator) or result['message']
    return result


def _occurs(component):
    return {'min': component.min_occurs, 'max': component.max_occurs}


class _FormSchema:
    def __init__(self):
        self.types = {}
        self.type_ids = {}

    def element(self, component):
        return {
            'kind': 'element', 'name': component.local_name, 'type': self.type_id(component.type),
            'documentation': _documentation(component), **_occurs(component),
        }

    def particle(self, component):
        if isinstance(component, XsdElement):
            return self.element(component)
        if isinstance(component, XsdGroup):
            return {
                'kind': component.model, 'children': [self.particle(child) for child in component],
                **_occurs(component),
            }
        raise TypeError(f'Unsupported XSD form particle: {type(component).__name__}')

    def attribute(self, component):
        result = {
            'name': component.local_name, 'type': self.type_id(component.type),
            'required': component.use == 'required', 'documentation': _documentation(component),
        }
        for property_name in ('default', 'fixed'):
            value = getattr(component, property_name)
            if value is not None:
                result[property_name] = value
        return result

    def type_id(self, component):
        identity = id(component)
        if identity in self.type_ids:
            return self.type_ids[identity]

        key = component.name or f'@{len(self.type_ids) + 1}'
        self.type_ids[identity] = key
        result = {'kind': 'complex' if component.is_complex() else 'simple', 'documentation': _documentation(component)}
        self.types[key] = result
        if component.is_complex():
            result['attributes'] = [self.attribute(attribute) for attribute in component.attributes.values()]
            if isinstance(component.content, XsdGroup):
                result['particle'] = self.particle(component.content)
            elif component.has_simple_content():
                result['textType'] = self.type_id(component.content)
        else:
            result['base'] = component.root_type.local_name
            enumeration = component.get_facet(XSD + 'enumeration')
            if enumeration is not None:
                result['enumeration'] = [
                    {'value': node.get('value'), 'label': (node.findtext(f'{XSD}annotation/{XSD}documentation') or '').strip()}
                    for node in enumeration
                ]
        return key


@lru_cache(maxsize=1)
def form_schema():
    profile = _FormSchema()
    root = profile.element(conclusion_schema(CURRENT_VERSION).elements['Conclusion'])
    return {'version': CURRENT_VERSION, 'root': root, 'types': profile.types}
