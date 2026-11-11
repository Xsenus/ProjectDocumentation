import { useRef, useState } from 'react';
import { documentFamilies } from './documentFamilies.js';

const operationLabels = { import: 'Импорт', export: 'Выгрузка' };
const exportLabels = { checked: 'Проверенный XML', draft: 'Черновик XML', original: 'Исходный XML' };
const outcomes = {
  started: 'Начато',
  opened: 'Открыт',
  warnings: 'Открыт с замечаниями',
  unchecked: 'Открыт без проверки',
  download: 'Скачивание запущено',
  blocked: 'Выгрузка остановлена',
  failed: 'Ошибка',
  cancelled: 'Отменено',
};
const dateFormat = new Intl.DateTimeFormat('ru-RU', { dateStyle: 'short', timeStyle: 'medium' });
const pageSize = 30;

function outcomeClass(outcome) {
  if (outcome === 'opened' || outcome === 'download') return 'history-ok';
  if (outcome === 'failed' || outcome === 'blocked') return 'history-error';
  if (outcome === 'warnings' || outcome === 'unchecked') return 'history-warning';
  return 'history-neutral';
}

function FileOperationDetails({ entry, onReopen, busy, panelRef }) {
  const [copyMessage, setCopyMessage] = useState('');
  const diagnostics = entry.diagnostics || [];
  async function copyDiagnostics() {
    const lines = diagnostics.map(issue =>
      [issue.path, issue.line ? 'строка ' + issue.line : null, issue.message].filter(Boolean).join(' — '));
    try {
      await navigator.clipboard.writeText([entry.fileName, ...lines].join('\n'));
      setCopyMessage('Замечания скопированы.');
    } catch {
      setCopyMessage('Выделите текст замечаний и скопируйте его вручную.');
    }
  }
  return (
    <section className="history-details" aria-label="Сведения об операции" ref={panelRef} tabIndex={-1}>
      <div className="history-detail-head">
        <h2>{entry.fileName}</h2>
        <span className={'history-status ' + outcomeClass(entry.outcome)}>{outcomes[entry.outcome]}</span>
      </div>
      <dl className="history-meta">
        <div><dt>Дата и время</dt><dd>{dateFormat.format(entry.createdAt)}</dd></div>
        <div><dt>Операция</dt><dd>{operationLabels[entry.operation]}{entry.variant && ' · ' + exportLabels[entry.variant]}</dd></div>
        {entry.family && <div><dt>Документ</dt><dd>{documentFamilies[entry.family]?.title || entry.family}</dd></div>}
        {entry.version && <div><dt>Версия схемы</dt><dd>{entry.version}</dd></div>}
      </dl>
      <p className="history-message">{entry.message}</p>
      {diagnostics.length > 0 && (
        <div className="history-diagnostics">
          <div className="history-detail-head">
            <h3>Замечания ({diagnostics.length})</h3>
            <button className="text-button" type="button" onClick={copyDiagnostics}>Копировать замечания</button>
          </div>
          {copyMessage && <p className="muted" role="status">{copyMessage}</p>}
          <ul>
            {diagnostics.map((issue, index) => <li key={index}>
              <div><code>{issue.path}</code>{issue.line && <small>Строка {issue.line}</small>}</div>
              <p>{issue.message}</p>
            </li>)}
          </ul>
        </div>
      )}
      {entry.operation === 'import' && <button className="button button-quiet" type="button" disabled={busy}
        onClick={onReopen}>Выбрать XML заново</button>}
    </section>
  );
}

export default function FileHistory({ entries, onReopen, busy }) {
  const [query, setQuery] = useState('');
  const [operation, setOperation] = useState('');
  const [outcome, setOutcome] = useState('');
  const [selected, setSelected] = useState('');
  const [visibleCount, setVisibleCount] = useState(pageSize);
  const details = useRef(null);
  const filter = query.trim().toLocaleLowerCase('ru-RU');
  const matches = entries.filter(entry => (!operation || entry.operation === operation) &&
    (!outcome || entry.outcome === outcome) && entry.fileName.toLocaleLowerCase('ru-RU').includes(filter));
  const visible = matches.slice(0, visibleCount);
  const active = visible.find(entry => entry.id === selected) || visible[0];

  function selectEntry(entry) {
    setSelected(entry.id);
    if (window.matchMedia('(max-width: 530px)').matches) requestAnimationFrame(() => {
      details.current?.focus({ preventScroll: true });
      details.current?.scrollIntoView({ block: 'start' });
    });
  }

  return (
    <section className="paper library history">
      <h1>История файлов</h1>
      <div className="history-filters">
        <label><span>Имя файла</span><input type="search" value={query} placeholder="Найти файл"
          onChange={event => { setQuery(event.target.value); setVisibleCount(pageSize); }} /></label>
        <label><span>Операция</span><select value={operation}
          onChange={event => { setOperation(event.target.value); setVisibleCount(pageSize); }}>
          <option value="">Все операции</option>
          {Object.entries(operationLabels).map(([value, label]) => <option key={value} value={value}>{label}</option>)}
        </select></label>
        <label><span>Результат</span><select value={outcome}
          onChange={event => { setOutcome(event.target.value); setVisibleCount(pageSize); }}>
          <option value="">Все результаты</option>
          {Object.entries(outcomes).map(([value, label]) => <option key={value} value={value}>{label}</option>)}
        </select></label>
      </div>
      {visible.length ? <div className="history-workspace">
        <div className="history-list-panel">
          <ul className="history-list" aria-label="Операции с файлами">
            {visible.map(entry => <li key={entry.id}>
              <button type="button" aria-pressed={entry === active} onClick={() => selectEntry(entry)}>
                <span className="history-row-meta">{operationLabels[entry.operation]} · {dateFormat.format(entry.createdAt)}</span>
                <strong>{entry.fileName}</strong>
                <span className={'history-status ' + outcomeClass(entry.outcome)}>{outcomes[entry.outcome]}</span>
              </button>
            </li>)}
          </ul>
          {matches.length > visibleCount && <button className="text-button" type="button"
            onClick={() => setVisibleCount(count => count + pageSize)}>Показать ещё</button>}
        </div>
        <FileOperationDetails key={active.id} entry={active} onReopen={onReopen} busy={busy} panelRef={details} />
      </div> : <p className="empty-state">{entries.length ? 'По выбранным условиям операций нет.' : 'Операций с XML пока нет.'}</p>}
    </section>
  );
}
