from functools import lru_cache
from itertools import islice
from uuid import uuid4
from xml.etree import ElementTree as ET

from lxml import etree
from xmlschema import XMLSchema11
from xmlschema.validators import XsdAssert, XsdElement, XsdGroup

from .documents import (
    DEFAULT_FAMILY, DOCUMENTS, FAMILIES_BY_ROOT, SCHEMA_DIRECTORY, document_version,
    legacy_note_format, resolve_version, schema_filename,
)
from .xmlio import parse_document


XSD = '{http://www.w3.org/2001/XMLSchema}'
XML_ID = '{http://www.w3.org/XML/1998/namespace}id'
MAX_VALIDATION_ERRORS = 200


@lru_cache(maxsize=None)
def _compiled_schema(family, version):
    source = SCHEMA_DIRECTORY / schema_filename(family, version)
    if family == 'explanatorynote' and not legacy_note_format(family, version):
        source = _note_validation_source(source)
    return XMLSchema11(source, allow='local', defuse='always')


def _note_validation_source(path):
    # The official note schemas repeat Name and Symbol xml:ids in documentation.
    # Keep the original files for download, and preserve all validation rules here.
    parser = etree.XMLParser(collect_ids=False, resolve_entities=False, no_network=True, load_dtd=False)
    root = etree.parse(str(path), parser).getroot()
    documentation_ids = {}
    for node in root.iter(XSD + 'documentation'):
        if value := node.get(XML_ID):
            documentation_ids.setdefault(value, []).append(node)
    for nodes in documentation_ids.values():
        if len(nodes) > 1:
            for node in nodes:
                del node.attrib[XML_ID]
    return etree.tostring(root, encoding='utf-8')


def new_document(family=DEFAULT_FAMILY, version=None):
    version = resolve_version(family, version)
    document = DOCUMENTS[family]
    root = ET.Element(document.root, {document.id_attribute: str(uuid4()), 'SchemaVersion': version})
    return ET.tostring(root, encoding='utf-8', xml_declaration=True).decode('utf-8')


def new_conclusion():
    return new_document()


def validate_conclusion(xml):
    return validate_document(xml, expected_family=DEFAULT_FAMILY)


def validate_document(xml, expected_family=None):
    try:
        root = parse_document(xml)
    except ET.ParseError as error:
        return {'valid': False, 'version': None, 'errors': [
            {'path': '/', 'message': f'Ошибка синтаксиса XML: {error}', 'line': error.position[0]},
        ]}
    except (ValueError, UnicodeError) as error:
        return {'valid': False, 'version': None, 'errors': [{'path': '/', 'message': str(error)}]}

    family = FAMILIES_BY_ROOT.get(root.tag)
    if family is None or (expected_family is not None and family != expected_family):
        expected = DOCUMENTS[expected_family].root if expected_family else 'Conclusion или ExplanatoryNote'
        return {'valid': False, 'version': root.get('SchemaVersion'), 'errors': [
            {'path': '/', 'message': f'Ожидается корневой элемент {expected} без пространства имён.'},
        ]}

    version = document_version(root, family)
    metadata = {'family': family, 'version': version}
    document = DOCUMENTS[family]
    path = '/' + document.root
    if version not in document.versions:
        return {'valid': False, **metadata, 'errors': [
            {'path': path + '/@SchemaVersion', 'message': f'Укажите версию {", ".join(document.versions)}.'},
        ]}

    metadata['validationBasis'] = 'legacy-wire' if legacy_note_format(family, version) else 'official-xsd'
    validation_errors = _compiled_schema(family, version).iter_errors(root, use_defaults=False)
    errors = [_validation_error(error, path) for error in islice(validation_errors, MAX_VALIDATION_ERRORS + 1)]
    if len(errors) > MAX_VALIDATION_ERRORS:
        errors[MAX_VALIDATION_ERRORS:] = [
            {'path': path, 'message': 'Показаны первые 200 ошибок. После исправления запустите проверку ещё раз.'},
        ]
    return {'valid': not errors, **metadata, 'errors': errors}


def _documentation(component):
    if component.annotation is None:
        return ''
    return '\n'.join(
        text for node in component.annotation.documentation
        if (text := ' '.join(''.join(node.itertext()).split()))
    )


def _validation_error(error, root_path):
    result = {'path': error.path or root_path, 'message': error.reason or error.message}
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


def form_schema(family=DEFAULT_FAMILY, version=None):
    return _form_schema(family, resolve_version(family, version))


@lru_cache(maxsize=None)
def _form_schema(family, version):
    profile = _FormSchema()
    root = profile.element(_compiled_schema(family, version).elements[DOCUMENTS[family].root])
    return {'family': family, 'version': version, 'root': root, 'types': profile.types}
