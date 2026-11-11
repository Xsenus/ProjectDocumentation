const XML_HEADER = '<?xml version="1.0" encoding="UTF-8"?>';
const unsafeDeclarations = /<!\s*(DOCTYPE|ENTITY)\b/i;
const declaration = /^\uFEFF?\s*<\?xml\s+[^?]*\?>\s*/i;
const XMLNS = 'http://www.w3.org/2000/xmlns/';

export function parseXml(xml) {
  if (typeof xml !== 'string' || !xml.trim()) throw new Error('XML-документ пуст.');
  if (unsafeDeclarations.test(xml)) throw new Error('XML с объявлением DTD или сущностей не поддерживается.');
  const xml10 = xml.replace(/(^\uFEFF?\s*<\?xml\s+version\s*=\s*["'])1\.1(["'])/, '$11.0$2');
  const document = new DOMParser().parseFromString(xml10, 'application/xml');
  if (document.documentElement?.localName === 'parsererror' ||
      document.getElementsByTagNameNS('http://www.mozilla.org/newlayout/xml/parsererror.xml', 'parsererror').length) {
    throw new Error('Проверьте структуру XML. Документ не удалось прочитать.');
  }
  if (document.documentElement?.localName !== 'Conclusion') {
    throw new Error('Нужен XML заключения экспертизы с корневым элементом Conclusion.');
  }
  return document;
}

export function serializeXml(document) {
  const xml = new XMLSerializer().serializeToString(document).replace(declaration, '');
  return XML_HEADER + '\n' + xml;
}

export function schemaVersion(document) {
  if (!document) return '';
  return document.documentElement.getAttribute('SchemaVersion') || '';
}

export async function readXmlFile(file) {
  const bytes = new Uint8Array(await file.arrayBuffer());
  let encoding;
  if (bytes[0] === 0xFF && bytes[1] === 0xFE || bytes[0] === 0x3C && bytes[1] === 0x00) {
    encoding = 'utf-16le';
  } else if (bytes[0] === 0xFE && bytes[1] === 0xFF || bytes[0] === 0x00 && bytes[1] === 0x3C) {
    encoding = 'utf-16be';
  } else {
    const header = new TextDecoder('windows-1252').decode(bytes.slice(0, 250)).replace(/^ï»¿/, '');
    encoding = /^<\?xml\s+[^?]*\bencoding\s*=\s*['"]([^'"]+)['"]/i.exec(header)?.[1] || 'utf-8';
  }
  return new TextDecoder(encoding, { fatal: true }).decode(bytes);
}

export function fileNameFor(name) {
  const basename = name.trim().replace(/[<>:"/\\|?*\u0000-\u001F]/g, '_').replace(/\.+$/, '');
  return (basename || 'Заключение').replace(/\.xml$/i, '') + '.xml';
}

export function downloadXml(xml, fileName) {
  const url = URL.createObjectURL(xml instanceof Blob ? xml : new Blob([xml], { type: 'application/xml;charset=utf-8' }));
  const link = document.createElement('a');
  link.href = url;
  link.download = fileNameFor(fileName);
  document.body.append(link);
  link.click();
  link.remove();
  setTimeout(() => URL.revokeObjectURL(url), 1000);
}

export function directElements(parent, names) {
  return Array.from(parent?.children || []).filter(child => names.has(child.localName));
}

export function leafNames(particle) {
  if (!particle) return new Set();
  if (particle.kind === 'element') return new Set([particle.name]);
  return new Set((particle.children || []).flatMap(child => [...leafNames(child)]));
}

export function pathFor(element) {
  if (!element.parentElement) return '/' + element.localName;
  const siblings = directElements(element.parentElement, new Set([element.localName]));
  const suffix = siblings.length > 1 ? '[' + (siblings.indexOf(element) + 1) + ']' : '';
  return pathFor(element.parentElement) + '/' + element.localName + suffix;
}

function particleOrder(particle) {
  const ranks = new Map();
  let nextRank = 0;
  function add(part) {
    if (!part) return;
    if (part.kind === 'element') {
      if (!ranks.has(part.name)) ranks.set(part.name, nextRank++);
      return;
    }
    if (part.kind === 'choice' && (part.max === null || part.max > 1)) {
      const rank = nextRank++;
      for (const name of leafNames(part)) if (!ranks.has(name)) ranks.set(name, rank);
      return;
    }
    for (const child of part.children || []) add(child);
  }
  add(particle);
  return ranks;
}

function makeElement(parent, spec) {
  const document = parent.ownerDocument;
  if (parent.namespaceURI) {
    const qualifiedName = parent.prefix ? parent.prefix + ':' + spec.name : spec.name;
    return document.createElementNS(parent.namespaceURI, qualifiedName);
  }
  return document.createElement(spec.name);
}

export function insertElement(parent, spec, parentParticle, types, seen = new Set()) {
  const ranks = particleOrder(parentParticle);
  const rank = ranks.get(spec.name);
  const next = Array.from(parent.children).find(child => ranks.get(child.localName) > rank);
  const element = makeElement(parent, spec);
  parent.insertBefore(element, next || null);

  const value = spec.fixed ?? spec.default;
  if (value !== undefined && value !== null) element.textContent = value;
  const type = types[spec.type];
  if (!type || seen.has(spec.type) || seen.size > 20) return element;
  const nextSeen = new Set(seen);
  nextSeen.add(spec.type);
  for (const attribute of type.attributes || []) {
    const initial = attribute.fixed ?? attribute.default;
    if (initial !== undefined && initial !== null) element.setAttribute(attribute.name, initial);
  }
  addRequired(element, type.particle, type.particle, types, nextSeen);
  return element;
}

function addRequired(parent, particle, parentParticle, types, seen) {
  if (!particle || particle.min === 0) return;
  if (particle.kind === 'element') {
    const count = Math.max(1, particle.min || 0);
    for (let i = 0; i < count; i++) insertElement(parent, particle, parentParticle, types, seen);
    return;
  }
  if (particle.kind === 'choice' && particle.children?.length !== 1) return;
  for (const child of particle.children || []) addRequired(parent, child, parentParticle, types, seen);
}

export function addBranch(parent, branch, parentParticle, types) {
  if (branch.kind === 'element') return insertElement(parent, branch, parentParticle, types);
  addRequired(parent, branch, parentParticle, types, new Set());
  return null;
}

export function readElementText(element) {
  return Array.from(element?.childNodes || [])
    .filter(node => node.nodeType === Node.TEXT_NODE || node.nodeType === Node.CDATA_SECTION_NODE)
    .map(node => node.nodeValue).join('');
}

export function writeElementText(element, value) {
  for (const node of Array.from(element.childNodes)) {
    if (node.nodeType === Node.TEXT_NODE || node.nodeType === Node.CDATA_SECTION_NODE) node.remove();
  }
  if (value !== '') element.prepend(element.ownerDocument.createTextNode(value));
}
export function knownAttributeNames(element, attributes) {
  const declared = new Set(attributes.map(attribute => attribute.name));
  return Array.from(element.attributes)
    .filter(attribute => attribute.namespaceURI !== XMLNS && !declared.has(attribute.name))
    .map(attribute => attribute.name);
}

export async function api(path, data, signal) {
  const response = await fetch('/api/' + path, {
    method: data === undefined ? 'GET' : 'POST',
    headers: data === undefined ? { Accept: 'application/json' } : {
      Accept: 'application/json', 'Content-Type': 'application/json',
    },
    body: data === undefined ? undefined : JSON.stringify(data),
    signal,
  });
  let result;
  try {
    result = await response.json();
  } catch {
    throw new Error('Сервис XML недоступен. Проверьте, что он запущен.');
  }
  if (!response.ok) throw new Error(result.error || 'Сервис XML вернул ошибку.');
  return result;
}
