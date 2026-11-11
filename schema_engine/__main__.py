from argparse import ArgumentParser
from http import HTTPStatus
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
import json
import mimetypes
from pathlib import Path
import socket
from threading import Thread
from urllib.parse import parse_qs, unquote, urlsplit
import webbrowser

from schema_engine.documents import DEFAULT_FAMILY, DOCUMENTS, SCHEMA_ASSETS, SCHEMA_DIRECTORY, resolve_version
from schema_engine.schema import form_schema, new_document, validate_document
from schema_engine.xmlio import MAX_REQUEST_SIZE


PORTAL = SCHEMA_DIRECTORY.parent.parent / 'portal' / 'dist'
PORTAL_PORT = 8765
DEVELOPMENT_ORIGINS = {'http://127.0.0.1:5173', 'http://localhost:5173'}


class Handler(BaseHTTPRequestHandler):
    def setup(self):
        self.request.settimeout(30)
        super().setup()

    def log_message(self, format, *args):
        # Documents and paths are handled locally. Keep the launcher handshake on stdout.
        pass

    def _send(self, status, body, content_type, headers=None):
        self.send_response(status)
        self.send_header('Content-Type', content_type)
        self.send_header('Content-Length', str(len(body)))
        self.send_header('Cache-Control', 'no-store')
        self.send_header('X-Content-Type-Options', 'nosniff')
        for key, value in (headers or {}).items():
            self.send_header(key, value)
        self.end_headers()
        try:
            self.wfile.write(body)
        except (BrokenPipeError, ConnectionResetError):
            pass

    def _json(self, result, status=HTTPStatus.OK):
        self._send(status, json.dumps(result, ensure_ascii=False).encode('utf-8'), 'application/json; charset=utf-8')

    def _trusted_request(self):
        port = self.server.server_port
        hosts = {f'127.0.0.1:{port}', f'localhost:{port}'}
        origins = {f'http://{host}' for host in hosts} | DEVELOPMENT_ORIGINS
        origin = self.headers.get('Origin')
        if self.headers.get('Host') in hosts and (origin is None or origin in origins):
            return True
        self._json({'error': 'Разрешены только запросы из локального приложения.'}, HTTPStatus.FORBIDDEN)
        return False

    def _file(self, path, *, attachment=False):
        headers = {'Content-Disposition': f'attachment; filename="{path.name}"'} if attachment else {}
        mime_type = mimetypes.guess_type(path.name)[0] or 'application/octet-stream'
        if attachment and path.suffix in {'.xsd', '.xsl'}:
            mime_type = 'application/xml'
        self._send(HTTPStatus.OK, path.read_bytes(), mime_type, headers)

    def do_GET(self):
        if not self._trusted_request():
            return
        request = urlsplit(self.path)
        path = request.path
        if path in {'/api/schema', '/api/new'}:
            try:
                parameters = parse_qs(request.query, keep_blank_values=True, max_num_fields=16)
                family = parameters.get('family', [DEFAULT_FAMILY])[0]
                version = resolve_version(family, parameters.get('version', [None])[0])
                result = form_schema(family, version) if path == '/api/schema' else {
                    'xml': new_document(family, version), 'family': family, 'version': version,
                }
            except ValueError as error:
                self._json({'error': str(error)}, HTTPStatus.BAD_REQUEST)
            else:
                self._json(result)
        elif path.startswith('/schemas/') and (asset := path.removeprefix('/schemas/')) in SCHEMA_ASSETS:
            self._file(SCHEMA_DIRECTORY / asset, attachment=True)
        elif path.startswith(('/api/', '/schemas/')):
            self._json({'error': 'Ресурс не найден.'}, HTTPStatus.NOT_FOUND)
        else:
            portal = self.server.portal
            try:
                candidate = (portal / (unquote(path).lstrip('/') or 'index.html')).resolve()
            except (OSError, ValueError):
                self._json({'error': 'Ресурс не найден.'}, HTTPStatus.NOT_FOUND)
                return
            if candidate.is_relative_to(portal) and candidate.is_file():
                self._file(candidate)
            elif path == '/' and not (portal / 'index.html').exists():
                self._json({'error': 'Соберите React-портал командой npm run build в каталоге portal.'}, HTTPStatus.SERVICE_UNAVAILABLE)
            else:
                self._json({'error': 'Ресурс не найден.'}, HTTPStatus.NOT_FOUND)

    def _body(self):
        if self.headers.get_content_type() != 'application/json':
            raise ValueError('Ожидается запрос application/json.')
        size = int(self.headers.get('Content-Length', '0'))
        if not 0 < size <= MAX_REQUEST_SIZE:
            raise ValueError('Размер запроса превышает допустимый или запрос пуст.')
        body = json.loads(self.rfile.read(size).decode('utf-8'))
        if not isinstance(body, dict) or not isinstance(body.get('xml'), str):
            raise ValueError('Передайте XML строкой в свойстве xml.')
        return body

    def do_POST(self):
        if not self._trusted_request():
            return
        path = urlsplit(self.path).path
        if path not in {'/api/validate', '/api/migrate', '/api/preview'}:
            self._json({'error': 'Ресурс не найден.'}, HTTPStatus.NOT_FOUND)
            return
        try:
            body = self._body()
        except (ValueError, UnicodeError, TimeoutError):
            self._json({'error': 'Передайте XML строкой в корректном JSON-запросе.'}, HTTPStatus.BAD_REQUEST)
            return

        if path == '/api/validate':
            self._json(validate_document(body['xml']))
            return

        try:
            if path == '/api/preview':
                from schema_engine.preview import render_preview
                result = {'html': render_preview(body['xml'])}
            else:
                from schema_engine.migration import migrate_xml
                result = migrate_xml(body['xml'], body.get('targetVersion'))
        except ValueError as error:
            self._json({'error': str(error)}, HTTPStatus.BAD_REQUEST)
        else:
            self._json(result)


class PortalServer(ThreadingHTTPServer):
    allow_reuse_address = not hasattr(socket, 'SO_EXCLUSIVEADDRUSE')

    def server_bind(self):
        # SO_REUSEADDR can bind to a running listener on Windows.
        if not self.allow_reuse_address:
            self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_EXCLUSIVEADDRUSE, 1)
        super().server_bind()


def create_portal_server(host, port):
    server = PortalServer((host, port), Handler)
    server.portal = PORTAL.resolve()
    return server


def run_desktop_worker(host, port):
    from schema_engine.stdio import run

    server = None

    def start_portal():
        nonlocal server
        if server is None:
            if not (PORTAL / 'index.html').is_file():
                raise ValueError('В этом комплекте нет веб-редактора.')
            try:
                server = create_portal_server(host, port)
            except OSError as error:
                raise ValueError(f'Не удалось открыть веб-редактор на порту {port}. '
                                 'Если он уже открыт из другой установки, закройте её и повторите.') from error
            Thread(target=server.serve_forever, daemon=True).start()
        return {'url': f'http://{host}:{server.server_port}'}

    try:
        run(start_portal)
    finally:
        if server is not None:
            server.shutdown()
            server.server_close()


def main():
    parser = ArgumentParser(description='Локальный сервис проектной документации и React-портала.')
    parser.add_argument('--host', choices=['127.0.0.1'], default='127.0.0.1')
    parser.add_argument('--port', type=int, default=PORTAL_PORT)
    parser.add_argument('--open-browser', action='store_true')
    parser.add_argument('--stdio', action='store_true', help='Обрабатывать JSON-запросы настольного приложения через стандартный ввод и вывод.')
    options = parser.parse_args()
    if options.stdio:
        run_desktop_worker(options.host, options.port)
        return
    for family in DOCUMENTS:
        form_schema(family)
    with create_portal_server(options.host, options.port) as server:
        url = f'http://{options.host}:{server.server_port}'
        print(json.dumps({'url': url}), flush=True)
        if options.open_browser:
            Thread(target=webbrowser.open, args=(url,), daemon=True).start()
        try:
            server.serve_forever()
        except KeyboardInterrupt:
            pass


if __name__ == '__main__':
    main()
