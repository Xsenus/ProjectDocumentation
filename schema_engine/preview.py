from functools import lru_cache
from xml.etree import ElementTree as ET

from lxml import etree

from .schema import CONCLUSION_SCHEMAS, SCHEMA_DIRECTORY
from .xmlio import parse_document


PREVIEW_POLICY = (
    '<meta http-equiv="Content-Security-Policy" content="default-src \'none\'; '
    'style-src \'unsafe-inline\'; img-src data:; form-action \'none\'; base-uri \'none\'">'
)


@lru_cache(maxsize=len(CONCLUSION_SCHEMAS))
def _stylesheet(version):
    source = SCHEMA_DIRECTORY / CONCLUSION_SCHEMAS[version].replace('.xsd', '.xsl')
    parser = etree.XMLParser(resolve_entities=False, no_network=True, load_dtd=False)
    return etree.XSLT(etree.parse(str(source), parser), access_control=etree.XSLTAccessControl.DENY_ALL)


def render_preview(xml):
    try:
        root = parse_document(xml)
    except ET.ParseError as error:
        raise ValueError(f'Ошибка синтаксиса XML: {error}') from error
    version = root.get('SchemaVersion')
    if root.tag != 'Conclusion' or version not in CONCLUSION_SCHEMAS:
        raise ValueError('Предпросмотр доступен для заключений 01.03 и 01.04.')
    try:
        document = etree.fromstring(ET.tostring(root, encoding='utf-8'))
        result = str(_stylesheet(version)(document))
    except etree.LxmlError as error:
        raise ValueError('Не удалось сформировать предпросмотр заключения по XSL.') from error
    return result.replace('<head>', '<head>' + PREVIEW_POLICY, 1)
