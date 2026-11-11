import base64
import json
from pathlib import Path
import socket
import subprocess
import sys
from tempfile import TemporaryDirectory
import unittest
from urllib.request import urlopen
from xml.etree import ElementTree as ET

from schema_engine.documents import SCHEMA_DIRECTORY


class DesktopWorkerTests(unittest.TestCase):
    def test_process_handles_requests_without_http_handshake(self):
        requests = [
            {'operation': 'schema', 'family': 'explanatorynote', 'version': '01.07'},
            {'operation': 'new', 'family': 'conclusion'},
            {'operation': 'validate', 'xml': '<Conclusion SchemaVersion="01.04"/>'},
            {'operation': 'asset', 'name': 'conclusion-01-04.xsd'},
            {'operation': 'asset', 'name': '../conclusion-01-04.xsd'},
            {'operation': 'normalize-legacy-conclusion',
             'xml': '<Conclusion SchemaVersion="01.03"><Object/><ExpertOrganization/></Conclusion>'},
        ]
        lines = ''.join(json.dumps(request, ensure_ascii=False) + '\n' for request in requests)
        result = subprocess.run([sys.executable, '-m', 'schema_engine', '--stdio'], input=lines,
                                capture_output=True, encoding='utf-8', timeout=30, check=True)
        replies = [json.loads(line) for line in result.stdout.splitlines()]
        self.assertEqual(len(replies), len(requests))
        self.assertEqual(replies[0]['result']['root']['name'], 'ExplanatoryNote')
        self.assertIn('SchemaVersion="01.04"', replies[1]['result']['xml'])
        self.assertFalse(replies[2]['result']['valid'])
        self.assertEqual(base64.b64decode(replies[3]['result']['base64']),
                         (SCHEMA_DIRECTORY / 'conclusion-01-04.xsd').read_bytes())
        self.assertIn('error', replies[4])
        normalized = ET.fromstring(replies[5]['result']['xml'])
        self.assertEqual([element.tag for element in normalized], ['ExpertOrganization', 'Object'])

    def test_browser_and_xml_share_the_same_desktop_worker(self):
        with TemporaryDirectory() as directory:
            Path(directory, 'index.html').write_text('Portal fixture', encoding='utf-8')
            with self.start_worker(directory, 0) as worker:
                try:
                    portal = self.request(worker, 'start-portal')['result']
                    self.assertEqual(self.request(worker, 'start-portal')['result'], portal)
                    with urlopen(portal['url'], timeout=5) as response:
                        self.assertEqual(response.read(), b'Portal fixture')
                    self.assertIn('SchemaVersion="01.04"', self.request(worker, 'new')['result']['xml'])
                finally:
                    worker.stdin.close()
                    try:
                        worker.wait(timeout=10)
                    except subprocess.TimeoutExpired:
                        worker.kill()
                        raise
                self.assertEqual(worker.returncode, 0)
                with self.assertRaises(OSError):
                    urlopen(portal['url'], timeout=2)

    def test_missing_or_occupied_portal_does_not_stop_desktop_worker(self):
        with TemporaryDirectory() as directory:
            with self.start_worker(directory, 0) as worker:
                self.assertIn('нет веб-редактора', self.request(worker, 'start-portal')['error'])
            Path(directory, 'index.html').touch()
            with socket.socket() as occupied:
                occupied.bind(('127.0.0.1', 0))
                occupied.listen()
                port = occupied.getsockname()[1]
                with self.start_worker(directory, port) as worker:
                    self.assertIn(f'порту {port}', self.request(worker, 'start-portal')['error'])
                    self.assertIn('SchemaVersion="01.04"', self.request(worker, 'new')['result']['xml'])

    @staticmethod
    def start_worker(directory, port):
        source = ('from pathlib import Path; import sys; import schema_engine.__main__ as entry; '
                  'entry.PORTAL = Path(sys.argv[1]); entry.run_desktop_worker("127.0.0.1", int(sys.argv[2]))')
        return subprocess.Popen([sys.executable, '-c', source, directory, str(port)],
                                stdin=subprocess.PIPE, stdout=subprocess.PIPE, encoding='utf-8')

    @staticmethod
    def request(worker, operation):
        worker.stdin.write(json.dumps({'operation': operation}) + '\n')
        worker.stdin.flush()
        return json.loads(worker.stdout.readline())
