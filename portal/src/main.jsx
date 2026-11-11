import { useEffect, useMemo, useRef, useState } from 'react';
import { createRoot } from 'react-dom/client';
import SchemaForm, { groupLabelsFor, normalPath, sectionFor, sectionProgress } from './SchemaForm.jsx';
import { documentFamilies, familyForDocument, familyForId, familyRequest } from './documentFamilies.js';
import { api, downloadXml, parseXml, readXmlFile, schemaVersion, serializeXml } from './xml.js';
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
  const style = copy.createProcessingInstruction('xml-stylesheet', 'type="text/xsl" href="' + base + '.xsl"');
  copy.insertBefore(style, copy.documentElement);
  return serializeXml(copy);
}

function ValidationPanel({ validation, onField }) {
  if (!validation) return null;
  const errors = validation.errors || [];
  return (
    <aside className={'validation ' + (validation.valid ? 'valid' : 'invalid')} aria-live="polite">
      <div className="validation-header">
        <span className="validation-icon" aria-hidden="true">{validation.valid ? '✓' : '!'}</span>
        <div>
          <strong>{validation.valid ? 'XML прошёл проверку' : 'XML требует исправлений'}</strong>
          <p>{validation.validationBasis === 'legacy-wire' ? 'Совместимость с форматом приложения 01.03' :
            validation.valid ? 'Схема ' + versionLabel(validation.version) : 'Ошибок: ' + errors.length}</p>
        </div>
      </div>
      {errors.length > 0 && (
        <ol className="validation-list">
          {errors.map((error, index) => (
            <li key={index}>
              {error.path ? (
                <button type="button" className="error-path" onClick={() => onField(error.path)}>
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
  const operation = useRef('');
  const fileInput = useRef(null);
  const documentRef = useRef(null);
  const keys = useRef(new WeakMap());
  const nextKey = useRef(0);
  const family = familyForDocument(document) || selectedFamily;
  const currentVersion = family.latestVersion;
  const currentDocumentVersion = schemaVersion(document);
  const profileVersion = family.formVersions.includes(currentDocumentVersion) ? currentDocumentVersion : currentVersion;
  const profileKey = family.id + '@' + profileVersion;
  const model = profiles[profileKey];
  const formReady = Boolean(model && document && model.version === currentDocumentVersion);
  const groupLabels = groupLabelsFor(family.id);
  const drafts = draftsByFamily[family.id] || [];
  const migrationTarget = Object.hasOwn(family.migrationTargets, currentDocumentVersion)
    ? family.migrationTargets[currentDocumentVersion]
    : !currentDocumentVersion && family.id === 'explanatorynote' ? currentVersion : undefined;
  const rawDirty = raw !== rawBase;
  const errorPaths = useMemo(() => new Set((validation?.errors || []).map(error => normalPath(error.path))), [validation]);

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
        if (typeof message.xml === 'string' && message.xml.trim()) {
          try { replaceDocument(message.xml, message.name, message); } catch (error) { setProblem(error.message); }
        } else {
          const requestedFamily = message.family ? familyForId(message.family) : initialFamily;
          api(familyRequest('new', requestedFamily))
            .then(({ xml }) => replaceDocument(xml, message.name || requestedFamily.newTitle, message))
            .catch(error => setProblem(requestMessage(error)));
        }
      } else if (message?.type === 'error') {
        setProblem(message.message || 'Не удалось сохранить документ.');
      } else if (message?.type === 'saved') {
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
    if (readOnly) return;
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
    run(requestedFamily.createProgress, async () => {
      const { xml } = await api(familyRequest('new', requestedFamily));
      replaceDocument(xml, requestedFamily.newTitle);
      setShowDrafts(false);
    });
  }

  function importFile(event) {
    const file = event.target.files?.[0];
    event.target.value = '';
    if (!file) return;
    run('Читаю XML', async () => {
      const xml = await readXmlFile(file);
      replaceDocument(xml, nameFromFile(file.name), { original: { xml, file, name: nameFromFile(file.name) } });
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
          ? 'XML выгружен. Схема и стиль доступны в разделе «Файлы схемы».'
          : 'XML выгружен в исходном формате приложения.');
      } else if (result) {
        setNotice('Исправьте ошибки перед выгрузкой или скачайте черновик.');
      }
    });
  }

  function desktopSave() {
    run('Проверяю перед сохранением', async () => {
      const xml = serializeXml(document);
      const result = await validateSnapshot(xml, revision.current);
      if (result?.valid) {
        host.postMessage({ type: 'save', xml, name: name.trim() || defaultName(document) });
        setNotice('Документ передан на сохранение.');
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
    const draft = {
      id, name: name.trim() || defaultName(document), xml: serializeXml(document), updatedAt: new Date().toISOString(),
      original: original ? { xml: original.xml, name: original.name } : undefined,
      warnings: conversionWarnings,
    };
    const updated = [draft, ...drafts.filter(item => item.id !== id)];
    try {
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

  function navigateError(path) {
    if (!formReady) {
      openXmlView();
      return;
    }
    const normalized = normalPath(path);
    const name = normalized.split('/').filter(Boolean)[1]?.replace(/\[\d+\]/g, '');
    setSection(sectionFor(name, family.id));
    setTab('form');
    requestAnimationFrame(function reveal() {
      let frames = 0;
      function nextFrame() {
        const matches = Array.from(window.document.querySelectorAll('[data-xml-path]'));
        const field = matches.find(node => node.dataset.xmlPath === normalized && node.classList.contains('field'));
        if (field) {
          field.scrollIntoView({ block: 'center' });
          field.querySelector('input,select,textarea')?.focus({ preventScroll: true });
          return;
        }
        const ancestor = matches.find(node => node.tagName === 'DETAILS' && !node.open &&
          (normalized === node.dataset.xmlPath || normalized.startsWith(node.dataset.xmlPath + '/')));
        if (ancestor) ancestor.open = true;
        if (frames++ < 15 && ancestor) requestAnimationFrame(nextFrame);
        else {
          const closest = matches.filter(node => normalized.startsWith(node.dataset.xmlPath))
            .sort((a, b) => b.dataset.xmlPath.length - a.dataset.xmlPath.length)[0];
          closest?.scrollIntoView({ block: 'center' });
        }
      }
      nextFrame();
    });
  }

  const actionsDisabled = Boolean(busy || rawDirty || !document);
  const hasSchemaAssets = family.schemaVersions.includes(currentDocumentVersion);
  const assetVersion = hasSchemaAssets ? currentDocumentVersion : currentVersion;
  const assetBase = '/schemas/' + family.id + '-' + assetVersion.replace('.', '-');
  const context = { types: model?.types || {}, depth: 0, readOnly, lockGuid: Boolean(host),
    idAttribute: family.idAttribute, bridge: host, errorPaths, change, keyFor };

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
              <select className="header-family" aria-label="Вид документа" value={family.id} disabled={Boolean(busy)}
                onChange={event => newDocument(familyForId(event.target.value))}>
                {Object.values(documentFamilies).map(option => (
                  <option key={option.id} value={option.id}>{option.title}</option>
                ))}
              </select>
              <button type="button" className="header-button" disabled={Boolean(busy)} onClick={() => newDocument()}>{family.newAction}</button>
              <button type="button" className="header-button header-primary" disabled={Boolean(busy)}
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
                <button className={showDrafts ? 'selected' : ''} type="button" onClick={() => setShowDrafts(true)}>Черновики <span>{drafts.length}</span></button>
              </div>
            )}
            <h2>{family.sectionsTitle}</h2>
            <nav className="section-nav" aria-label={family.sectionsTitle}>
              {Object.entries(groupLabels).map(([id, label]) => (
                <button type="button" key={id} className={!showDrafts && tab === 'form' && section === id ? 'active' : ''}
                  aria-current={!showDrafts && tab === 'form' && section === id ? 'page' : undefined}
                  disabled={!formReady || rawDirty}
                  onClick={() => { setShowDrafts(false); setSection(id); setTab('form'); }}>
                  <span>{label}</span>
                  {formReady && sectionProgress(model, document, id) > 0 && <span className="nav-dot" aria-hidden="true"></span>}
                </button>
              ))}
            </nav>
            <div className="sidebar-bottom">
              <h2>Файлы схемы</h2>
              <a href={assetBase + '.xsd'} download>Схема XSD {assetVersion}</a>
              <a href={assetBase + '.xsl'} download>Стиль XSL {assetVersion}</a>
            </div>
          </div>
        </aside>

        <main className="main-content">
          {problem && <div className="feedback feedback-error" role="alert">{problem}</div>}
          {notice && <div className="feedback feedback-info" role="status">{notice}</div>}
          {busy && <p className="live-progress" role="status"><span aria-hidden="true"></span>{busy}…</p>}

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
                    <input value={name} onChange={event => {
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
                      try { downloadXml(original.file || serializeXml(parseXml(original.xml)), original.name + ' (исходный)'); }
                      catch (error) { setProblem(error.message); }
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
              <ValidationPanel validation={validation} onField={navigateError} />

              <section className="paper editor">
                <div className="editor-heading">
                  <h2>{tab === 'preview' ? 'Предпросмотр' : tab === 'form' && formReady ? groupLabels[section] : 'Исходный XML'}</h2>
                  <div className="editor-switch" role="group" aria-label="Вид редактора">
                    <button type="button" className={tab === 'form' ? 'selected' : ''} disabled={!formReady || rawDirty}
                      onClick={() => setTab('form')}>Форма</button>
                    <button type="button" className={tab === 'xml' || tab !== 'preview' && !formReady ? 'selected' : ''} onClick={openXmlView}>XML</button>
                    <button type="button" className={tab === 'preview' ? 'selected' : ''} disabled={actionsDisabled || !hasSchemaAssets}
                      onClick={showPreview}>Предпросмотр</button>
                  </div>
                </div>
                {tab === 'preview' ? (
                  <iframe className="print-preview" title={family.previewTitle} sandbox="" referrerPolicy="no-referrer"
                    srcDoc={sandboxedPreview(preview)} />
                ) : tab === 'form' && formReady ? (
                  <SchemaForm key={keyFor(document.documentElement)} model={model} document={document} section={section} context={context} />
                ) : (
                  <div className="raw-panel">
                    {!family.formVersions.includes(currentDocumentVersion) && (
                      <p className="feedback feedback-info">
                        Форма доступна для {family.formVersions.join(' и ')}. Этот документ можно открыть как XML
                        {migrationTarget && ' и преобразовать в ' + migrationTarget}.
                      </p>
                    )}
                    <textarea className="raw-editor" aria-label="Содержимое XML" value={raw} spellCheck={false} readOnly={readOnly}
                      onChange={event => { setRaw(event.target.value); setMigration(null); revision.current++; }} />
                    {rawDirty && !readOnly && (
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
