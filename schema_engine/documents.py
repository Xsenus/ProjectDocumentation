from dataclasses import dataclass
from pathlib import Path


SCHEMA_DIRECTORY = Path(__file__).with_name('schemas')
DEFAULT_FAMILY = 'conclusion'
LEGACY_NOTE_VERSION = '01.03'
LEGACY_CONCLUSION_VERSIONS = ('01.00', '01.02')
SCHEMA_LOCATION = '{http://www.w3.org/2001/XMLSchema-instance}noNamespaceSchemaLocation'


@dataclass(frozen=True)
class DocumentFamily:
    root: str
    id_attribute: str
    current_version: str
    versions: tuple[str, ...]


DOCUMENTS = {
    'conclusion': DocumentFamily('Conclusion', 'ConclusionGUID', '01.04', ('01.03', '01.04')),
    'explanatorynote': DocumentFamily('ExplanatoryNote', 'id', '01.07', ('01.03', '01.05', '01.07')),
}
FAMILIES_BY_ROOT = {document.root: family for family, document in DOCUMENTS.items()}


def source_versions(family):
    versions = DOCUMENTS[family].versions
    return LEGACY_CONCLUSION_VERSIONS + versions if family == 'conclusion' else versions


def resolve_version(family, version=None):
    if not isinstance(family, str) or family not in DOCUMENTS:
        raise ValueError('Выберите заключение экспертизы или пояснительную записку.')
    document = DOCUMENTS[family]
    if version is None:
        return document.current_version
    if version not in document.versions:
        raise ValueError(f'Доступные версии для {document.root}: {", ".join(document.versions)}.')
    return version


def legacy_note_format(family, version):
    return family == 'explanatorynote' and version == LEGACY_NOTE_VERSION


def asset_stem(family, version):
    return f'{family}-{version.replace(".", "-")}'


def schema_filename(family, version):
    if legacy_note_format(family, version):
        return 'explanatorynote-wire-01-03.xsd'
    return asset_stem(family, version) + '.xsd'


def document_version(root, family):
    version = root.get('SchemaVersion')
    if family == 'explanatorynote' and not (version or '').strip():
        hints = ((root.get(name) or '').strip() for name in ('SchemaLink', SCHEMA_LOCATION))
        filenames = {hint.split('?', 1)[0].split('#', 1)[0].replace('\\', '/').rsplit('/', 1)[-1].lower()
                     for hint in hints if hint}
        if filenames <= {'explanatorynote-01-03.xsd', schema_filename(family, LEGACY_NOTE_VERSION)}:
            # The old WinForms serializer uses 01.03 when the attribute is absent.
            return LEGACY_NOTE_VERSION
    return version


SCHEMA_ASSETS = frozenset(
    asset_stem(family, version) + extension
    for family, document in DOCUMENTS.items()
    for version in document.versions
    if not legacy_note_format(family, version)
    for extension in ('.xsd', '.xsl')
) | frozenset(schema_filename('conclusion', version) for version in LEGACY_CONCLUSION_VERSIONS) | {
    schema_filename('explanatorynote', LEGACY_NOTE_VERSION),
}
