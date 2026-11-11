from functools import lru_cache
from itertools import islice
import re
from uuid import uuid4
from xml.etree import ElementTree as ET

from lxml import etree
from xmlschema import XMLSchema11
from xmlschema.validators import XMLSchemaChildrenValidationError, XsdAssert, XsdElement, XsdGroup
from xmlschema.validators.facets import (
    XsdEnumerationFacets, XsdLengthFacet, XsdMaxLengthFacet, XsdMinLengthFacet, XsdPatternFacets,
)

from .documents import (
    DEFAULT_FAMILY, DOCUMENTS, FAMILIES_BY_ROOT, SCHEMA_DIRECTORY, document_version,
    legacy_note_format, resolve_version, schema_filename, source_versions,
)
from .xmlio import parse_document


XSD = '{http://www.w3.org/2001/XMLSchema}'
XML_ID = '{http://www.w3.org/XML/1998/namespace}id'
MAX_VALIDATION_ERRORS = 200
MISSING_ATTRIBUTE = re.compile(r"^missing required attribute '([^']+)'$")
INVALID_ATTRIBUTE = re.compile(r"^attribute ([^=\s]+)=")
FIXED_ATTRIBUTE = re.compile(r"^attribute '([^']+)' has a fixed value '([^']*)'$")


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
    versions = source_versions(family)
    if version not in versions:
        return {'valid': False, **metadata, 'errors': [
            {'path': path + '/@SchemaVersion', 'message': f'Укажите версию {", ".join(versions)}.'},
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
    path = error.path or root_path
    reason = error.reason or error.message
    result = {'path': path, 'message': reason}
    if isinstance(error.validator, XsdAssert):
        result['code'] = 'xsd-assertion'
        result['message'] = _documentation(error.validator) or reason
        return result
    if isinstance(error, XMLSchemaChildrenValidationError):
        return _children_error(error, result)
    if match := MISSING_ATTRIBUTE.fullmatch(reason):
        name, = match.groups()
        return {'path': f'{path}/@{name}', 'code': 'required-attribute', 'message': f'Заполните обязательный атрибут {name}.'}
    if match := FIXED_ATTRIBUTE.fullmatch(reason):
        name, value = match.groups()
        return {'path': f'{path}/@{name}', 'code': 'fixed-value', 'message': f'Допустимое значение атрибута {name} — {value}.'}
    if match := INVALID_ATTRIBUTE.match(reason):
        name, = match.groups()
        result['path'] = f'{path}/@{name}'
    if message := _facet_message(error.validator):
        result['message'] = message
    return result


def _children_error(error, result):
    expected_elements = [element for element in error.expected or ()
                         if isinstance(element, XsdElement) and element.local_name]
    expected = tuple(dict.fromkeys(element.local_name for element in expected_elements))
    child = error.invalid_child
    children = list(error.elem) if error.elem is not None else []
    present = {node.tag for node in children}
    known_names = {element.name for element in error.validator.iter_elements()}
    if len(expected) == 1 and expected[0] not in present and (
        child is None or child.tag in known_names
    ):
        name, = expected
        return {'path': f'{result["path"]}/{name}', 'code': 'required-element', 'message': f'Заполните обязательный элемент {name}.'}
    if child is not None:
        # The validator may list both the missing required field and later members of the same sequence.
        for index, element in enumerate(expected_elements):
            if element.name != child.tag:
                continue
            group = element.parent
            if isinstance(group, XsdGroup) and group.model == 'sequence' and group.max_occurs == 1:
                for preceding in expected_elements[:index]:
                    if preceding.parent is group and preceding.min_occurs > 0 and preceding.name not in present:
                        name = preceding.local_name
                        return {'path': f'{result["path"]}/{name}', 'code': 'required-element',
                                'message': f'Заполните обязательный элемент {name}.'}
            break
        siblings = [node for node in children if node.tag == child.tag]
        suffix = f'[{siblings.index(child) + 1}]' if len(siblings) > 1 else ''
        name = child.tag.rsplit('}', 1)[-1]
        result['path'] = f'{result["path"]}/{name}{suffix}'
        result['code'] = 'unexpected-element'
        result['message'] = f'Элемент {name} недопустим в этой позиции.'
        if expected:
            result['message'] += f' Ожидается {", ".join(expected)}.'
    elif expected:
        result['code'] = 'incomplete-element'
        result['message'] = f'Заполните вложенные элементы. Ожидается {", ".join(expected)}.'
    return result


def _facet_message(validator):
    if isinstance(validator, XsdEnumerationFacets):
        values = [str(value) for value in validator.enumeration]
        if len(values) <= 8:
            return f'Выберите допустимое значение: {", ".join(values)}.'
        return 'Выберите значение из справочника.'
    if isinstance(validator, XsdPatternFacets):
        return 'Значение не соответствует формату, заданному в схеме.'
    if isinstance(validator, XsdLengthFacet):
        return f'Длина значения должна быть {validator.value}.'
    if isinstance(validator, XsdMinLengthFacet):
        return f'Минимальная длина значения — {validator.value}.'
    if isinstance(validator, XsdMaxLengthFacet):
        return f'Максимальная длина значения — {validator.value}.'
    return ''


def _declared_values(component):
    return {name: value for name in ('default', 'fixed') if (value := getattr(component, name)) is not None}


def _occurs(component):
    return {'min': component.min_occurs, 'max': component.max_occurs}


class _FormSchema:
    def __init__(self):
        self.types = {}
        self.type_ids = {}

    def element(self, component):
        return {
            'kind': 'element', 'name': component.local_name, 'type': self.type_id(component.type),
            'documentation': _documentation(component), **_occurs(component), **_declared_values(component),
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
        return {
            'name': component.local_name, 'type': self.type_id(component.type),
            'required': component.use == 'required', 'documentation': _documentation(component),
            **_declared_values(component),
        }

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
