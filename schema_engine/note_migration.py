"""Loss-aware migration for explanatory notes and the app's legacy XML contract."""

from collections import Counter
from functools import lru_cache
from xml.etree import ElementTree as ET

from .documents import DOCUMENTS, SCHEMA_DIRECTORY, SCHEMA_LOCATION, schema_filename
from .migration import (
    ADDRESS_VARIANTS, LEGACY_NAMES, PARTICLES, PERSON_AND_ORGANIZATION_NAMES,
    XSD, _Particle, _Schema, _check_required,
    _downgrade_address, _ordered_fields, _renamed, _upgrade_address, _with_children,
)


FAMILY = 'explanatorynote'
VERSIONS = DOCUMENTS[FAMILY].versions
SECTION_WRAPPERS = {
    'tProjectDocumentSection', 'tSection5', 'tSectionEstimate', 'tOtherDocumentsSection',
}
CHIEF_ROLES = {'ChiefProjectEngineer', 'ChiefProjectArchitect', 'ChiefProjectEngineerSurvey'}
ESTIMATE_NAMES = {'ObjectEstimates': 'Объектные сметы', 'LocalEstimates': 'Локальные сметы'}
ESTIMATE_FIELDS = {value.casefold(): key for key, value in ESTIMATE_NAMES.items()}


class _NoteSchema(_Schema):
    def __init__(self, version):
        schema = ET.parse(SCHEMA_DIRECTORY / schema_filename(FAMILY, version)).getroot()
        self.root = schema.find(f'{XSD}element[@name="ExplanatoryNote"]')
        self.complex_types = self._named(schema, 'complexType')
        self.simple_types = self._named(schema, 'simpleType')
        self.groups = self._named(schema, 'group')
        self.global_attributes = self._named(schema, 'attribute')

    @lru_cache(maxsize=None)
    def layout(self, definition):
        content = tuple(self._inherited_content(self._content(definition)))
        fields = tuple(self._particle(child) for base in content for child in base if child.tag in PARTICLES)
        attributes = {}
        for base in content:
            for attribute in base.findall(XSD + 'attribute'):
                name = attribute.get('name') or attribute.attrib['ref']
                if 'ref' in attribute.attrib:
                    reference = self.global_attributes[name]
                    attributes[name] = {**reference.attrib, **attribute.attrib}
                else:
                    attributes[name] = attribute.attrib
        return _Particle('sequence', children=fields), attributes


@lru_cache(maxsize=len(VERSIONS))
def _note_schema(version):
    return _NoteSchema(version)


def _limits(particle, parent_maximum=1):
    maximum = None if parent_maximum is None or particle.maximum is None else parent_maximum * particle.maximum
    if particle.element is not None:
        yield particle.name, maximum
    else:
        for child in particle.children:
            yield from _limits(child, maximum)


def _convert_address(element, type_name, source_version, target_version, path, warnings):
    local_warnings = []
    if target_version == '01.07':
        converted = _upgrade_address(element, type_name, path, local_warnings)
    else:
        converted = _downgrade_address(element, path, local_warnings)
    old_version = source_version if target_version == '01.07' else target_version
    warnings.extend(warning.replace('01.04', '01.07').replace('01.03', old_version) for warning in local_warnings)
    return converted


def _state_owner(element, target_version, path, warnings):
    wrappers = [child for child in element if child.tag in {'Organization', 'ForeignOrganization'}]
    if target_version == '01.07' and wrappers:
        selected = wrappers[0]
        if len(wrappers) > 1:
            warnings.append(f'{path}: перенесён первый владелец. Остальные варианты доступны в оригинале.')
        if selected.tag == 'ForeignOrganization' and selected.find('RAFP') is None:
            warnings.append(f'{path}/RAFP: укажите номер аккредитации иностранной организации.')
        result = _with_children(element, [*selected, *(child for child in element if child not in wrappers)])
        for name, value in selected.attrib.items():
            if name in result.attrib and result.attrib[name] != value:
                warnings.append(f'{path}/@{name}: конфликт значений владельца. Сверьте оригинал.')
            else:
                result.set(name, value)
        return result
    if target_version != '01.07' and not wrappers and len(element):
        name = 'ForeignOrganization' if element.find('RAFP') is not None and element.find('OGRN') is None else 'Organization'
        organization = ET.Element(name)
        organization.extend(element)
        return _with_children(element, [organization])
    return element


def _signers(element, target_version, path, warnings):
    convert = CHIEF_ROLES if target_version == '01.03' else {'ChiefProjectEngineerSurvey'} if target_version == '01.05' else set()
    children = []
    for child in element:
        if child.tag in convert:
            children.append(_renamed(child, 'Signer'))
            warnings.append(f'{path}/{child.tag}: имя перенесено в общий список подписантов. Отдельная роль недоступна в {target_version}.')
        else:
            children.append(child)
    return _with_children(element, children)


def _assurance(element, source_version, target_version, path, warnings):
    if source_version == '01.03' and target_version != '01.03':
        content = element.findall('Assurance')
        for name in element.attrib:
            warnings.append(f'{path}/@{name}: атрибут исходной обёртки заверения доступен в оригинале.')
        for child in element:
            if child.tag != 'Assurance':
                warnings.append(f'{path}/{child.tag}: структура заверения изменилась. Сведения доступны в оригинале.')
        if not content:
            return _with_children(element, [])
        if len(content) > 1:
            warnings.append(f'{path}/Assurance: перенесён первый блок заверения. Остальные блоки доступны в оригинале.')
        return _renamed(content[0], element.tag)
    if source_version != '01.03' and target_version == '01.03':
        container = ET.Element(element.tag)
        if len(element) or element.attrib or (element.text or '').strip():
            container.append(_renamed(element, 'Assurance'))
        return container
    return element


def _separate_estimates(container, path, warnings):
    groups = {}
    for child in container:
        category = ESTIMATE_FIELDS.get((child.get('Name') or '').strip().casefold()) if child.tag == 'SubSection' else None
        if category is None or not len(child):
            warnings.append(f'{path}/{child.tag}: категория сметы не определена. Распределите документы из оригинала между объектными и локальными сметами.')
            continue
        group = groups.setdefault(category, ET.Element(category))
        group.extend(child)
    if not len(container):
        warnings.append(f'{path}: объединённый раздел не содержит сведений для разделения на объектные и локальные сметы.')
    return list(groups.values())


def _combine_estimates(children):
    combined = ET.Element('ObjectLocalEstimates')
    result = []
    for child in children:
        label = ESTIMATE_NAMES.get(child.tag)
        if label is None:
            result.append(child)
            continue
        if not len(combined):
            result.append(combined)
        subsection = ET.SubElement(combined, 'SubSection', {'Name': label})
        subsection.extend(child)
    return result


def _flatten_other_subsections(element, path, warnings):
    children = []
    for index, child in enumerate(element, start=1):
        if child.tag != 'SubSection':
            children.append(child)
            continue
        child_path = f'{path}/SubSection[{index}]'
        warnings.append(f'{child_path}: документы перенесены в родительский раздел. Заголовок и структура подраздела доступны в оригинале.')
        children.extend(_flatten_other_subsections(child, child_path, warnings))
    return _with_children(element, children)


def _section(element, type_name, source_version, target_version, path, warnings):
    if source_version == '01.03' and target_version != '01.03':
        if element.find('SectionContent') is not None or element.find('NotDeveloped') is not None:
            return element
        content = ET.Element('SectionContent')
        for child in element:
            if child.tag == 'ObjectLocalEstimates' and type_name == 'tSectionEstimate':
                content.extend(_separate_estimates(child, f'{path}/ObjectLocalEstimates', warnings))
            else:
                content.append(child)
        return _with_children(element, [content]) if len(content) else element
    if source_version != '01.03' and target_version == '01.03':
        children = []
        for child in element:
            if child.tag == 'SectionContent':
                children.extend(child)
            elif child.tag == 'NotDeveloped':
                warnings.append(f'{path}/NotDeveloped: объяснение отсутствия раздела не имеет отдельного поля в 01.03 и доступно в оригинале.')
            else:
                children.append(child)
        if type_name == 'tSectionEstimate':
            children = _combine_estimates(children)
        return _with_children(element, children)
    return element


def _prepare(source, name, type_name, source_version, target_version, path, warnings):
    if type_name in {'tAddress', 'tPostAddress', 'tObjectAddress'}:
        if target_version == '01.07' or any(child.tag in ADDRESS_VARIANTS for child in source):
            source = _convert_address(source, type_name, source_version, target_version, path, warnings)
    if name == 'Owner' and '/StateCustomerSource' in path:
        source = _state_owner(source, target_version, path, warnings)
    if name == 'Signers':
        source = _signers(source, target_version, path, warnings)
    if name == 'DesignerAssurance':
        source = _assurance(source, source_version, target_version, path, warnings)
    if type_name in SECTION_WRAPPERS:
        source = _section(source, type_name, source_version, target_version, path, warnings)
    if source_version == '01.03' and type_name == 'tProjectDocumentNotNecessarySectionContent':
        source = _flatten_other_subsections(source, path, warnings)
    return source


def _candidate(child, fields, target_version, path, warnings):
    name = child.tag
    if target_version == '01.07' and name == 'ForeignOrganization' and 'Organization' in fields:
        if child.find('RAFP') is None:
            warnings.append(f'{path}/RAFP: укажите номер аккредитации иностранной организации.')
        return 'Organization'
    if target_version != '01.07' and name == 'Organization' and 'ForeignOrganization' in fields:
        if child.find('RAFP') is not None and child.find('OGRN') is None:
            return 'ForeignOrganization'
    if name in fields:
        return name
    if target_version == '01.03' and name == 'OtherDocument' and 'Document' in fields:
        warnings.append(f'{path}: совместимые реквизиты перенесены в формат документа 01.03.')
        return 'Document'
    if target_version == '01.07':
        if name == 'ProjectDocParticipant':
            return 'DocParticipant'
        if name == 'NOPRIZ':
            return 'NOPRIZNumber'
        return PERSON_AND_ORGANIZATION_NAMES.get(name, name)
    if name == 'DocParticipant':
        return 'ProjectDocParticipant'
    if name == 'NOPRIZNumber':
        return 'NOPRIZ'
    return LEGACY_NAMES.get(name, name)


def _valid_enum(value, values):
    return not values or value.strip() in values


def _attributes(result, source, allowed, schema, target_version, path, warnings):
    for name, value in source.attrib.items():
        if path == '/ExplanatoryNote' and name == 'SchemaLink':
            if value.strip():
                warnings.append(f'{path}/@SchemaLink: снята ссылка на схему исходной версии.')
        elif path == '/ExplanatoryNote' and name == SCHEMA_LOCATION:
            if target_version != '01.03':
                result.set(name, schema_filename(FAMILY, target_version))
            else:
                warnings.append(f'{path}/@noNamespaceSchemaLocation: ссылка снята. Исходная официальная XSD 01.03 недоступна.')
        elif name not in allowed:
            warnings.append(f'{path}/@{name}: атрибут не имеет аналога в {target_version} и доступен в оригинале.')
        else:
            values = schema.enumerated_values(allowed[name].get('type'))
            if not _valid_enum(value, values):
                if target_version != '01.07':
                    warnings.append(f'{path}/@{name}: значение отсутствует в справочнике {target_version} и доступно в оригинале.')
                    continue
                warnings.append(f'{path}/@{name}: значение отсутствует в справочнике {target_version}.')
            result.set(name, value.strip() if values else value)
    for name, definition in allowed.items():
        if definition.get('use') == 'required' and not (result.get(name) or '').strip():
            warnings.append(f'{path}/@{name}: заполните обязательное поле.')


def _project(source, name, definition, schema, source_version, target_version, path, warnings):
    type_name = definition.get('type')
    source = _prepare(source, name, type_name, source_version, target_version, path, warnings)
    particle, attributes = schema.layout(definition)
    fields = dict(_ordered_fields(particle))
    limits = dict(_limits(particle))
    result = ET.Element(name)
    _attributes(result, source, attributes, schema, target_version, path, warnings)

    if not fields:
        values = schema.enumerated_values(type_name)
        result.text = (source.text or '').strip() if values else source.text
        if values and not _valid_enum(result.text or '', values):
            warnings.append(f'{path}: значение отсутствует в справочнике {target_version}.')
        elif type_name in {'tNotEmptyString', 'non-empty-string'} and not (result.text or '').strip():
            warnings.append(f'{path}: в целевой схеме это поле не может быть пустым.')
    elif (source.text or '').strip():
        warnings.append(f'{path}: свободный текст не соответствует целевой структуре и доступен в оригинале.')

    accepted = []
    counts = Counter()
    source_counts = Counter(child.tag for child in source)
    positions = Counter()
    for child in source:
        positions[child.tag] += 1
        suffix = f'[{positions[child.tag]}]' if source_counts[child.tag] > 1 else ''
        child_path = f'{path}/{child.tag}{suffix}'
        candidate = _candidate(child, fields, target_version, child_path, warnings)
        if candidate not in fields:
            warnings.append(f'{child_path}: поле не имеет аналога в {target_version} и доступно в оригинале.')
            continue
        order, child_definition = fields[candidate]
        if target_version != '01.07':
            values = schema.enumerated_values(child_definition.get('type'))
            if not _valid_enum(child.text or '', values):
                warnings.append(f'{child_path}: значение отсутствует в справочнике {target_version} и доступно в оригинале.')
                continue
            maximum = limits[candidate]
            if maximum is not None and counts[candidate] >= maximum:
                warnings.append(f'{child_path}: превышено допустимое число элементов в {target_version}. Элемент доступен в оригинале.')
                continue
        accepted.append((order, candidate, child_definition, child))
        counts[candidate] += 1

    positions.clear()
    for _, candidate, child_definition, child in sorted(accepted, key=lambda item: item[0]):
        positions[candidate] += 1
        suffix = f'[{positions[candidate]}]' if counts[candidate] > 1 else ''
        result.append(_project(child, candidate, child_definition, schema, source_version, target_version,
                               f'{path}/{candidate}{suffix}', warnings))
    _check_required(particle, counts, path, warnings)
    if type_name == 'tObjectParts' and target_version != '01.03' and sum(counts.values()) == 1:
        warnings.append(f'{path}: в {target_version} требуется не менее двух частей объекта.')
    return result


def migrate_note(root: ET.Element, source_version: str, target_version: str) -> tuple[ET.Element, list[str]]:
    """Return a schema-ordered draft. Keep the source document independently."""
    if root.tag != 'ExplanatoryNote':
        raise ValueError('Ожидается корневой элемент ExplanatoryNote без пространства имён.')
    if not all(version in VERSIONS for version in (source_version, target_version)):
        raise ValueError('Поддерживаются версии пояснительной записки 01.03, 01.05 и 01.07.')
    if source_version == target_version:
        return root, []
    schema = _note_schema(target_version)
    warnings = []
    if target_version == '01.03':
        warnings.append('/ExplanatoryNote: обратное преобразование выполнено для формата приложения 01.03. Официальная XSD 01.03 не проверена.')
    converted = _project(root, root.tag, schema.root, schema, source_version, target_version, '/ExplanatoryNote', warnings)
    converted.set('SchemaVersion', target_version)
    return converted, warnings
