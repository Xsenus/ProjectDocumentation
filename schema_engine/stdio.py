import json
import sys
from base64 import b64encode

from .documents import DEFAULT_FAMILY, SCHEMA_ASSETS, SCHEMA_DIRECTORY, resolve_version
from .migration import migrate_xml, normalize_legacy_conclusion
from .preview import render_preview
from .schema import form_schema, new_document, validate_document
from .xmlio import MAX_REQUEST_SIZE


def dispatch(request):
    if not isinstance(request, dict):
        raise ValueError('Ожидается JSON-объект.')
    operation = request.get('operation')
    if operation in ('schema', 'new'):
        family = request.get('family') or DEFAULT_FAMILY
        version = resolve_version(family, request.get('version'))
        if operation == 'schema':
            return form_schema(family, version)
        return {'xml': new_document(family, version), 'family': family, 'version': version}
    if operation == 'asset':
        name = request.get('name')
        if not isinstance(name, str) or name not in SCHEMA_ASSETS:
            raise ValueError('Файл схемы не найден.')
        return {'base64': b64encode((SCHEMA_DIRECTORY / name).read_bytes()).decode('ascii')}
    if operation in ('validate', 'migrate', 'preview', 'normalize-legacy-conclusion'):
        xml = request.get('xml')
        if not isinstance(xml, str):
            raise ValueError('Передайте XML строкой в свойстве xml.')
        if operation == 'validate':
            return validate_document(xml)
        if operation == 'migrate':
            return migrate_xml(xml, request.get('targetVersion'))
        if operation == 'normalize-legacy-conclusion':
            return {'xml': normalize_legacy_conclusion(xml)}
        return {'html': render_preview(xml, rich_edit=request.get('layout') == 'rich-edit')}
    raise ValueError('Неизвестная операция проверки XML.')


def run():
    source = sys.stdin.buffer
    target = sys.stdout.buffer
    while line := source.readline(MAX_REQUEST_SIZE + 1):
        if len(line) > MAX_REQUEST_SIZE:
            while line and not line.endswith(b'\n'):
                line = source.readline(65536)
            response = {'error': 'Размер запроса превышает допустимый.'}
        else:
            try:
                response = {'result': dispatch(json.loads(line.decode('utf-8')))}
            except ValueError as error:
                response = {'error': str(error)}
        try:
            target.write(json.dumps(response, ensure_ascii=False).encode('utf-8') + b'\n')
            target.flush()
        except BrokenPipeError:
            return
