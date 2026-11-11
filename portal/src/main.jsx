import { useEffect, useMemo, useRef, useState } from 'react';
import { createRoot } from 'react-dom/client';
import SchemaForm, { formFieldIndex, groupLabelsFor, normalPath, sectionFor, sectionProgress } from './SchemaForm.jsx';
import { documentFamilies, familyForDocument, familyForId, familyRequest } from './documentFamilies.js';
import { api, downloadXml, encodeXmlBytes, parseXml, readXmlFile, schemaVersion, serializeXml, xmlBlobFromBase64 } from './xml.js';
import './styles.css';

const host = window.chrome?.webview;
const initialFamily = familyForId(new URLSearchParams(window.location.search).get('family'));
const previewCsp = `<meta http-equiv="Content-Security-Policy" content="default-src 'none'; style-src 'unsafe-inline'; img-src data:; base-uri 'none'; form-action 'none'">`;

function loadDrafts() {
  if (host) return {};
  return Object.fromEntries(Object.values(documentFamilies).map(family => {
    try {
      const saved = JSON.parse(localStorage.getItem(family.draftsKey) || '[]');
      const documents = Array.isArray(saved) ? saved.filter(item => item && typeof item.id === 'string' &&
        typeof item.name === 'string' && typeof item.xml === 'string') : [];
      return [family.id, documents];
    } catch {
      return [family.id, []];
    }
  }));
}

function requestMessage(error) {
  return error instanceof TypeError ? 'Сервис XML недоступен. Проверьте, что он запущен.' : error.message;
}

function versionLabel(version) {
  return version || 'версия не указана';
}

function defaultName(document) {
  const family = familyForDocument(document);
  const children = Array.from(document.documentElement.children);
  if (family.id === 'explanatorynote') {
    const number = children.find(node => node.localName === 'ExplanatoryNoteNumber')?.textContent?.trim();
    return number ? family.title + ' № ' + number : family.title;
  }
  const object = children.find(node => node.localName === 'Object');
  const name = object && Array.from(object.children).find(node => /^(Name|ObjectName)$/i.test(node.localName))?.textContent?.trim();
  return name || family.title;
}

function nameFromFile(name) {
  return name.replace(/\.xml$/i, '');
}

function isExactRevision(revision, serial) {
  return revision.current === serial;
}

function sandboxedPreview(html) {
  const head = /<head(?:\s[^>]*)?>/i;
  return head.test(html) ? html.replace(head, match => match + previewCsp) : previewCsp + html;
}

function exportCopy(document, family, version) {
  if (!family.schemaVersions.includes(version)) return serializeXml(document);
  const copy = document.cloneNode(true);
  const base = family.id + '-' + version.replace('.', '-');
  const xsi = 'http://www.w3.org/2001/XMLSchema-instance';
  copy.documentElement.setAttributeNS(xsi, 'xsi:noNamespaceSchemaLocation', base + '.xsd');
  Array.from(copy.childNodes)
    .filter(node => node.nodeType === Node.PROCESSING_INSTRUCTION_NODE && node.target === 'xml-stylesheet')
    .forEach(node => node.remove());
  if (family.stylesheetVersions.includes(version)) {
    const style = copy.createProcessingInstruction('xml-stylesheet', 'type="text/xsl" href="' + base + '.xsl"');
    copy.insertBefore(style, copy.documentElement);
  }
  return serializeXml(copy);
}

function ValidationPanel({ validation, onField, navigationDisabled }) {
  const panel = useRef(null);
  useEffect(() => {
    if (validation && !validation.valid) panel.current?.focus();
  }, [validation]);
  if (!validation) return null;
  const errors = validation.errors || [];
  return (
    <aside ref={panel} tabIndex={-1} className={'validation ' + (validation.valid ? 'valid' : 'invalid')}
      aria-label="Результат локальной проверки XML" aria-live="polite">
      <div className="validation-header">
        <span className="validation-icon" aria-hidden="true">{validation.valid ? '✓' : '!'}</span>
        <div>
          <strong>{validation.valid ? 'XML прошёл локальную проверку' : 'XML требует исправлений'}</strong>
          <p>{validation.validationBasis === 'legacy-wire' ? 'Совместимость с форматом приложения 01.03' :
            validation.valid ? 'XSD ' + versionLabel(validation.version) : 'Ошибок: ' + errors.length}</p>
        </div>
      </div>
      {errors.length > 0 && (
        <ol className="validation-list">
          {errors.map((error, index) => (
            <li key={index}>
              {error.path ? (
                <button type="button" className="error-path" disabled={navigationDisabled} onClick={() => onField(error.path)}>
                  {error.path}{error.line && ' · строка ' + error.line}
                </button>
              ) : error.line ? <span className="error-path">Строка {error.line}</span> : null}
              <span>{error.message}</span>
            </li>
          ))}
        </ol>
      )}
    </aside>
  );
}

function searchText(value) {
  return value.toLocaleLowerCase('ru').replaceAll('ё', 'е');
}

function FieldSearch({ index, onField, onSearchFocus, disabled }) {
  const [query, setQuery] = useState('');
  const [limit, setLimit] = useState(10);
  const typedPath = query.trim().startsWith('/') ? xmlPathParts(query.trim()) : [];
  const needle = searchText(query.trim()).replace(/\[\d+\]/g, '');
  const terms = needle.split(/\s+/).filter(Boolean);
  function priority(item) {
    if (searchText(item.path) === needle) return 2;
    return Number(searchText(item.title) === needle || searchText(item.path.split('/').at(-1)) === needle);
  }
  const matches = terms.length ? index.filter(item => {
    const searchable = searchText(`${item.title} ${item.path} ${item.section} ${item.documentation || ''}`);
    return terms.every(term => searchable.includes(term));
  }).sort((first, second) => priority(second) - priority(first)) : [];
  function choose(path) {
    setQuery('');
    setLimit(10);
    const exactPath = typedPath.length > 0 && searchText(path) === needle;
    const target = exactPath ? '/' + xmlPathParts(path).map((part, index) =>
      part + (typedPath[index]?.match(/\[\d+\]$/)?.[0] || '')).join('/') : path;
    onField(target);
  }
  return (
    <form className="field-search" role="search" onSubmit={event => {
      event.preventDefault();
      if (!disabled && matches.length) choose(matches[0].path);
    }}>
      <label htmlFor="schema-field-search">Найти поле</label>
      <input id="schema-field-search" type="search" value={query} disabled={disabled} spellCheck={false}
        placeholder="Название или имя XML" onFocus={onSearchFocus}
        onChange={event => { setQuery(event.target.value); setLimit(10); }} />
      {terms.length > 0 && !disabled && (
        <>
          <p className="field-search-count" role="status">{matches.length ? 'Найдено: ' + matches.length : 'Поля не найдены'}</p>
          {matches.length > 0 && <ul className="field-search-results">
            {matches.slice(0, limit).map(item => <li key={item.path}>
              <button type="button" onClick={() => choose(item.path)}>
                <strong>{item.title}</strong><span>{item.section}</span><small>{item.path}</small>
              </button>
            </li>)}
          </ul>}
          {matches.length > limit && <button className="text-button" type="button" onClick={() => setLimit(limit + 10)}>Показать ещё</button>}
        </>
      )}
    </form>
  );
}

function matchesPathPart(actual, expected) {
  if (actual === expected) return true;
  const actualName = actual.replace(/\[\d+\]$/, '');
  const expectedName = expected.replace(/\[\d+\]$/, '');
  return actualName === expectedName && (expectedName === expected || actualName === actual && expected.endsWith('[1]'));
}

function xmlPathParts(path) {
  return normalPath(path).split('/').filter(Boolean);
}

function isPathPrefix(parts, target) {
  return parts.length <= target.length && parts.every((part, index) => matchesPathPart(part, target[index]));
}

function resolveOccurrence(document, parts) {
  let element = document.documentElement;
  return parts.map((part, depth) => {
    if (depth === 0 || !element || part.startsWith('@')) return part;
    const index = Number(part.match(/\[(\d+)\]$/)?.[1] || 1);
    const name = part.replace(/\[\d+\]$/, '');
    const siblings = Array.from(element.children).filter(child => child.localName === name);
    element = siblings[index - 1];
    return element && siblings.length > 1 ? `${name}[${index}]` : part;
  });
}

function MigrationPanel({ result, onApply, onCancel }) {
  if (!result) return null;
  return (
    <aside className="migration" aria-live="polite">
      <div className="validation-header">
        <span className="validation-icon" aria-hidden="true">↻</span>
        <div>
          <strong>Преобразование {versionLabel(result.sourceVersion)} → {result.targetVersion}</strong>
          <p>{result.warnings?.length ? 'Проверьте предупреждения перед применением.' : 'Результат преобразования готов.'}</p>
        </div>
      </div>
      {result.warnings?.length > 0 && (
        <ul className="migration-list">{result.warnings.map((warning, index) => <li key={index}>{warning}</li>)}</ul>
      )}
      <div className="inline-actions">
        <button className="button button-primary" type="button" onClick={onApply}>Открыть результат</button>
        <button className="button button-quiet" type="button" onClick={onCancel}>Отмена</button>
      </div>
    </aside>
  );
}

function App() {
  const [profiles, setProfiles] = useState({});
  const [selectedFamily, setSelectedFamily] = useState(initialFamily);
  const [document, setDocument] = useState(null);
  const [name, setName] = useState('');
  const [section, setSection] = useState('details');
  const [tab, setTab] = useState('form');
  const [busy, setBusy] = useState('');
  const [problem, setProblem] = useState('');
  const [notice, setNotice] = useState('');
  const [dirty, setDirty] = useState(false);
  const [readOnly, setReadOnly] = useState(false);
  const [desktopSaving, setDesktopSaving] = useState(false);
  const [fileReads, setFileReads] = useState(0);
  const [validation, setValidation] = useState(null);
  const [preview, setPreview] = useState('');
  const [migration, setMigration] = useState(null);
  const [conversionWarnings, setConversionWarnings] = useState([]);
  const [original, setOriginal] = useState(null);
  const [raw, setRaw] = useState('');
  const [rawBase, setRawBase] = useState('');
  const [draftsByFamily, setDraftsByFamily] = useState(loadDrafts);
  const [activeDraft, setActiveDraft] = useState(null);
  const [showDrafts, setShowDrafts] = useState(false);
  const [, refresh] = useState(0);
  const revision = useRef(0);
  const navigationRequest = useRef(0);
  const operation = useRef('');
  const desktopSavePending = useRef(false);
  const fileInput = useRef(null);
  const documentRef = useRef(null);
  const keys = useRef(new WeakMap());
  const nextKey = useRef(0);
  const family = familyForDocument(document) || selectedFamily;
  const currentVersion = family.latestVersion;
  const currentDocumentVersion = schemaVersion(document);
  const hasFormVersion = family.formVersions.includes(currentDocumentVersion);
  const profileVersion = hasFormVersion ? currentDocumentVersion : currentVersion;
  const profileKey = family.id + '@' + profileVersion;
  const model = profiles[profileKey];
  const formReady = Boolean(model && document && model.version === currentDocumentVersion);
  const groupLabels = groupLabelsFor(family.id);
  const drafts = draftsByFamily[family.id] || [];
  const migrationTarget = Object.hasOwn(family.migrationTargets, currentDocumentVersion)
    ? family.migrationTargets[currentDocumentVersion]
    : !currentDocumentVersion && family.id === 'explanatorynote' ? currentVersion : undefined;
  const rawDirty = raw !== rawBase;
  const currentValidation = rawDirty ? null : validation;
  const fieldErrors = useMemo(() => {
    const result = new Map();
    for (const error of currentValidation?.errors || []) {
      const path = normalPath(error.path);
      const messages = result.get(path) || [];
      if (!messages.includes(error.message)) messages.push(error.message);
      result.set(path, messages);
    }
    return result;
  }, [currentValidation]);
  const fieldIndex = useMemo(() => model ? formFieldIndex(model, family.id) : [], [model, family.id]);

  function keyFor(element) {
    if (!keys.current.has(element)) keys.current.set(element, ++nextKey.current);
    return keys.current.get(element);
  }

  function replaceDocument(xml, title, options = {}) {
    const next = parseXml(xml);
    const nextFamily = familyForDocument(next);
    const canonical = serializeXml(next);
    documentRef.current = next;
    revision.current++;
    setSelectedFamily(nextFamily);
    setDocument(next);
    setName(title?.trim() || defaultName(next));
    setReadOnly(Boolean(options.readOnly));
    setSection(options.section ? sectionFor(options.section, nextFamily.id) : 'details');
    setTab(nextFamily.formVersions.includes(schemaVersion(next)) ? 'form' : 'xml');
    setValidation(null);
    setPreview('');
    setMigration(null);
    setConversionWarnings(options.warnings || []);
    setOriginal(options.original || null);
    setRaw(canonical);
    setRawBase(canonical);
    setDirty(Boolean(options.dirty));
    setActiveDraft(options.draftId || null);
    setProblem('');
    setNotice('');
  }

  useEffect(() => {
    window.document.title = family.collection;
  }, [family]);

  useEffect(() => {
    if (model) return undefined;
    const controller = new AbortController();
    api(familyRequest('schema', family, profileVersion), undefined, controller.signal).then(profile => {
      if (profile.version !== profileVersion || profile.root?.name !== family.root) {
        throw new Error('Сервис вернул профиль другой схемы.');
      }
      setProfiles(previous => ({ ...previous, [profileKey]: profile }));
    }).catch(error => {
      if (error.name !== 'AbortError') setProblem(requestMessage(error));
    });
    return () => controller.abort();
  }, [family, profileVersion, profileKey, model]);

  useEffect(() => {
    if (host) return undefined;
    const controller = new AbortController();
    api(familyRequest('new', initialFamily), undefined, controller.signal).then(({ xml }) => {
      if (!documentRef.current && !operation.current) replaceDocument(xml, initialFamily.newTitle);
    }).catch(error => {
      if (error.name !== 'AbortError' && !documentRef.current) setProblem(requestMessage(error));
    });
    return () => controller.abort();
  }, []);

  useEffect(() => {
    if (!host) return undefined;
    const handleMessage = ({ data }) => {
      let message = data;
      if (typeof data === 'string') {
        try { message = JSON.parse(data); } catch { return; }
      }
      if (message?.type === 'load') {
        finishDesktopSave();
        if (typeof message.xml === 'string' && message.xml.trim()) {
          try { replaceDocument(message.xml, message.name, message); } catch (error) { setProblem(error.message); }
        } else {
          const requestedFamily = message.family ? familyForId(message.family) : initialFamily;
          api(familyRequest('new', requestedFamily))
            .then(({ xml }) => replaceDocument(xml, message.name || requestedFamily.newTitle, message))
            .catch(error => setProblem(requestMessage(error)));
        }
      } else if (message?.type === 'error') {
        finishDesktopSave();
        setProblem(message.message || 'Не удалось сохранить документ.');
      } else if (message?.type === 'saved') {
        finishDesktopSave();
        setNotice('Документ сохранён.');
        setDirty(false);
      }
    };
    host.addEventListener('message', handleMessage);
    host.postMessage({ type: 'ready' });
    return () => host.removeEventListener('message', handleMessage);
  }, []);

  useEffect(() => {
    if (host) host.postMessage({ type: 'dirty', dirty: Boolean(!readOnly && (dirty || rawDirty)) });
  }, [dirty, rawDirty, readOnly]);

  useEffect(() => {
    if (host || (!dirty && !rawDirty)) return undefined;
    const unload = event => { event.preventDefault(); event.returnValue = ''; };
    window.addEventListener('beforeunload', unload);
    return () => window.removeEventListener('beforeunload', unload);
  }, [dirty, rawDirty]);

  function change(callback) {
    if (readOnly || desktopSavePending.current) return;
    callback();
    revision.current++;
    setDirty(true);
    setValidation(null);
    setPreview('');
    setMigration(null);
    setNotice('');
    refresh(value => value + 1);
  }

  function permitReplacement() {
    return !dirty && !rawDirty || window.confirm('Открыть другой документ? Несохранённые изменения будут потеряны.');
  }

  async function run(label, work) {
    if (operation.current) return;
    operation.current = label;
    setBusy(label);
    setProblem('');
    setNotice('');
    try {
      await work();
    } catch (error) {
      setProblem(requestMessage(error));
    } finally {
      operation.current = '';
      setBusy('');
    }
  }

  function newDocument(requestedFamily = family) {
    if (!permitReplacement()) return;
    const serial = revision.current;
    run(requestedFamily.createProgress, async () => {
      const { xml } = await api(familyRequest('new', requestedFamily));
      if (!isExactRevision(revision, serial)) {
        setNotice('Документ изменился во время создания. Повторите создание нового документа.');
        return;
      }
      replaceDocument(xml, requestedFamily.newTitle);
      setShowDrafts(false);
    });
  }

  function importFile(event) {
    const file = event.target.files?.[0];
    event.target.value = '';
    if (!file) return;
    const serial = revision.current;
    run('Читаю XML', async () => {
      const { xml, bytes } = await readXmlFile(file);
      if (!isExactRevision(revision, serial)) {
        setNotice('Документ изменился во время чтения файла. Откройте XML ещё раз.');
        return;
      }
      replaceDocument(xml, nameFromFile(file.name), { original: { bytes, file, name: nameFromFile(file.name) } });
      setShowDrafts(false);
      setNotice('Файл открыт. Версия схемы: ' + versionLabel(schemaVersion(documentRef.current)) + '.');
    });
  }

  function openXmlView() {
    if (!rawDirty && document) {
      const xml = serializeXml(document);
      setRaw(xml);
      setRawBase(xml);
    }
    setTab('xml');
  }

  function applyRaw() {
    try {
      const next = parseXml(raw);
      const nextFamily = familyForDocument(next);
      if (nextFamily.id !== family.id) {
        if (host) throw new Error('Тип открытого документа нельзя изменить.');
        setSelectedFamily(nextFamily);
        setSection('details');
        setActiveDraft(null);
        setOriginal(null);
        setConversionWarnings([]);
      }
      if (host && next.documentElement.getAttribute(family.idAttribute) !== document.documentElement.getAttribute(family.idAttribute)) {
        throw new Error('Идентификатор документа задаётся приложением. Восстановите его исходное значение в XML.');
      }
      const canonical = serializeXml(next);
      documentRef.current = next;
      setDocument(next);
      setRaw(canonical);
      setRawBase(canonical);
      change(() => {});
      setProblem('');
      setNotice('Изменения XML применены.');
    } catch (error) {
      setProblem(error.message);
    }
  }

  function revertRaw() {
    const xml = serializeXml(document);
    setRaw(xml);
    setRawBase(xml);
    setProblem('');
  }

  async function validateSnapshot(xml, serial) {
    const result = await api('validate', { xml });
    if (!isExactRevision(revision, serial)) return null;
    setValidation(result);
    return result;
  }

  function validateCurrent() {
    run('Проверяю XML', () => validateSnapshot(serializeXml(document), revision.current));
  }

  function showPreview() {
    const serial = revision.current;
    const xml = serializeXml(document);
    run('Формирую предпросмотр', async () => {
      const result = await api('preview', { xml });
      if (isExactRevision(revision, serial)) {
        setPreview(result.html);
        setTab('preview');
      }
    });
  }

  function exportCurrent() {
    run('Проверяю перед выгрузкой', async () => {
      const xml = exportCopy(document, family, currentDocumentVersion);
      const result = await validateSnapshot(xml, revision.current);
      if (result?.valid) {
        downloadXml(xml, name + ' ' + currentDocumentVersion);
        setNotice(family.schemaVersions.includes(currentDocumentVersion)
          ? 'XML выгружен. Схему можно скачать в разделе «Файлы схемы».'
          : 'XML выгружен в исходном формате приложения.');
      } else if (result) {
        setNotice('Исправьте ошибки перед выгрузкой или скачайте черновик.');
      }
    });
  }

  function finishDesktopSave() {
    desktopSavePending.current = false;
    setDesktopSaving(false);
  }

  function desktopSave() {
    if (desktopSavePending.current || operation.current) return;
    desktopSavePending.current = true;
    setDesktopSaving(true);
    run('Проверяю перед сохранением', async () => {
      let submitted = false;
      try {
        const xml = serializeXml(document);
        const result = await validateSnapshot(xml, revision.current);
        if (result?.valid) {
          host.postMessage({ type: 'save', xml, name: name.trim() || defaultName(document) });
          submitted = true;
        }
      } finally {
        if (!submitted) finishDesktopSave();
      }
    });
  }

  function previewMigration() {
    const serial = revision.current;
    const snapshot = serializeXml(document);
    if (!migrationTarget) return;
    run('Преобразую XML', async () => {
      const result = await api('migrate', { xml: snapshot, targetVersion: migrationTarget });
      if (isExactRevision(revision, serial)) setMigration({ ...result, snapshot, serial });
    });
  }

  function applyMigration() {
    if (!migration || !isExactRevision(revision, migration.serial)) return;
    try {
      replaceDocument(migration.xml, name, {
        warnings: migration.warnings,
        original: original || { xml: migration.snapshot, name },
        dirty: true,
        draftId: activeDraft,
      });
      setNotice('Открыт результат преобразования. Проверьте обязательные поля.');
    } catch (error) {
      setProblem(error.message);
    }
  }

  function saveDraft() {
    const id = activeDraft || crypto.randomUUID();
    try {
      const draft = {
        id, name: name.trim() || defaultName(document), xml: serializeXml(document), updatedAt: new Date().toISOString(),
        original: original ? {
          name: original.name,
          base64: original.bytes ? encodeXmlBytes(original.bytes) : original.base64,
          xml: original.xml,
        } : undefined,
        warnings: conversionWarnings,
      };
      const updated = [draft, ...drafts.filter(item => item.id !== id)];
      localStorage.setItem(family.draftsKey, JSON.stringify(updated));
      setDraftsByFamily(previous => ({ ...previous, [family.id]: updated }));
      setActiveDraft(id);
      setDirty(false);
      setNotice('Черновик сохранён в этом браузере.');
    } catch {
      setProblem('Браузер не смог сохранить черновик. Скачайте его в XML.');
    }
  }

  function openDraft(draft) {
    if (!permitReplacement()) return;
    try {
      replaceDocument(draft.xml, draft.name, { draftId: draft.id, original: draft.original, warnings: draft.warnings });
      setShowDrafts(false);
    } catch (error) {
      setProblem(error.message);
    }
  }

  function deleteDraft(draft) {
    if (!window.confirm('Удалить черновик «' + draft.name + '» из браузера?')) return;
    const updated = drafts.filter(item => item.id !== draft.id);
    try {
      localStorage.setItem(family.draftsKey, JSON.stringify(updated));
      setDraftsByFamily(previous => ({ ...previous, [family.id]: updated }));
      if (activeDraft === draft.id) {
        setActiveDraft(null);
        setDirty(true);
      }
    } catch {
      setProblem('Не удалось удалить черновик из хранилища браузера.');
    }
  }

  function navigateField(path) {
    if (rawDirty || readingFiles) return;
    setShowDrafts(false);
    if (!formReady) {
      openXmlView();
      requestAnimationFrame(() => window.document.querySelector('.raw-editor')?.focus());
      return;
    }
    const parts = resolveOccurrence(document, xmlPathParts(path));
    const name = parts[1]?.replace(/\[\d+\]$/, '');
    const request = ++navigationRequest.current;
    setSection(sectionFor(name, family.id));
    setTab('form');
    function reveal(remainingFrames) {
      if (documentRef.current !== document || navigationRequest.current !== request) return;
      const root = window.document.querySelector('.schema-form');
      const ancestors = Array.from(root?.querySelectorAll('[data-xml-path]') || []).flatMap(node => {
        const candidate = xmlPathParts(node.dataset.xmlPath);
        return isPathPrefix(candidate, parts) ? [{ node, depth: candidate.length }] : [];
      });
      const closed = ancestors.find(({ node }) => node.tagName === 'DETAILS' && !node.open);
      const opening = ancestors.some(({ node }) => node.tagName === 'DETAILS' && node.open &&
        !node.querySelector(':scope > .complex-content'));
      if ((!root || closed || opening) && remainingFrames > 0) {
        if (closed) closed.node.open = true;
        requestAnimationFrame(() => reveal(remainingFrames - 1));
        return;
      }
      const candidates = ancestors.flatMap(({ node, depth }) => {
        const control = node.classList.contains('field') ? node.querySelector('input, select, textarea')
          : node.tagName === 'BUTTON' ? node : node.querySelector(':scope > .button-add');
        if (control && !control.disabled) return [{ control, depth, priority: node.classList.contains('field') ? 2 : 1 }];
        const summary = node.tagName === 'DETAILS' && node.querySelector(':scope > summary');
        return summary ? [{ control: summary, depth, priority: depth === parts.length ? 2 : 0 }] : [];
      });
      for (const choice of root?.querySelectorAll('[data-choice-parent-path]') || []) {
        const parent = xmlPathParts(choice.dataset.choiceParentPath);
        const next = parts[parent.length]?.replace(/\[\d+\]$/, '');
        const picker = choice.querySelector(':scope > .choice-picker > select, :scope > .choice-switch');
        if (picker && isPathPrefix(parent, parts) && choice.dataset.choiceElements.split(' ').includes(next)) {
          candidates.push({ control: picker, depth: parent.length + 1, priority: 1 });
        }
      }
      const target = candidates.sort((a, b) => b.depth - a.depth || b.priority - a.priority)[0]?.control;
      (target || root)?.scrollIntoView({ block: 'center' });
      target?.focus({ preventScroll: true });
    }
    requestAnimationFrame(() => reveal(60));
  }

  const readingFiles = fileReads > 0;
  const actionsDisabled = Boolean(busy || desktopSaving || readingFiles || rawDirty || !document);
  const editorLocked = readOnly || desktopSaving;
  const hasSchemaAssets = family.schemaVersions.includes(currentDocumentVersion);
  const hasPreview = family.stylesheetVersions.includes(currentDocumentVersion);
  const assetVersion = hasSchemaAssets ? currentDocumentVersion : currentVersion;
  const assetBase = '/schemas/' + family.id + '-' + assetVersion.replace('.', '-');
  const context = { types: model?.types || {}, depth: 0, readOnly: editorLocked, lockGuid: Boolean(host),
    idAttribute: family.idAttribute, familyId: family.id, bridge: host, fieldErrors, change, keyFor,
    isCurrent: element => element.isConnected && element.ownerDocument === documentRef.current,
    beginFileRead() {
      setFileReads(count => count + 1);
      return () => setFileReads(count => count - 1);
    } };

  return (
    <div className="application">
      <header className="app-header">
        <div className="brand">
          <span className="brand-symbol" aria-hidden="true"><i></i><i></i><i></i></span>
          <div><strong>Проектная документация</strong><span>{family.collection}</span></div>
        </div>
        <div className="header-actions">
          {!host && (
            <>
              <select className="header-family" aria-label="Вид документа" value={family.id} disabled={Boolean(busy || readingFiles)}
                onChange={event => newDocument(familyForId(event.target.value))}>
                {Object.values(documentFamilies).map(option => (
                  <option key={option.id} value={option.id}>{option.title}</option>
                ))}
              </select>
              <button type="button" className="header-button" disabled={Boolean(busy || readingFiles)} onClick={() => newDocument()}>{family.newAction}</button>
              <button type="button" className="header-button header-primary" disabled={Boolean(busy || readingFiles)}
                onClick={() => permitReplacement() && fileInput.current.click()}>Открыть XML</button>
            </>
          )}
          {host && <span className="header-mode">{readOnly ? 'Просмотр' : 'Редактирование'}</span>}
          <input type="file" hidden ref={fileInput} accept=".xml,application/xml,text/xml" onChange={importFile} />
        </div>
      </header>
      <div className="app-layout">
        <aside className="sidebar" aria-label="Рабочая область">
          <div className="sidebar-inner">
            {!host && (
              <div className="workspace-switch" role="group" aria-label="Рабочая область">
                <button className={!showDrafts ? 'selected' : ''} type="button" onClick={() => setShowDrafts(false)}>Редактор</button>
                <button className={showDrafts ? 'selected' : ''} type="button" disabled={readingFiles} onClick={() => setShowDrafts(true)}>Черновики <span>{drafts.length}</span></button>
              </div>
            )}
            {formReady && <FieldSearch key={profileKey} index={fieldIndex} onField={navigateField}
              onSearchFocus={() => { navigationRequest.current++; }} disabled={rawDirty || readingFiles} />}
            <h2>{family.sectionsTitle}</h2>
            <nav className="section-nav" aria-label={family.sectionsTitle}>
              {Object.entries(groupLabels).map(([id, label]) => (
                <button type="button" key={id} className={!showDrafts && tab === 'form' && section === id ? 'active' : ''}
                  aria-current={!showDrafts && tab === 'form' && section === id ? 'page' : undefined}
                  disabled={!formReady || rawDirty || readingFiles}
                  onClick={() => { setShowDrafts(false); setSection(id); setTab('form'); }}>
                  <span>{label}</span>
                  {formReady && sectionProgress(model, document, id) > 0 && <span className="nav-dot" aria-hidden="true"></span>}
                </button>
              ))}
            </nav>
            <div className="sidebar-bottom">
              <h2>Файлы схемы</h2>
              <a href={assetBase + '.xsd'} download>Схема XSD {assetVersion}</a>
              {family.stylesheetVersions.includes(assetVersion) && <a href={assetBase + '.xsl'} download>Стиль XSL {assetVersion}</a>}
            </div>
            {!host && <div className="sidebar-bottom">
              <h2>Проверка и реестр</h2>
              <a href="https://checkxml.gge.ru/" target="_blank" rel="noreferrer">Сервис проверки XML Главгосэкспертизы</a>
              <a href="https://egrz.ru/organisation/reestr-advance/latest" target="_blank" rel="noreferrer">Поиск заключений в ЕГРЗ</a>
            </div>}
          </div>
        </aside>

        <main className="main-content">
          {problem && <div className="feedback feedback-error" role="alert">{problem}</div>}
          {notice && <div className="feedback feedback-info" role="status">{notice}</div>}
          {(busy || desktopSaving) && <p className="live-progress" role="status"><span aria-hidden="true"></span>{busy || 'Сохраняю в приложении'}…</p>}

          {showDrafts ? (
            <section className="paper library">
              <h1>Черновики</h1>
              <p className="muted">{family.collection}, сохранённые в этом браузере.</p>
              {drafts.length ? <ul className="draft-list">{drafts.map(draft => (
                <li key={draft.id}>
                  <button type="button" className="draft-item" onClick={() => openDraft(draft)}>
                    <strong>{draft.name}</strong>
                    <span>{new Date(draft.updatedAt).toLocaleString('ru-RU')}</span>
                  </button>
                  <button type="button" className="text-button" onClick={() => deleteDraft(draft)}>Удалить</button>
                </li>
              ))}</ul> : <p className="empty-state">Сохранённых черновиков пока нет.</p>}
            </section>
          ) : !document ? (
            <section className="paper empty-document">
              <h1>{family.title}</h1>
              <p className="muted">{host ? 'Открываю документ…' : 'Создайте документ или откройте XML.'}</p>
              {!host && <div className="inline-actions">
                <button className="button button-primary" type="button" disabled={Boolean(busy)}
                  onClick={() => newDocument()}>{family.createAction}</button>
                <button className="button button-quiet" type="button" disabled={Boolean(busy)} onClick={() => fileInput.current.click()}>Открыть XML</button>
              </div>}
            </section>
          ) : (
            <>
              <div className="document-head">
                <div>
                  <div className="document-meta">
                    <span className="version-label">XML {versionLabel(currentDocumentVersion)}</span>
                    {(dirty || rawDirty) && <span>Есть несохранённые изменения</span>}
                  </div>
                  <h1>{name.trim() || family.title}</h1>
                </div>
                <div className="document-actions">
                  <button type="button" className="button button-quiet" disabled={actionsDisabled} onClick={validateCurrent}>Проверить XML</button>
                  {host && !readOnly
                    ? <button type="button" className="button button-primary" disabled={actionsDisabled || currentDocumentVersion !== currentVersion}
                        onClick={desktopSave}>Сохранить</button>
                    : <button type="button" className="button button-primary" disabled={actionsDisabled} onClick={exportCurrent}>Выгрузить XML</button>}
                </div>
              </div>
              {!readOnly && (
                <div className="document-toolbar">
                  <label className="document-name">
                    <span>Название документа</span>
                    <input value={name} readOnly={editorLocked} onChange={event => {
                      setName(event.target.value);
                      revision.current++;
                      setDirty(true);
                      setMigration(null);
                    }} />
                  </label>
                  <div className="utility-actions">
                    {!host && <button type="button" className="text-button" disabled={actionsDisabled} onClick={saveDraft}>Сохранить в браузере</button>}
                    {migrationTarget && (
                      <button type="button" className="text-button" disabled={actionsDisabled} onClick={previewMigration}>
                        Преобразовать в {migrationTarget}
                      </button>
                    )}
                    {host && currentDocumentVersion !== currentVersion && (
                      <button type="button" className="text-button" disabled={actionsDisabled} onClick={exportCurrent}>Выгрузить XML</button>
                    )}
                    {original && <button type="button" className="text-button" onClick={() => {
                      try {
                        const source = original.file || (typeof original.base64 === 'string'
                          ? xmlBlobFromBase64(original.base64) : serializeXml(parseXml(original.xml)));
                        downloadXml(source, original.name + ' (исходный)');
                      } catch (error) { setProblem(error.message); }
                    }}>Скачать исходный XML</button>}
                    <button type="button" className="text-button" disabled={actionsDisabled}
                      onClick={() => downloadXml(serializeXml(document), name + ' (черновик)')}>Скачать черновик</button>
                  </div>
                </div>
              )}

              <MigrationPanel result={migration} onApply={applyMigration} onCancel={() => setMigration(null)} />
              {conversionWarnings.length > 0 && (
                <details className="conversion-notes">
                  <summary>Предупреждения преобразования ({conversionWarnings.length})</summary>
                  <ul>{conversionWarnings.map((warning, index) => <li key={index}>{warning}</li>)}</ul>
                </details>
              )}
              <ValidationPanel validation={currentValidation} onField={navigateField} navigationDisabled={readingFiles} />

              <section className="paper editor">
                <div className="editor-heading">
                  <h2>{tab === 'preview' ? 'Предпросмотр' : tab === 'form' && hasFormVersion ? groupLabels[section] : 'Исходный XML'}</h2>
                  <div className="editor-switch" role="group" aria-label="Вид редактора">
                    <button type="button" className={tab === 'form' ? 'selected' : ''} disabled={!formReady || rawDirty || readingFiles}
                      onClick={() => setTab('form')}>Форма</button>
                    <button type="button" className={tab === 'xml' || tab !== 'preview' && !hasFormVersion ? 'selected' : ''} disabled={readingFiles} onClick={openXmlView}>XML</button>
                    <button type="button" className={tab === 'preview' ? 'selected' : ''} disabled={actionsDisabled || !hasPreview}
                      onClick={showPreview}>Предпросмотр</button>
                  </div>
                </div>
                {tab === 'preview' ? (
                  <iframe className="print-preview" title={family.previewTitle} sandbox="" referrerPolicy="no-referrer"
                    srcDoc={sandboxedPreview(preview)} />
                ) : tab === 'form' && hasFormVersion ? (
                  formReady ? <SchemaForm key={keyFor(document.documentElement)} model={model} document={document} section={section} context={context} />
                    : <p className="empty-state">{problem ? 'Форма недоступна. Откройте вкладку XML.' : 'Загружаю поля схемы…'}</p>
                ) : (
                  <div className="raw-panel">
                    {!family.formVersions.includes(currentDocumentVersion) && (
                      <p className="feedback feedback-info">
                        Форма доступна для {family.formVersions.join(' и ')}. Этот документ можно открыть как XML
                        {migrationTarget && ' и преобразовать в ' + migrationTarget}.
                      </p>
                    )}
                    <textarea className="raw-editor" aria-label="Содержимое XML" value={raw} spellCheck={false} readOnly={editorLocked}
                      onChange={event => { setRaw(event.target.value); setMigration(null); revision.current++; }} />
                    {rawDirty && !editorLocked && (
                      <div className="inline-actions">
                        <button className="button button-primary" type="button" onClick={applyRaw}>Применить XML</button>
                        <button className="button button-quiet" type="button" onClick={revertRaw}>Отменить изменения XML</button>
                      </div>
                    )}
                  </div>
                )}
              </section>
            </>
          )}
        </main>
      </div>
    </div>
  );
}

createRoot(window.document.getElementById('root')).render(<App />);
