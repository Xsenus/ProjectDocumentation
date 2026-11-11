import { useEffect, useRef, useState } from 'react';
import { fileMetadata, imageType, readBase64 } from './localFiles.js';
import { directElements, insertElement, readElementText, serializeXml, writeElementText } from './xml.js';

const metadataTypes = { FileName: 'tFileName', FileFormat: 'tFileFormat', FileChecksum: 'tFileChecksum' };
// The XML engine accepts up to 32 MiB of UTF-8 XML, including the encoded image.
const maxXmlBytes = 32 * 1024 * 1024;

function localTypeName(name) {
  return name?.split(/[:}]/).pop();
}

function schemaField(type, name, fieldType) {
  if (type?.particle?.kind !== 'sequence') return undefined;
  return type.particle.children?.find(field => field.kind === 'element' && field.name === name && field.max === 1 &&
    field.fixed == null && localTypeName(field.type) === fieldType);
}

export function fileFields(type) {
  const fields = Object.entries(metadataTypes).map(([name, fieldType]) => schemaField(type, name, fieldType));
  return fields.every(Boolean) ? fields : null;
}

function findChild(element, name) {
  return directElements(element, new Set([name]))[0];
}

function setField(element, field, type, types, value) {
  const child = findChild(element, field.name) || insertElement(element, field, type.particle, types);
  writeElementText(child, value);
}

function LocalFilePicker({ label, accept, multiple, onSelect, beginRead }) {
  const input = useRef(null);
  const operation = useRef(null);
  const [reading, setReading] = useState(false);
  const [problem, setProblem] = useState('');
  useEffect(() => () => operation.current?.abort(), []);

  async function select(event) {
    const files = Array.from(event.target.files || []);
    event.target.value = '';
    if (!files.length || operation.current) return;
    const controller = new AbortController();
    operation.current = controller;
    const finishRead = beginRead();
    setReading(true);
    setProblem('');
    try {
      await onSelect(files, controller.signal);
    } catch (error) {
      if (!controller.signal.aborted) setProblem(error.message || 'Не удалось прочитать выбранный файл.');
    } finally {
      finishRead();
      operation.current = null;
      if (!controller.signal.aborted) setReading(false);
    }
  }

  return (
    <div className="field-wide local-file-picker">
      <button type="button" className="button button-quiet" disabled={reading} onClick={() => input.current.click()}>{label}</button>
      <input ref={input} type="file" aria-label={label} hidden disabled={reading} accept={accept} multiple={multiple} onChange={select} />
      {reading && <p className="field-hint" role="status">Читаю выбранные файлы…</p>}
      {problem && <p className="lookup-error" role="alert">{problem}</p>}
    </div>
  );
}

export function FileMetadataPicker({ element, type, fields, context, parentParticle, spec }) {
  const adding = Boolean(spec);
  const repeatable = adding && (spec.max === null || spec.max > 1);
  const fieldValues = () => fields.map(field => readElementText(findChild(element, field.name)));
  const label = adding ? (repeatable ? 'Добавить файлы с диска' : 'Добавить файл с диска')
    : fieldValues().some(Boolean) ? 'Заменить файл с диска' : 'Выбрать файл с диска';

  function checkCapacity(count) {
    const existing = directElements(element, new Set([spec.name])).length;
    if (spec.max !== null && existing + count > spec.max) {
      throw new Error('Выбрано больше файлов, чем можно добавить в эту группу.');
    }
  }

  async function select(files, signal) {
    const before = adding ? [] : fieldValues();
    if (adding) checkCapacity(files.length);
    const metadata = [];
    for (const file of files) metadata.push(await fileMetadata(file, context.familyId, signal));
    signal.throwIfAborted();
    if (!context.isCurrent(element)) return;
    if (adding) checkCapacity(files.length);
    else if (fieldValues().some((value, index) => value !== before[index])) {
      throw new Error('Поля файла изменились во время чтения. Выберите файл снова.');
    }

    context.change(() => {
      for (const values of metadata) {
        const target = adding ? insertElement(element, spec, parentParticle, context.types) : element;
        for (const field of fields) setField(target, field, type, context.types, values[field.name]);
      }
    });
  }

  return <LocalFilePicker label={label} multiple={repeatable} onSelect={select} beginRead={context.beginFileRead} />;
}

export function ImagePicker({ element, type, context }) {
  const field = schemaField(type, 'ImageData', 'base64Binary');
  const typeAttribute = type.attributes?.find(attribute => attribute.name === 'Type' && attribute.fixed == null);
  const allowed = context.types[typeAttribute?.type]?.enumeration?.map(option => option.value);
  if (!field || !allowed?.length) return null;
  const content = () => readElementText(findChild(element, field.name));

  function checkSize(file) {
    const base64Bytes = Math.ceil(file.size / 3) * 4;
    if (base64Bytes > maxXmlBytes || new Blob([serializeXml(element.ownerDocument)]).size -
        new Blob([content()]).size + base64Bytes + 128 > maxXmlBytes) {
      throw new Error('Изображение превышает доступный объём. Размер XML ограничен 32 МиБ.');
    }
  }

  async function select([file], signal) {
    const previous = content();
    const previousType = element.getAttribute('Type');
    checkSize(file);
    const format = await imageType(file, allowed);
    const base64 = await readBase64(file, signal);
    signal.throwIfAborted();
    if (!context.isCurrent(element)) return;
    if (content() !== previous || element.getAttribute('Type') !== previousType) {
      throw new Error('Поля изображения изменились во время чтения. Выберите файл снова.');
    }
    checkSize(file);
    context.change(() => {
      element.setAttribute('Type', format);
      setField(element, field, type, context.types, base64);
    });
  }

  return <LocalFilePicker label={content() ? 'Заменить изображение' : 'Выбрать изображение'}
    accept={allowed.map(format => '.' + format).join(',')} onSelect={select} beginRead={context.beginFileRead} />;
}
