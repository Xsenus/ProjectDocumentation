from argparse import ArgumentParser
from http import HTTPStatus
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
import json
import mimetypes
from pathlib import Path
from threading import Thread
from urllib.parse import unquote, urlsplit
import webbrowser

from schema_engine.schema import CONCLUSION_SCHEMAS, SCHEMA_DIRECTORY, form_schema, new_conclusion, validate_conclusion
from schema_engine.xmlio import MAX_XML_SIZE


PORTAL = SCHEMA_DIRECTORY.parent.parent / 'portal' / 'dist'
MAX_REQUEST_SIZE = MAX_XML_SIZE * 2 + 4096
DEVELOPMENT_ORIGINS = {'http://127.0.0.1:5173', 'http://localhost:5173'}
SCHEMA_ASSETS = {*CONCLUSION_SCHEMAS.values(), *(name.replace('.xsd', '.xsl') for name in CONCLUSION_SCHEMAS.values())}


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
        path = urlsplit(self.path).path
        if path == '/api/schema':
            self._json(form_schema())
        elif path == '/api/new':
            self._json({'xml': new_conclusion()})
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
            self._json(validate_conclusion(body['xml']))
            return

        try:
            if path == '/api/preview':
                from schema_engine.preview import render_preview
                result = {'html': render_preview(body['xml'])}
            else:
                from schema_engine.migration import migrate_xml
                result = migrate_xml(body['xml'], body.get('targetVersion', '01.04'))
        except ValueError as error:
            self._json({'error': str(error)}, HTTPStatus.BAD_REQUEST)
        else:
            self._json(result)


def main():
    parser = ArgumentParser(description='Локальный сервис XML-заключений и React-портала.')
    parser.add_argument('--host', choices=['127.0.0.1'], default='127.0.0.1')
    parser.add_argument('--port', type=int, default=8765)
    parser.add_argument('--open-browser', action='store_true')
    options = parser.parse_args()
    form_schema()
    with ThreadingHTTPServer((options.host, options.port), Handler) as server:
        server.portal = PORTAL.resolve()
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
