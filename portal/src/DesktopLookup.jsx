import { useEffect, useId, useRef, useState } from 'react';
import { directElements, insertElement, leafNames, readElementText, writeElementText } from './xml.js';

const organizationFields = new Set(['FullName', 'AbbreviatedName', 'OGRN', 'INN', 'KPP']);
const addressFields = new Set(['RegionCode', 'PostIndex', 'OKTMOCode', 'District', 'City', 'Settlement', 'Street', 'Building', 'Room', 'Note']);
const addressQueryFields = ['District', 'City', 'Settlement', 'Street', 'Building', 'Room'];

function fieldNode(parent, name) {
  return directElements(parent, new Set([name]))[0];
}

function queryFrom(parent, organization) {
  if (organization) return readElementText(fieldNode(parent, 'INN'));
  return readElementText(fieldNode(parent, 'Note')) ||
    addressQueryFields.map(name => readElementText(fieldNode(parent, name))).filter(Boolean).join(', ');
}

function candidates(parent, particle, fields, allowed, types) {
  if (!particle) return [];
  if (particle.kind === 'element') {
    const value = fields[particle.name];
    if (!allowed.has(particle.name) || typeof value !== 'string' || !value.trim()) return [];
    const matches = directElements(parent, new Set([particle.name]));
    if (matches.length > 1 || matches.length === 1 && readElementText(matches[0]).trim()) return [];
    const enumeration = types[particle.type]?.enumeration;
    if (enumeration?.length && !enumeration.some(item => String(item.value) === value)) return [];
    return [{ spec: particle, element: matches[0], value }];
  }
  const children = particle.children || [];
  if (particle.kind === 'choice' && (particle.max ?? 1) === 1 && particle.max !== null) {
    const active = children.filter(child => directElements(parent, leafNames(child)).length > 0);
    if (active.length > 1) return [];
    if (active.length === 1) return candidates(parent, active[0], fields, allowed, types);
    return children.map(child => candidates(parent, child, fields, allowed, types)).find(values => values.length) || [];
  }
  return children.flatMap(child => candidates(parent, child, fields, allowed, types));
}

export default function DesktopLookup({ element, type, context }) {
  const names = leafNames(type.particle);
  const organization = ['FullName', 'OGRN', 'INN'].every(name => names.has(name));
  const address = ['RegionCode', 'OKTMOCode', 'Street', 'Note'].every(name => names.has(name));
  const bridge = context.bridge;
  const id = useId();
  const [query, setQuery] = useState('');
  const [expanded, setExpanded] = useState(false);
  const [fields, setFields] = useState(null);
  const [error, setError] = useState('');
  const [pending, setPending] = useState(false);
  const request = useRef(null);
  const timeout = useRef(null);

  useEffect(() => {
    if (!bridge || context.readOnly || !organization && !address || !expanded) return undefined;
    const receive = ({ data }) => {
      let message = data;
      if (typeof data === 'string') {
        try { message = JSON.parse(data); } catch { return; }
      }
      if (message?.type !== 'lookup-result' || !request.current || message.requestId !== request.current) return;
      clearTimeout(timeout.current);
      request.current = null;
      setPending(false);
      if (message.error) setError(message.error);
      else setFields(message.fields || {});
    };
    bridge.addEventListener('message', receive);
    return () => {
      clearTimeout(timeout.current);
      request.current = null;
      bridge.removeEventListener('message', receive);
    };
  }, [bridge, context.readOnly, organization, address, expanded]);

  if (!bridge || context.readOnly || !organization && !address) return null;
  const allowed = organization ? organizationFields : addressFields;
  const value = query.trim();
  const savedInn = organization ? queryFrom(element, true).trim() : '';
  const savedOgrn = organization ? readElementText(fieldNode(element, 'OGRN')).trim() : '';
  const queryMismatch = Boolean(savedInn && savedInn !== value);
  const resultMismatch = Boolean(organization && fields && (
    fields.INN !== value || savedOgrn && fields.OGRN && fields.OGRN !== savedOgrn
  ));
  const available = fields && !queryMismatch && !resultMismatch
    ? candidates(element, type.particle, fields, allowed, context.types) : [];
  const valid = organization ? /^\d{10}$/.test(value) && !queryMismatch : Boolean(value);
  const button = organization ? 'Найти организацию' : 'Нормализовать адрес';

  function lookup() {
    if (!valid || request.current) return;
    const requestId = crypto.randomUUID();
    request.current = requestId;
    setPending(true);
    setFields(null);
    setError('');
    timeout.current = setTimeout(() => {
      request.current = null;
      setPending(false);
      setError('DaData не ответила. Попробуйте ещё раз.');
    }, 30000);
    bridge.postMessage(organization
      ? { type: 'lookup-organization', requestId, inn: value }
      : { type: 'normalize-address', requestId, query: value });
  }

  function apply() {
    if (!available.length) return;
    context.change(() => {
      for (const { spec, element: existing, value: fieldValue } of available) {
        const target = existing || insertElement(element, spec, type.particle, context.types);
        writeElementText(target, fieldValue);
      }
    });
    setFields(null);
    setExpanded(false);
  }

  if (!expanded) return (
    <button type="button" className="text-button lookup-open" onClick={() => {
      setExpanded(true);
      setQuery(queryFrom(element, organization));
    }}>{button} через DaData</button>
  );

  return (
    <div className="lookup-panel">
      <div className="lookup-query">
        <label htmlFor={id}>{organization ? 'ИНН организации' : 'Адрес для нормализации'}</label>
        <input id={id} inputMode={organization ? 'numeric' : undefined} value={query}
          onChange={event => { setQuery(event.target.value); setFields(null); setError(''); }} readOnly={pending} />
        <button type="button" className="button button-quiet" disabled={!valid || pending} onClick={lookup}>
          {pending ? 'Запрашиваю…' : button}
        </button>
      </div>
      {queryMismatch && <p className="lookup-error" role="alert">Поиск доступен по ИНН, указанному в документе.</p>}
      {resultMismatch && !queryMismatch && <p className="lookup-error" role="alert">Реквизиты организации в ответе DaData не совпадают с документом.</p>}
      {error && <p className="lookup-error" role="alert">{error}</p>}
      {fields && !queryMismatch && !resultMismatch && (
        <div className="lookup-results" aria-live="polite">
          {available.length ? (
            <>
              <dl>{available.map(({ spec, value: fieldValue }) => (
                <div key={spec.name}><dt>{spec.documentation || spec.name}</dt><dd>{fieldValue}</dd></div>
              ))}</dl>
              <button type="button" className="button button-primary" onClick={apply}>Заполнить пустые поля</button>
            </>
          ) : <p>Подходящих пустых полей нет.</p>}
        </div>
      )}
      <button type="button" className="text-button" disabled={pending}
        onClick={() => { setExpanded(false); setFields(null); setError(''); }}>Свернуть</button>
    </div>
  );
}
