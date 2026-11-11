import base64
import json
import subprocess
import sys
import unittest

from schema_engine.documents import SCHEMA_DIRECTORY


class DesktopWorkerTests(unittest.TestCase):
    def test_process_handles_requests_without_http_handshake(self):
        requests = [
            {'operation': 'schema', 'family': 'explanatorynote', 'version': '01.07'},
            {'operation': 'new', 'family': 'conclusion'},
            {'operation': 'validate', 'xml': '<Conclusion SchemaVersion="01.04"/>'},
            {'operation': 'asset', 'name': 'conclusion-01-04.xsd'},
            {'operation': 'asset', 'name': '../conclusion-01-04.xsd'},
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
