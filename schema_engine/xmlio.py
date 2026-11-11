from xml.etree import ElementTree as ET


MAX_XML_SIZE = 32 * 1024 * 1024
MAX_XML_DEPTH = 128
MAX_REQUEST_SIZE = MAX_XML_SIZE * 2 + 4096


class _DocumentBuilder(ET.TreeBuilder):
    def __init__(self):
        super().__init__()
        self.depth = 0

    def doctype(self, name, public_id, system_id):
        raise ValueError('DTD и объявления сущностей в XML не поддерживаются.')

    def start(self, tag, attrs):
        self.depth += 1
        if self.depth > MAX_XML_DEPTH:
            raise ValueError('Превышена допустимая глубина XML.')
        return super().start(tag, attrs)

    def end(self, tag):
        self.depth -= 1
        return super().end(tag)


def parse_document(xml: str) -> ET.Element:
    if not isinstance(xml, str) or not xml.strip():
        raise ValueError('Передайте непустой XML-документ.')
    if len(xml.encode('utf-8')) > MAX_XML_SIZE:
        raise ValueError('XML превышает 32 МиБ.')
    return ET.fromstring(xml, parser=ET.XMLParser(target=_DocumentBuilder()))
