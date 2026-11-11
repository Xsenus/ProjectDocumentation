import { useId, useState } from 'react';
import DesktopLookup from './DesktopLookup.jsx';
import { familyForDocument } from './documentFamilies.js';
import { addBranch, directElements, insertElement, knownAttributeNames, leafNames, pathFor, readElementText, writeElementText } from './xml.js';

const conclusionGroupLabels = {
  details: 'Реквизиты',
  object: 'Объект экспертизы',
  people: 'Участники',
  documents: 'Документы',
  finance: 'Финансирование',
  results: 'Выводы и эксперты',
};

const conclusionFields = {
  details: ['ExpertOrganization', 'Approver', 'ExaminationObject'],
  object: ['Object', 'EcologyExpertise', 'CadastralNumber', 'ClimateConditions', 'ClimateConditionsNote', 'EngineeringSurveyAddress'],
  people: ['Declarant', 'ProjectDocumentsDeveloper', 'ProjectDocumentsTechnicalCustomer', 'ProjectDocumentsOtherOrganization',
    'Designer', 'EngineeringSurveyDeveloper', 'EngineeringSurveyTechnicalCustomer', 'EngineeringSurveyOtherOrganization'],
  documents: ['Documents', 'PreviousConclusions', 'PreviousSimpleConclusions', 'EEPDUse'],
  finance: ['Finance', 'FinanceComment', 'EstimatedCost'],
  results: ['ExpertEngineeringSurveys', 'ExpertProjectDocuments', 'ExpertEstimate', 'Summary', 'Experts'],
};

const noteGroupLabels = {
  details: 'Реквизиты',
  people: 'Авторы и участники',
  object: 'Объект строительства',
  documents: 'Документы и нормы',
  finance: 'Финансирование',
  results: 'Пояснения и заверение',
};

const noteFields = {
  details: ['ExplanatoryNoteNumber', 'ExplanatoryNoteYear', 'ExplanatoryNoteModifications'],
  people: ['IssueAuthor', 'ProjectDocumentationAuthors', 'Signers', 'Developer', 'TechnicalCustomer'],
  object: ['NonIndustrialObject', 'IndustrialObject', 'LinearObject'],
  documents: ['UsedNorms', 'ProjectDecisionDocuments', 'ProjectInitialDocuments', 'EngineeringSurveyDocuments'],
  finance: ['FinanceSources'],
  results: ['UsedAbbreviations', 'AdditionalData', 'DesignerNote', 'DesignerAssurance'],
};

const labels = {
  ExplanatoryNoteNumber: 'Номер пояснительной записки',
  ExplanatoryNoteYear: 'Год подготовки',
  ExplanatoryNoteModifications: 'Изменения записки',
  IssueAuthor: 'Лицо, подготовившее документацию',
  ProjectDocumentationAuthors: 'Авторы проектной документации',
  Signers: 'Подписанты',
  Developer: 'Застройщик',
  TechnicalCustomer: 'Технический заказчик',
  UsedAbbreviations: 'Сокращения',
  UsedNorms: 'Нормативные документы',
  ProjectDecisionDocuments: 'Документы для подготовки проектных решений',
  ProjectInitialDocuments: 'Исходные документы',
  EngineeringSurveyDocuments: 'Документы инженерных изысканий',
  AdditionalData: 'Дополнительные сведения',
  FinanceSources: 'Источники финансирования',
  NonIndustrialObject: 'Объект непроизводственного назначения',
  IndustrialObject: 'Объект производственного назначения',
  LinearObject: 'Линейный объект',
  DesignerNote: 'Пояснения проектировщика',
  DesignerAssurance: 'Заверение проектировщика',
  id: 'Идентификатор записки',
  SchemaVersion: 'Версия схемы',
  ConclusionGUID: 'Идентификатор заключения',
  ExpertOrganization: 'Экспертная организация',
  Approver: 'Утверждающее лицо',
  ExaminationObject: 'Параметры экспертизы',
  Object: 'Описание объекта',
  EcologyExpertise: 'Экологическая экспертиза',
  CadastralNumber: 'Кадастровый номер',
  ClimateConditions: 'Природные условия',
  ClimateConditionsNote: 'Примечание к природным условиям',
  EngineeringSurveyAddress: 'Местоположение изысканий',
  Declarant: 'Заявитель',
  ProjectDocumentsDeveloper: 'Застройщик проектной документации',
  ProjectDocumentsTechnicalCustomer: 'Технический заказчик проектной документации',
  ProjectDocumentsOtherOrganization: 'Другая организация проектной документации',
  Designer: 'Проектировщик',
  EngineeringSurveyDeveloper: 'Застройщик по изысканиям',
  EngineeringSurveyTechnicalCustomer: 'Технический заказчик по изысканиям',
  EngineeringSurveyOtherOrganization: 'Другая организация по изысканиям',
  Documents: 'Поданные документы',
  PreviousConclusions: 'Предыдущие заключения',
  PreviousSimpleConclusions: 'Предыдущие упрощённые заключения',
  EEPDUse: 'Сведения об использовании ЕЭПД',
  Finance: 'Источник финансирования',
  FinanceComment: 'Примечание о финансировании',
  EstimatedCost: 'Сметная стоимость',
  ExpertEngineeringSurveys: 'Заключения по инженерным изысканиям',
  ExpertProjectDocuments: 'Заключения по проектной документации',
  ExpertEstimate: 'Сметная экспертиза',
  Summary: 'Итоговые выводы',
  Experts: 'Эксперты',
};

export function groupLabelsFor(familyId) {
  return familyId === 'explanatorynote' ? noteGroupLabels : conclusionGroupLabels;
}

export function sectionFor(name, familyId) {
  const fields = familyId === 'explanatorynote' ? noteFields : conclusionFields;
  return Object.keys(fields).find(group => fields[group].includes(name)) || 'details';
}

export function titleFor(spec, types) {
  return labels[spec.name] || spec.documentation || types[spec.type]?.documentation || spec.name || 'Вариант';
}

export function normalPath(path) {
  return (path || '').replace(/\[1\]/g, '').replace(/\{[^}]*\}/g, '');
}

export function sectionProgress(model, document, section) {
  const root = model.types[model.root.type];
  const familyId = familyForDocument(document)?.id;
  const particles = topParticles(root?.particle).filter(part => sectionFor([...leafNames(part)][0], familyId) === section);
  return particles.reduce((total, part) => total + directElements(document.documentElement, leafNames(part)).length, 0);
}

function topParticles(particle) {
  if (!particle) return [];
  return particle.kind === 'sequence' || particle.kind === 'all' ? particle.children || [] : [particle];
}

function primitive(typeName, types, seen = new Set()) {
  if (seen.has(typeName)) return typeName;
  seen.add(typeName);
  const next = types[typeName]?.base;
  return next && next !== typeName ? primitive(next, types, seen) : typeName?.split(/[:}]/).pop();
}

function choicesFor(spec, types) {
  const entries = types[spec.type]?.enumeration;
  if (entries?.length) return entries;
  if (primitive(spec.type, types) === 'boolean') {
    return [{ value: 'true', label: 'Да' }, { value: 'false', label: 'Нет' }];
  }
  return [];
}

function Field({ spec, types, value, onChange, readOnly, path, errorPaths, onRemove, occurrence }) {
  const id = useId();
  const title = titleFor(spec, types);
  const fieldTitle = occurrence ? title + ' ' + occurrence : title;
  const doc = spec.documentation && spec.documentation !== title ? spec.documentation : undefined;
  const fixed = spec.fixed !== undefined && spec.fixed !== null;
  const invalid = errorPaths.has(normalPath(path));
  const blank = value == null ? '' : String(value);
  const inputType = primitive(spec.type, types);
  const choices = choicesFor(spec, types);
  const trueFalse = inputType === 'boolean' && (blank === '0' || blank === '1');
  const selected = trueFalse ? (blank === '0' ? 'false' : 'true') : blank;
  const common = { id, value: blank, 'aria-invalid': invalid || undefined,
    'aria-describedby': doc ? id + '-hint' : undefined, 'aria-required': Boolean(spec.required), readOnly: readOnly || fixed,
    onChange: event => onChange(event.target.value) };
  const multiline = spec.name === 'Text' || inputType === 'base64Binary' ||
    spec.type?.match(/(?:1000|2000|4000|8000|Text|Description)/i) || blank.includes('\n') || blank.length > 150;
  let control;
  if (choices.length && !readOnly && !fixed) {
    control = (
      <select {...common} readOnly={undefined} value={selected}>
        <option value="">Выберите значение</option>
        {selected && !choices.some(item => String(item.value) === selected) && <option value={selected}>{selected}</option>}
        {choices.map(item => <option key={item.value} value={item.value}>{item.label || item.value}</option>)}
      </select>
    );
  } else if (multiline && !choices.length) {
    control = <textarea {...common} rows={3} spellCheck={false} />;
  } else {
    const display = readOnly && choices.length ? choices.find(item => String(item.value) === selected)?.label || blank : blank;
    const type = inputType === 'date' && (!blank || /^\d{4}-\d{2}-\d{2}$/.test(blank)) ? 'date' : 'text';
    const inputMode = inputType === 'decimal' || inputType === 'float' || inputType === 'double' ? 'decimal'
      : /integer|^int$|^long$|^short$/i.test(inputType || '') ? 'numeric' : undefined;
    control = <input {...common} type={type} inputMode={inputMode} value={display} spellCheck={false} />;
  }
  return (
    <div className={'field' + (multiline ? ' field-wide' : '')} data-xml-path={normalPath(path)}>
      <div className="field-caption">
        <label htmlFor={id}>{fieldTitle}{spec.required && <span className="required" aria-hidden="true"> *</span>}</label>
        {onRemove && !readOnly && (
          <button className="text-button field-remove" type="button" onClick={onRemove} aria-label={'Убрать ' + fieldTitle}>Убрать</button>
        )}
      </div>
      {control}
      {doc && <small id={id + '-hint'} className="field-hint">{doc}</small>}
      {fixed && blank !== String(spec.fixed) && !readOnly && (
        <button type="button" className="text-button" onClick={() => onChange(String(spec.fixed))}>Установить {spec.fixed}</button>
      )}
    </div>
  );
}

function Attributes({ element, type, context }) {
  const attributes = type.attributes || [];
  const extra = knownAttributeNames(element, attributes);
  if (!attributes.length && !extra.length) return null;
  return (
    <div className="field-grid attributes">
      {attributes.map(spec => (
        <Field key={spec.name} spec={spec} types={context.types} value={element.getAttribute(spec.name)}
          readOnly={context.readOnly || (context.lockGuid && !element.parentElement && spec.name === context.idAttribute)}
          path={pathFor(element) + '/@' + spec.name} errorPaths={context.errorPaths}
          onChange={value => context.change(() => {
            if (value === '' && !spec.required) element.removeAttribute(spec.name);
            else element.setAttribute(spec.name, value);
          })} />
      ))}
      {extra.length > 0 && <p className="preserved field-wide">Дополнительные атрибуты: {extra.join(', ')}. Доступны во вкладке XML.</p>}
    </div>
  );
}

function ComplexContent({ element, type, context }) {
  const names = leafNames(type.particle);
  const unknown = Array.from(element.children).filter(child => !names.has(child.localName));
  const valueType = type.textType;
  return (
    <>
      <DesktopLookup element={element} type={type} context={context} />
      <Attributes element={element} type={type} context={context} />
      {valueType && (
        <Field spec={{ name: 'Значение', type: valueType, required: true }} types={context.types} value={readElementText(element)}
          readOnly={context.readOnly} path={pathFor(element)} errorPaths={context.errorPaths}
          onChange={value => context.change(() => writeElementText(element, value))} />
      )}
      {type.particle && (
        <ParticleEditor parent={element} spec={type.particle} parentParticle={type.particle} context={context} />
      )}
      {unknown.length > 0 && (
        <p className="preserved">Дополнительные элементы: {[...new Set(unknown.map(node => node.localName))].join(', ')}. Доступны во вкладке XML.</p>
      )}
    </>
  );
}

function ComplexInstance({ element, spec, type, index, repeatable, removable, context }) {
  const [open, setOpen] = useState(index === 0 && (context.depth <= 0 || context.readOnly));
  const title = titleFor(spec, context.types);
  const example = Array.from(element.children).find(child => /^(Name|FullName|ObjectName|DocName|Surname|NameProject|Number)$/i.test(child.localName))?.textContent;
  return (
    <details open={open} onToggle={event => setOpen(event.currentTarget.open)} className="complex-card"
      data-xml-path={normalPath(pathFor(element))}>
      <summary>
        <span className="complex-heading">{title}{repeatable ? ' ' + (index + 1) : ''}{spec.min > 0 && <span className="required"> *</span>}</span>
        {example && <small>{example}</small>}
      </summary>
      {open && (
        <div className="complex-content">
          <ComplexContent element={element} type={type} context={{ ...context, depth: context.depth + 1 }} />
          {removable && !context.readOnly && (
            <button type="button" className="button button-danger"
              onClick={() => context.change(() => element.remove())}>Удалить запись</button>
          )}
        </div>
      )}
    </details>
  );
}

function ElementEditor({ parent, spec, parentParticle, context }) {
  const type = context.types[spec.type];
  const isComplex = type?.kind === 'complex';
  const nodes = directElements(parent, new Set([spec.name]));
  const repeatable = spec.max === null || spec.max > 1;
  const canAdd = spec.max === null || nodes.length < (spec.max ?? 1);
  const title = titleFor(spec, context.types);
  const fieldSpec = { ...spec, required: spec.min > 0 };
  const add = () => context.change(() => insertElement(parent, spec, parentParticle, context.types));
  const updateSimple = (element, value) => {
    if (!element && value === '') return;
    context.change(() => {
      const target = element || insertElement(parent, spec, parentParticle, context.types);
      writeElementText(target, value);
    });
  };

  if (!isComplex && !repeatable && nodes.length <= 1) {
    const node = nodes[0];
    if (!node && context.readOnly) return null;
    const path = node ? pathFor(node) : pathFor(parent) + '/' + spec.name;
    return (
      <Field spec={fieldSpec} types={context.types} value={readElementText(node)}
        readOnly={context.readOnly} path={path} errorPaths={context.errorPaths}
        onChange={value => updateSimple(node, value)}
        onRemove={node && spec.min === 0 ? () => context.change(() => node.remove()) : undefined} />
    );
  }

  if (!nodes.length && context.readOnly) return null;
  return (
    <div className="element-group field-wide">
      {nodes.map((element, index) => isComplex
        ? <ComplexInstance key={context.keyFor(element)} element={element} spec={spec} type={type} index={index}
            repeatable={repeatable || nodes.length > 1} removable={nodes.length > spec.min} context={context} />
        : <Field key={context.keyFor(element)} spec={{ ...fieldSpec, name: title + ' ' + (index + 1) }} types={context.types}
            value={readElementText(element)} readOnly={context.readOnly} path={pathFor(element)} errorPaths={context.errorPaths}
            onChange={value => updateSimple(element, value)}
            onRemove={nodes.length > spec.min ? () => context.change(() => element.remove()) : undefined} />
      )}
      {!context.readOnly && canAdd && (
        <button type="button" className="button button-add" aria-label={'Добавить ' + title} onClick={add}>
          <span aria-hidden="true">+</span> {nodes.length || repeatable ? 'Добавить' : 'Заполнить'} {title}
          {!nodes.length && spec.min > 0 && <span className="required"> *</span>}
        </button>
      )}
    </div>
  );
}

function branchLabel(spec, types, index) {
  if (spec.kind === 'element') return titleFor(spec, types);
  if (spec.documentation) return spec.documentation;
  const first = [...leafNames(spec)].slice(0, 3);
  return first.length ? first.map(name => labels[name] || name).join(' / ') : 'Вариант ' + (index + 1);
}

function OrderedChoiceEntries({ parent, branches, spec, context }) {
  const byName = new Map(branches.map(branch => [branch.name, branch]));
  const entries = Array.from(parent.children).filter(element => byName.has(element.localName));
  const removable = entries.length > spec.min;

  return entries.map((element, index) => {
    const branch = byName.get(element.localName);
    const type = context.types[branch.type];
    const title = titleFor(branch, context.types);
    return (
      <div className="choice-selection" key={context.keyFor(element)}>
        {type?.kind === 'complex'
          ? <ComplexInstance element={element} spec={{ ...branch, min: 0 }} type={type} index={index}
              repeatable removable={removable} context={context} />
          : <Field spec={branch} occurrence={index + 1} types={context.types}
              value={readElementText(element)} readOnly={context.readOnly} path={pathFor(element)}
              errorPaths={context.errorPaths} onChange={value => context.change(() => writeElementText(element, value))}
              onRemove={removable ? () => context.change(() => element.remove()) : undefined} />}
        {!context.readOnly && entries.length > 1 && (
          <div className="inline-actions">
            <button className="text-button" type="button" disabled={index === 0} aria-label={'Переместить вверх ' + title + ' ' + (index + 1)}
              onClick={() => context.change(() => parent.insertBefore(element, entries[index - 1]))}>Вверх</button>
            <button className="text-button" type="button" disabled={index === entries.length - 1} aria-label={'Переместить вниз ' + title + ' ' + (index + 1)}
              onClick={() => context.change(() => parent.insertBefore(element, entries[index + 1].nextSibling))}>Вниз</button>
          </div>
        )}
      </div>
    );
  });
}

function ChoiceEditor({ parent, spec, parentParticle, context }) {
  const [selection, setSelection] = useState('');
  const [clearing, setClearing] = useState(false);
  const id = useId();
  const branches = spec.children || [];
  const present = branches.map(branch => directElements(parent, leafNames(branch)));
  const multiple = spec.max === null || spec.max > 1;
  const ordered = multiple && branches.every(branch => branch.kind === 'element');
  const count = present.reduce((total, matches, index) => total +
    (multiple && branches[index].kind === 'element' ? matches.length : Number(matches.length > 0)), 0);
  const selected = selection === '' ? null : Number(selection);
  const showOptions = !context.readOnly && (spec.max === null || count < (spec.max ?? 1));
  const activeBranches = branches.map((_, index) => index).filter(index => present[index].length > 0 ||
    (!multiple && index === selected && count === 0 && branches[index].kind !== 'element'));
  function addSelected() {
    if (selected === null) return;
    context.change(() => addBranch(parent, branches[selected], parentParticle, context.types));
    if (multiple || branches[selected].kind === 'element') setSelection('');
  }
  if (context.readOnly && !count) return null;
  return (
    <div className="choice-block field-wide">
      {ordered ? <OrderedChoiceEntries parent={parent} branches={branches} spec={spec} context={context} /> : activeBranches.map(index => {
        const branch = branches[index];
        const activeSpec = branch.kind === 'element' && multiple ? { ...branch, min: 0, max: spec.max } : branch;
        return (
          <div className="choice-selection" key={index}>
            {branch.kind !== 'element' && <span className="subheading">{branchLabel(branch, context.types, index)}</span>}
            <ParticleEditor parent={parent} spec={activeSpec} parentParticle={parentParticle} context={context} />
          </div>
        );
      })}
      {showOptions && (
        <div className="choice-picker">
          <label htmlFor={id}>{spec.documentation || 'Добавить вариант'}{count < spec.min && <span className="required"> *</span>}</label>
          <select id={id} value={selection} onChange={event => setSelection(event.target.value)}>
            <option value="">Выберите вариант</option>
            {branches.map((branch, index) => <option key={index} value={index}>{branchLabel(branch, context.types, index)}</option>)}
          </select>
          <button type="button" className="button button-quiet" disabled={selected === null} onClick={addSelected}>Добавить</button>
        </div>
      )}
      {!context.readOnly && !multiple && count > 0 && (
        clearing ? (
          <div className="choice-confirm">
            <span>Удалить текущий вариант и его данные?</span>
            <button type="button" className="button button-danger" onClick={() => {
              context.change(() => new Set(present.flat()).forEach(node => node.remove()));
              setSelection('');
              setClearing(false);
            }}>Удалить</button>
            <button type="button" className="button button-quiet" onClick={() => setClearing(false)}>Отмена</button>
          </div>
        ) : <button type="button" className="text-button" onClick={() => setClearing(true)}>Сменить вариант</button>
      )}
    </div>
  );
}

function ParticleEditor({ parent, spec, parentParticle, context }) {
  if (spec.kind === 'element') return <ElementEditor parent={parent} spec={spec} parentParticle={parentParticle} context={context} />;
  if (spec.kind === 'choice') return <ChoiceEditor parent={parent} spec={spec} parentParticle={parentParticle} context={context} />;
  return (
    <div className="field-grid field-wide">
      {(spec.children || []).map((child, index) => (
        <ParticleEditor key={index} parent={parent} spec={child} parentParticle={parentParticle} context={context} />
      ))}
    </div>
  );
}

export default function SchemaForm({ model, document, section, context }) {
  const element = document.documentElement;
  const type = model.types[model.root.type];
  if (!type) return <p className="empty-state">В схеме отсутствует модель корневого элемента.</p>;
  const familyId = familyForDocument(document)?.id;
  const particles = topParticles(type.particle).filter(part => sectionFor([...leafNames(part)][0], familyId) === section);
  const unknown = Array.from(element.children).filter(child => !leafNames(type.particle).has(child.localName));
  return (
    <div className="schema-form">
      {section === 'details' && <Attributes element={element} type={type} context={context} />}
      <div className="field-grid">
        {particles.map((part, index) => (
          <ParticleEditor key={section + '-' + index} parent={element} spec={part} parentParticle={type.particle} context={context} />
        ))}
      </div>
      {section === 'details' && unknown.length > 0 && <p className="preserved">В исходном XML сохранены дополнительные элементы: {[...new Set(unknown.map(node => node.localName))].join(', ')}.</p>}
    </div>
  );
}
