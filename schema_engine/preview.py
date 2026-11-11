from functools import lru_cache
from xml.etree import ElementTree as ET

from lxml import etree

from .documents import (
    DOCUMENTS, FAMILIES_BY_ROOT, SCHEMA_DIRECTORY, asset_stem, document_version, legacy_note_format,
)
from .xmlio import parse_document


PREVIEW_POLICY = (
    '<meta http-equiv="Content-Security-Policy" content="default-src \'none\'; '
    'style-src \'unsafe-inline\'; img-src data:; form-action \'none\'; base-uri \'none\'">'
)
POSITIONED_WRAPPERS = {
    'width: 800px; position: relative; left: 50%; margin-left: -400px;',
    'width: 800px; position: absolute; left: 50%; margin-left: -400px;',
}


@lru_cache(maxsize=None)
def _stylesheet(family, version):
    source = SCHEMA_DIRECTORY / (asset_stem(family, version) + '.xsl')
    parser = etree.XMLParser(resolve_entities=False, no_network=True, load_dtd=False)
    return etree.XSLT(etree.parse(str(source), parser), access_control=etree.XSLTAccessControl.DENY_ALL)


def render_preview(xml, *, rich_edit=False):
    try:
        root = parse_document(xml)
    except ET.ParseError as error:
        raise ValueError(f'Ошибка синтаксиса XML: {error}') from error
    family = FAMILIES_BY_ROOT.get(root.tag)
    version = document_version(root, family)
    if family is None or version not in DOCUMENTS[family].versions or legacy_note_format(family, version):
        raise ValueError('Предпросмотр доступен для заключений 01.03 и 01.04, пояснительных записок 01.05 и 01.07.')
    try:
        # parse_document already enforces size, depth and the DTD prohibition.
        # Allow a base64 image to exceed libxml2's default text-node limit.
        parser = etree.XMLParser(huge_tree=True, resolve_entities=False, no_network=True, load_dtd=False)
        document = etree.fromstring(ET.tostring(root, encoding='utf-8'), parser)
        result = _stylesheet(family, version)(document)
        if rich_edit:
            # RichEdit reads the negative margin but ignores the CSS positioning that offsets it.
            for wrapper in result.xpath('/html/body/div[@style]'):
                if wrapper.get('style') in POSITIONED_WRAPPERS:
                    del wrapper.attrib['style']
                    # The HTML title frame is drawn across individual paragraphs by RichEdit.
                    for frame in wrapper.xpath('./div[@class="frame"]'):
                        del frame.attrib['class']
    except etree.LxmlError as error:
        raise ValueError('Не удалось сформировать предпросмотр документа по XSL.') from error
    return str(result).replace('<head>', '<head>' + PREVIEW_POLICY, 1)
