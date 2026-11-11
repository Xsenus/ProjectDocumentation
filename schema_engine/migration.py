"""Loss-aware conversion between supported document versions."""

from collections import Counter
from dataclasses import dataclass
from datetime import date
from functools import lru_cache
import re
from xml.etree import ElementTree as ET

from .documents import (
    DOCUMENTS, FAMILIES_BY_ROOT, LEGACY_CONCLUSION_VERSIONS, SCHEMA_DIRECTORY, SCHEMA_LOCATION,
    document_version, resolve_version, schema_filename, source_versions,
)
from .xmlio import parse_document


XSD = '{http://www.w3.org/2001/XMLSchema}'
PARTICLES = {XSD + name for name in ('all', 'choice', 'element', 'group', 'sequence')}
PERSON_AND_ORGANIZATION_NAMES = {
    'FamilyName': 'Surname', 'FirstName': 'Name', 'SecondName': 'Patronymic',
    'OrgFullName': 'FullName', 'OrgOGRN': 'OGRN', 'OrgINN': 'INN', 'OrgKPP': 'KPP',
}
LEGACY_NAMES = {target: source for source, target in PERSON_AND_ORGANIZATION_NAMES.items()}
ADDRESS_VARIANTS = {'RussianAddress', 'RussianPostAddress', 'ForeignAddress', 'SeaRussianAddress'}
RUSSIAN_NAMES = {'россия', 'российская федерация', 'рф', 'russia', 'russian federation', 'ru', 'rus'}
ADDRESS_LABELS = {
    'District': 'Район', 'City': 'Город', 'Settlement': 'Населённый пункт',
    'Street': 'Улица', 'Building': 'Здание', 'Room': 'Помещение',
    'PostIndex': 'Почтовый индекс', 'Landmark': 'Ориентир',
}
DATE_VALUE = r'\d{4}-\d{2}-\d{2}(?:Z|[+-]\d{2}:\d{2})?|\d{2}\.\d{2}\.\d{4}'
DATE_TEXT = re.compile(rf'\s*({DATE_VALUE})(?:\s*[,;:–—]\s*(.+))?\s*', re.DOTALL)
DATE_IN_SENTENCE = re.compile(rf'\b(?:по\s+состоянию\s+на|на\s+дату)\s+({DATE_VALUE})(?!\w)', re.IGNORECASE)
REQUIREMENTS_DATE_FIELDS = {'EngineeringSurveySummaryDate', 'ProjectDocumentsSummaryDate'}
LEGACY_DATE_PLACEHOLDERS = {'', '-', '–', '—'}
FUNCTIONS_CODE = r'[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,3}'
LEGACY_SUMMARY_SEPARATOR = re.compile(r'(?<=[.!?])\s+|[\r\n;]+')
ENGINEERING_SURVEYS_CONFORM = 'Результаты инженерных изысканий соответствуют требованиям технических регламентов.'


@dataclass(frozen=True)
class _Particle:
    kind: str
    minimum: int = 1
    maximum: int | None = 1
    element: ET.Element | None = None
    children: tuple['_Particle', ...] = ()

    @property
    def name(self) -> str:
        return self.element.get('name') if self.element is not None else ''


class _Schema:
    def __init__(self, version: str):
        schema = ET.parse(SCHEMA_DIRECTORY / schema_filename('conclusion', version)).getroot()
        self.root = schema.find(f'{XSD}element[@name="Conclusion"]')
        self.complex_types = self._named(schema, 'complexType')
        self.simple_types = self._named(schema, 'simpleType')
        self.groups = self._named(schema, 'group')

    @staticmethod
    def _named(schema, kind):
        return {item.attrib['name']: item for item in schema.findall(XSD + kind)}

    def _content(self, definition):
        inline = definition.find(XSD + 'complexType')
        if inline is not None:
            return inline
        return self.complex_types.get(definition.get('type'))

    def _inherited_content(self, complex_type):
        if complex_type is None:
            return
        extension = complex_type.find(f'{XSD}complexContent/{XSD}extension')
        if extension is None:
            extension = complex_type.find(f'{XSD}simpleContent/{XSD}extension')
        if extension is None:
            yield complex_type
            return
        yield from self._inherited_content(self.complex_types.get(extension.get('base')))
        yield extension

    def _particle(self, node):
        kind = node.tag.removeprefix(XSD)
        minimum = int(node.get('minOccurs', '1'))
        raw_maximum = node.get('maxOccurs', '1')
        maximum = None if raw_maximum == 'unbounded' else int(raw_maximum)
        if kind == 'element':
            return _Particle(kind, minimum, maximum, element=node)
        if kind == 'group':
            node = self.groups[node.attrib['ref']]
        children = tuple(self._particle(child) for child in node if child.tag in PARTICLES)
        return _Particle(kind, minimum, maximum, children=children)

    @lru_cache(maxsize=None)
    def layout(self, definition):
        content = tuple(self._inherited_content(self._content(definition)))
        fields = tuple(self._particle(child) for base in content for child in base if child.tag in PARTICLES)
        attributes = {node.attrib['name']: node for base in content for node in base.findall(XSD + 'attribute')}
        return _Particle('sequence', children=fields), attributes

    @lru_cache(maxsize=None)
    def enumerated_values(self, type_name):
        simple_type = self.simple_types.get(type_name)
        if simple_type is None:
            return frozenset()
        restriction = simple_type.find(XSD + 'restriction')
        if restriction is None:
            return frozenset()
        values = frozenset(node.attrib['value'] for node in restriction.findall(XSD + 'enumeration'))
        return values or self.enumerated_values(restriction.get('base'))


@lru_cache(maxsize=2)
def _schema(version):
    return _Schema(version)


def _ordered_fields(particle, order=()):
    if particle.element is not None:
        yield particle.name, (order, particle.element)
        return
    for index, child in enumerate(particle.children):
        # Repeated choices must retain the source order between alternatives.
        yield from _ordered_fields(child, order + (0 if particle.kind == 'choice' else index,))


def _field_names(particle):
    return [name for name, _ in _ordered_fields(particle)]


def _can_be_empty(particle):
    if particle.minimum == 0:
        return True
    if particle.element is not None:
        return False
    checks = (_can_be_empty(child) for child in particle.children)
    return any(checks) if particle.kind == 'choice' else all(checks)


def _check_required(particle, counts, path, warnings, parent_maximum=1):
    maximum = None if parent_maximum is None or particle.maximum is None else parent_maximum * particle.maximum
    if particle.element is not None:
        count = counts[particle.name]
        if count < particle.minimum:
            warnings.append(f'{path}/{particle.name}: заполните обязательное поле.')
        if maximum is not None and count > maximum:
            warnings.append(f'{path}/{particle.name}: превышено допустимое число элементов.')
        return

    present = [child for child in particle.children if any(counts[name] for name in _field_names(child))]
    if not present and _can_be_empty(particle):
        return
    if particle.kind == 'choice':
        if not present:
            choices = ', '.join('/'.join(_field_names(child)) for child in particle.children)
            warnings.append(f'{path}: выберите один из обязательных вариантов ({choices}).')
        elif len(present) > 1 and maximum == 1:
            warnings.append(f'{path}: перенесено несколько взаимоисключающих вариантов. Оставьте один.')
        children = present
    else:
        children = particle.children
    for child in children:
        _check_required(child, counts, path, warnings, maximum)


def _with_children(element, children):
    result = ET.Element(element.tag, element.attrib)
    result.extend(children)
    return result


def _value_element(tag, value):
    element = ET.Element(tag)
    element.text = value
    return element


def _renamed(element, name):
    result = _with_children(element, list(element))
    result.tag = name
    result.text = element.text
    return result


def _address_description(children):
    lines = []
    for child in children:
        value = (child.text or '').strip()
        if value:
            label = ADDRESS_LABELS.get(child.tag)
            lines.append(f'{label}: {value}' if label else value)
    return ', '.join(lines)


def _upgrade_address(element, target_type, path, warnings):
    if any(child.tag in ADDRESS_VARIANTS for child in element):
        return element
    regions = {(child.text or '').strip() for child in element.findall('Region')}
    countries = {(child.text or '').strip() for child in element.findall('Country')}
    if len(regions) > 1 or len(countries) > 1:
        warnings.append(f'{path}: в оригинале несколько кодов региона или стран. Определите адрес по оригиналу.')
        return _with_children(element, [])
    region = next(iter(regions), '')
    country = next(iter(countries), '')
    russian_country = country.casefold() in RUSSIAN_NAMES
    foreign_country = bool(country) and not russian_country
    russian_region = region != '00' and region in _schema('01.03').enumerated_values('tRegionsRF')
    if region == '00' and russian_country and target_type == 'tObjectAddress':
        details = [child for child in element if child.tag in ADDRESS_LABELS or child.tag == 'Note']
        for child in element:
            if child not in details and child.tag not in {'Country', 'Region'}:
                warnings.append(f'{path}/{child.tag}: поле не имеет аналога в морском адресе и доступно в оригинале.')
        warnings.append(f'{path}/SeaRussianAddress: код 00 и страна Россия перенесены как морской адрес. Проверьте описание объекта.')
        return _with_children(element, [_value_element('SeaRussianAddress', _address_description(details))])
    if (region == '00' and russian_country) or (russian_region and foreign_country):
        warnings.append(f'{path}: код региона и страна указывают на разные варианты адреса. Выберите правильный вариант по исходному документу.')
        return _with_children(element, [])
    if not russian_region and region != '00' and not country:
        warnings.append(f'{path}: невозможно определить страну адреса по исходному XML. Выберите вариант и перенесите адрес из оригинала.')
        return _with_children(element, [])
    if region == '00' or foreign_country:
        variant = ET.Element('ForeignAddress')
        variant.extend(element.findall('Country'))
        if region == '00' and not country and target_type == 'tObjectAddress':
            warnings.append(f'{path}: код 00 допускает зарубежный или морской адрес. Описание перенесено в незаполненный вариант ForeignAddress. Уточните тип адреса.')
        if region not in {'', '00'}:
            warnings.append(f'{path}/Region: исходный код региона не перенесён в иностранный адрес.')
        fields = [child for child in element if child.tag in ADDRESS_LABELS or child.tag == 'Note']
        if any(field.tag != 'Note' for field in fields):
            warnings.append(f'{path}/ForeignAddress: структурные поля сведены в текст адреса.')
            description = _address_description(fields)
            if description:
                variant.append(_value_element('Note', description))
        else:
            variant.extend(fields)
        for child in element:
            if child.tag not in ADDRESS_LABELS and child.tag not in {'Country', 'Region', 'Note'}:
                variant.append(child)
        return _with_children(element, [variant])

    variant = ET.Element('RussianPostAddress' if target_type == 'tPostAddress' else 'RussianAddress')
    fields = [child for child in element if child.tag in ADDRESS_LABELS and child.tag != 'PostIndex']
    notes = element.findall('Note')
    combine = bool(fields and any((note.text or '').strip() for note in notes))
    for child in element:
        if child.tag == 'Region':
            variant.append(_renamed(child, 'RegionCode'))
        elif child.tag != 'Country' and (not combine or child not in fields + notes):
            variant.append(child)
    if combine:
        variant.append(_value_element('Note', _address_description(fields + notes)))
        warnings.append(f'{path}/{variant.tag}: структурные поля и примечание сведены в текст адреса, чтобы сохранить сведения в одном варианте 01.04.')
    return _with_children(element, [variant])


def _downgrade_address(element, target_type, path, warnings):
    variants = [child for child in element if child.tag in ADDRESS_VARIANTS]
    if not variants:
        return element
    variant = variants[0]
    if len(variants) > 1:
        warnings.append(f'{path}: в 01.03 перенесён первый вариант адреса. Остальные варианты доступны в оригинале.')
    for child in element:
        if child not in variants:
            warnings.append(f'{path}/{child.tag}: поле за пределами выбранного варианта адреса не перенесено.')
    for attribute in variant.attrib:
        warnings.append(f'{path}/{variant.tag}/@{attribute}: атрибут не перенесён в плоский адрес 01.03.')
    if variant.tag == 'SeaRussianAddress':
        warnings.append(f'{path}: морской адрес перенесён в примечание с кодом 00. В 01.03 этот код также применяется для зарубежных адресов.')
        return _with_children(element, [_value_element('Region', '00'), _value_element('Note', variant.text or '')])
    if variant.tag == 'ForeignAddress':
        if (variant.findtext('Country') or '').strip().casefold() in RUSSIAN_NAMES:
            warnings.append(f'{path}/Country: иностранный вариант содержит название России. Проверьте классификацию адреса.')
        return _with_children(element, [*variant, _value_element('Region', '00')])
    children = []
    note_tags = {'Note', 'Landmark'}
    if target_type != 'tPostAddress':
        note_tags.add('PostIndex')
    notes = [child for child in variant if child.tag in note_tags]
    for child in variant:
        if child.tag == 'RegionCode':
            if (child.text or '').strip() == '00':
                warnings.append(f'{path}/RegionCode: код 00 в 01.03 относится к зарубежным и морским адресам. Укажите регион российского адреса по оригиналу.')
            else:
                children.append(_renamed(child, 'Region'))
        elif child.tag not in note_tags:
            children.append(child)
    folded_tags = dict.fromkeys(note.tag for note in notes if note.tag != 'Note')
    if folded_tags:
        children.append(_value_element('Note', _address_description(notes)))
        for tag in folded_tags:
            warnings.append(f'{path}/{tag}: поле «{ADDRESS_LABELS[tag]}» перенесено в примечание 01.03.')
    else:
        children.extend(notes)
    return _with_children(element, children)


def _normalized_date(raw_date):
    if '.' in raw_date:
        raw_date = '-'.join(reversed(raw_date.split('.')))
    try:
        parsed = date.fromisoformat(raw_date[:10])
    except ValueError:
        return None
    zone = raw_date[10:]
    if zone and zone != 'Z':
        hour, minute = map(int, zone[1:].split(':'))
        if hour > 14 or minute > 59 or (hour == 14 and minute != 0):
            return None
    return parsed.isoformat() + zone


def _upgrade_date(element, path, warnings):
    if len(element):
        return element
    value = element.text or ''
    match = DATE_TEXT.fullmatch(value)
    if match is not None:
        raw_date, justification = match.groups()
        parsed = _normalized_date(raw_date)
        if parsed is not None:
            children = [_value_element('Date', parsed)]
            if justification:
                children.append(_value_element('DateJustification', justification))
            return _with_children(element, children)
    dates = DATE_IN_SENTENCE.findall(value)
    if len(dates) == 1:
        parsed = _normalized_date(dates[0])
        if parsed is not None:
            warnings.append(f'{path}: дата перенесена из фразы исходного документа. Проверьте дату и обоснование.')
            return _with_children(element, [_value_element('Date', parsed), _value_element('DateJustification', value)])
    if value.strip():
        warnings.append(f'{path}: дата не распознана. Исходный текст сохранён в обосновании.')
        return _with_children(element, [_value_element('DateJustification', value)])
    return _with_children(element, [])


def _downgrade_date(element, path, warnings):
    if not len(element):
        return element
    pieces = []
    for child in element:
        if child.tag in {'Date', 'DateJustification'} and (child.text or '').strip():
            pieces.append(child.text)
        elif child.tag not in {'Date', 'DateJustification'}:
            warnings.append(f'{path}/{child.tag}: поле даты не имеет аналога в 01.03.')
    result = ET.Element(element.tag, element.attrib)
    result.text = '; '.join(pieces)
    return result


def _finance_owner(element, forward, path, warnings):
    wrappers = [child for child in element if child.tag in {'Organization', 'ForeignOrganization'}]
    if forward and wrappers:
        if len(wrappers) > 1:
            warnings.append(f'{path}: в 01.04 перенесён первый владелец. Остальные сведения доступны в оригинале.')
        owner = wrappers[0]
        if owner.tag == 'ForeignOrganization' and owner.find('RAFP') is None:
            warnings.append(f'{path}/RAFP: внесите номер записи об аккредитации иностранной организации.')
        result = _with_children(element, [*owner, *(child for child in element if child not in wrappers)])
        for attribute, value in owner.attrib.items():
            if attribute in result.attrib and result.get(attribute) != value:
                warnings.append(f'{path}/@{attribute}: исходные атрибуты владельца конфликтуют.')
            else:
                result.set(attribute, value)
        return result
    if not forward and not wrappers and len(element):
        tag = 'ForeignOrganization' if element.find('RAFP') is not None and element.find('OGRN') is None else 'Organization'
        owner = ET.Element(tag)
        owner.extend(element)
        return _with_children(element, [owner])
    return element


def _clean_legacy_address(element):
    fields = ADDRESS_LABELS.keys() | {'Country', 'Note', 'Region'}
    return _with_children(element, [
        child for child in element
        if child.tag not in fields or len(child) or child.attrib or (child.text or '').strip()
    ])


def _legacy_functions(element, type_name, path, warnings):
    descriptions = element.findall('Functions')
    if len(descriptions) != 1 or element.find('FunctionsClass') is not None:
        return element
    description = descriptions[0]
    if len(description) or description.attrib:
        return element
    value = (description.text or '').strip()
    suffix = r'\.[0-9]{1,3}'
    if type_name == 'tObject':
        suffix = f'(?:{suffix})?'
    if re.fullmatch(FUNCTIONS_CODE + suffix, value):
        warnings.append(f'{path}/FunctionsClass: в исходном поле назначения указан код. Код перенесён без изменения.')
        return _with_children(element, [
            _value_element('FunctionsClass', value) if child is description else child for child in element
        ])
    if value:
        warnings.append(f'{path}/Functions: текст назначения доступен в оригинале. Для 01.04 выберите код классификатора.')
        return _with_children(element, [child for child in element if child is not description])
    return element


def _statement_key(value):
    return ' '.join(value.casefold().replace('ё', 'е').split()).rstrip('.')


def _expand_primary_survey_summary(root, warnings):
    examination = root.find('ExaminationObject')
    if examination is None or (examination.findtext('ExaminationStage') or '').strip() != '1':
        return
    if not any((element.text or '').strip() == '1' for element in examination.findall('ExaminationType')):
        return
    summaries = root.findall('Summary/EngineeringSurveySummary')
    if len(summaries) != 1 or root.find('ExpertEngineeringSurveys/Mismatches') is not None:
        return
    summary = summaries[0]
    if not len(summary) and not summary.attrib and _statement_key(summary.text or '') in {'соответствует', 'соответствуют'}:
        summary.text = ENGINEERING_SURVEYS_CONFORM
        warnings.append('/Conclusion/Summary/EngineeringSurveySummary: краткий положительный вывод первичной '
                        'экспертизы перенесён в установленную формулировку 01.04.')


def _legacy_examination_summary(element, definition, schema, path, warnings):
    value = (element.text or '').strip()
    if len(element) or not value:
        return element
    particle, _ = schema.layout(definition)
    choices = {}
    for name, (_, field) in _ordered_fields(particle):
        for option in schema.enumerated_values(field.get('type')):
            choices.setdefault(_statement_key(option), []).append((name, option))
    key = _statement_key(value)
    statements = [value] if key in choices else LEGACY_SUMMARY_SEPARATOR.split(value)
    matches = {}
    unresolved = False
    for statement in statements:
        if not statement.strip():
            continue
        options = choices.get(_statement_key(statement), [])
        if len(options) == 1:
            name, option = options[0]
            matches.setdefault(name, set()).add(option)
        else:
            unresolved = True
    children = []
    for name, options in matches.items():
        if len(options) == 1:
            children.append(_value_element(name, next(iter(options))))
        else:
            unresolved = True
            warnings.append(f'{path}/{name}: в исходном выводе несколько вариантов. Выберите нужное значение по оригиналу.')
    if unresolved:
        warnings.append(f'{path}: часть исходного текста требует ручного выбора итоговых выводов 01.04. Полный текст доступен в оригинале.')
    return _with_children(element, children)


def _project(source, name, definition, schema, forward, path, warnings, historical=False):
    type_name = definition.get('type')
    if forward and type_name in {'tAddress', 'tObjectAddress', 'tPostAddress'}:
        if historical:
            source = _clean_legacy_address(source)
        source = _upgrade_address(source, type_name, path, warnings)
    elif not forward and type_name in {'tAddress', 'tPostAddress'}:
        source = _downgrade_address(source, type_name, path, warnings)
    if name in REQUIREMENTS_DATE_FIELDS:
        source = (_upgrade_date if forward else _downgrade_date)(source, path, warnings)
    if name == 'FinanceOwner':
        source = _finance_owner(source, forward, path, warnings)
    if historical and type_name in {'tObject', 'tObjectPart'}:
        source = _legacy_functions(source, type_name, path, warnings)
    if historical and type_name == 'tExaminationSummary':
        source = _legacy_examination_summary(source, definition, schema, path, warnings)

    particle, attributes = schema.layout(definition)
    fields = dict(_ordered_fields(particle))
    result = ET.Element(name)
    for attribute, value in source.attrib.items():
        if path == '/Conclusion' and attribute == 'SchemaLink':
            if value.strip():
                warnings.append(f'{path}/@SchemaLink: ссылка на исходную версию снята при преобразовании.')
        elif path == '/Conclusion' and attribute == SCHEMA_LOCATION:
            result.set(attribute, f'conclusion-{"01-04" if forward else "01-03"}.xsd')
        elif attribute in attributes:
            result.set(attribute, value)
        else:
            warnings.append(f'{path}/@{attribute}: атрибут не имеет аналога в целевой схеме и доступен в оригинале.')
    for attribute, attribute_definition in attributes.items():
        if attribute_definition.get('use') == 'required' and not (result.get(attribute) or '').strip():
            warnings.append(f'{path}/@{attribute}: заполните обязательное поле.')

    if not fields:
        result.text = source.text
        values = schema.enumerated_values(type_name)
        if values and result.text is not None and result.text not in values:
            matching_statements = []
            if historical and path.startswith('/Conclusion/Summary/'):
                key = _statement_key(result.text)
                matching_statements = [value for value in values if _statement_key(value) == key]
            if len(matching_statements) == 1:
                result.text = matching_statements[0]
            else:
                warnings.append(f'{path}: значение отсутствует в справочнике целевой схемы. Выберите допустимое значение.')
        if type_name in {'tNotEmptyString', 'tNotBreakString'} and not (result.text or '').strip():
            warnings.append(f'{path}: в целевой схеме это поле не может быть пустым.')
        if type_name == 'tNotBreakString' and result.text and (len(result.text) > 50 or any(ch in result.text for ch in '\r\n')):
            warnings.append(f'{path}: исходный текст сохранён целиком. Для 01.04 сократите его до 50 символов и уберите переносы строк.')
    elif (source.text or '').strip():
        warnings.append(f'{path}: свободный текст не соответствует структуре целевого поля и доступен в оригинале.')

    accepted = []
    aliases = PERSON_AND_ORGANIZATION_NAMES if forward else LEGACY_NAMES
    original_counts = Counter(child.tag for child in source)
    original_positions = Counter()
    for child in source:
        candidate = child.tag
        original_positions[candidate] += 1
        suffix = f'[{original_positions[candidate]}]' if original_counts[candidate] > 1 else ''
        if (historical and path == '/Conclusion/Summary' and candidate in REQUIREMENTS_DATE_FIELDS
                and not len(child) and not child.attrib and (child.text or '').strip() in LEGACY_DATE_PLACEHOLDERS):
            warnings.append(f'{path}/{candidate}{suffix}: в оригинале дата не указана. Внесите дату, если она обязательна для этого вида экспертизы.')
            continue
        if forward and candidate == 'ForeignOrganization' and 'Organization' in fields:
            candidate = 'Organization'
            if child.find('RAFP') is None:
                warnings.append(f'{path}/{child.tag}{suffix}/RAFP: внесите номер записи об аккредитации иностранной организации.')
        elif not forward and candidate == 'Organization' and 'ForeignOrganization' in fields and child.find('RAFP') is not None and child.find('OGRN') is None:
            candidate = 'ForeignOrganization'
        elif candidate not in fields:
            candidate = aliases.get(candidate, candidate)
        if candidate in fields:
            order, child_definition = fields[candidate]
            accepted.append((order, candidate, child_definition, child))
        else:
            warnings.append(f'{path}/{child.tag}{suffix}: поле не имеет аналога в целевой схеме и доступно в оригинале.')

    counts = Counter(candidate for _, candidate, _, _ in accepted)
    positions = Counter()
    for _, candidate, child_definition, child in sorted(accepted, key=lambda item: item[0]):
        positions[candidate] += 1
        suffix = f'[{positions[candidate]}]' if counts[candidate] > 1 else ''
        result.append(_project(child, candidate, child_definition, schema, forward, f'{path}/{candidate}{suffix}',
                               warnings, historical))
    _check_required(particle, counts, path, warnings)
    return result


def migrate_xml(xml: str, target_version: str | None = None) -> dict:
    """Return a converted draft and field warnings; callers retain the input XML."""
    try:
        root = parse_document(xml)
    except ET.ParseError as error:
        raise ValueError(f'Некорректный XML документа: {error}.') from error
    family = FAMILIES_BY_ROOT.get(root.tag)
    if family is None:
        raise ValueError('Ожидается XML с корневым элементом Conclusion или ExplanatoryNote.')
    document = DOCUMENTS[family]
    target_version = resolve_version(family, target_version)
    source_version = document_version(root, family)
    versions = source_versions(family)
    if source_version not in versions:
        if source_version and re.fullmatch(r'[0-9]{2}\.[0-9]{2}', source_version):
            raise ValueError(f'Версия {source_version} для {document.root} не поддерживается. '
                             f'Доступные версии: {", ".join(versions)}.')
        raise ValueError(f'У {document.root} отсутствует поддерживаемый атрибут SchemaVersion ({", ".join(versions)}).')
    historical = family == 'conclusion' and source_version in LEGACY_CONCLUSION_VERSIONS
    if historical and target_version != '01.04':
        raise ValueError(f'Заключение версии {source_version} можно преобразовать в 01.04.')
    warnings: list[str] = []
    if source_version != target_version:
        if family == 'explanatorynote':
            from .note_migration import migrate_note

            result, warnings = migrate_note(root, source_version, target_version)
        else:
            schema = _schema(target_version)
            if historical:
                _expand_primary_survey_summary(root, warnings)
            result = _project(root, 'Conclusion', schema.root, schema, target_version == '01.04', '/Conclusion',
                              warnings, historical)
        result.set('SchemaVersion', target_version)
        ET.indent(result, space='  ')
        xml = ET.tostring(result, encoding='unicode', xml_declaration=True)
    return {'xml': xml, 'family': family, 'warnings': warnings, 'sourceVersion': source_version, 'targetVersion': target_version}
