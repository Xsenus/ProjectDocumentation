import { useId, useState } from 'react';
import DesktopLookup from './DesktopLookup.jsx';
import { fileFields, FileMetadataPicker, ImagePicker } from './LocalFilePicker.jsx';
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
  align: 'Выравнивание',
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
  return (path || '').replace(/\{[^}]*\}/g, '');
}

export function formFieldIndex(model, familyId) {
  const result = [];
  const seenPaths = new Set();
  const sections = groupLabelsFor(familyId);
  function add(spec, path, section) {
    if (seenPaths.has(path)) return;
    seenPaths.add(path);
    result.push({ path, title: titleFor(spec, model.types), section: sections[section], documentation: spec.documentation });
  }
  function visitType(typeName, path, section, ancestors) {
    if (ancestors.has(typeName)) return;
    const type = model.types[typeName];
    for (const attribute of type?.attributes || []) add(attribute, `${path}/@${attribute.name}`, section);
    if (type?.particle) visitParticle(type.particle, path, section, new Set(ancestors).add(typeName));
  }
  function visitParticle(particle, path, section, ancestors) {
    if (particle.kind === 'element') {
      const elementPath = `${path}/${particle.name}`;
      add(particle, elementPath, section);
      visitType(particle.type, elementPath, section, ancestors);
      return;
    }
    for (const child of particle.children || []) visitParticle(child, path, section, ancestors);
  }
  const rootType = model.types[model.root.type];
  const rootPath = '/' + model.root.name;
  for (const attribute of rootType?.attributes || []) add(attribute, `${rootPath}/@${attribute.name}`, 'details');
  for (const part of topParticles(rootType?.particle)) {
    visitParticle(part, rootPath, sectionFor([...leafNames(part)][0], familyId), new Set([model.root.type]));
  }
  return result;
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
  if (spec.type === 'tAlignTypes' && entries?.length) {
    const alignments = { left: 'Слева', right: 'Справа', center: 'По центру' };
    return entries.map(entry => ({ ...entry, label: entry.label || alignments[entry.value] }));
  }
  if (entries?.length) return entries;
  if (primitive(spec.type, types) === 'boolean') {
    return [{ value: 'true', label: 'Да' }, { value: 'false', label: 'Нет' }];
  }
  return [];
}

function dateInputValue(value) {
  if (!/^(?!0000)\d{4}-\d{2}-\d{2}$/.test(value)) return false;
  const date = new Date(value + 'T00:00:00Z');
  return Number.isFinite(date.getTime()) && date.toISOString().slice(0, 10) === value;
}

function Field({ spec, types, value, onChange, readOnly, path, fieldErrors, onRemove, occurrence }) {
  const id = useId();
  const title = titleFor(spec, types);
  const fieldTitle = occurrence ? title + ' ' + occurrence : title;
  const doc = spec.documentation && spec.documentation !== title ? spec.documentation : undefined;
  const fixed = spec.fixed !== undefined && spec.fixed !== null;
  const messages = fieldErrors?.get(normalPath(path)) || [];
  const invalid = messages.length > 0;
  const defaultValue = spec.default == null ? '' : String(spec.default);
  const blank = value == null ? '' : String(value);
  const inputType = primitive(spec.type, types);
  const choices = choicesFor(spec, types);
  const trueFalse = inputType === 'boolean' && (blank === '0' || blank === '1');
  const selected = trueFalse ? (blank === '0' ? 'false' : 'true') : blank;
  const common = { id, value: blank, 'aria-invalid': invalid || undefined,
    'aria-describedby': [doc && id + '-hint', messages.length > 0 && id + '-error'].filter(Boolean).join(' ') || undefined,
    'aria-required': Boolean(spec.required), readOnly: readOnly || fixed,
    onChange: event => onChange(event.target.value) };
  const [initialMultiline] = useState(() => blank.includes('\n') || blank.length > 150);
  const multiline = initialMultiline || spec.name === 'Text' || inputType === 'base64Binary' ||
    spec.type?.match(/(?:1000|2000|4000|8000|Text|Description)/i);
  let control;
  if (choices.length && !readOnly && !fixed) {
    control = (
      <select {...common} readOnly={undefined} value={selected}>
        <option value="">{defaultValue ? 'По умолчанию: ' + (choices.find(item => item.value === defaultValue)?.label || defaultValue) : 'Выберите значение'}</option>
        {selected && !choices.some(item => String(item.value) === selected) && <option value={selected}>{selected}</option>}
        {choices.map(item => <option key={item.value} value={item.value}>{item.label || item.value}</option>)}
      </select>
    );
  } else if (multiline && !choices.length) {
    control = <textarea {...common} rows={3} spellCheck={false} />;
  } else {
    const display = readOnly && choices.length ? choices.find(item => String(item.value) === selected)?.label || blank : blank;
    const type = inputType === 'date' && (!blank || dateInputValue(blank)) ? 'date' : 'text';
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
      {messages.length > 0 && <span id={id + '-error'} className="field-error">{messages.join(' ')}</span>}
      {fixed && blank !== String(spec.fixed) && !readOnly && (
        <button type="button" className="text-button" onClick={() => onChange(String(spec.fixed))}>Установить {spec.fixed}</button>
      )}
      {!fixed && !blank && defaultValue && !readOnly && (
        <button type="button" className="text-button" onClick={() => onChange(defaultValue)}>Установить значение по умолчанию ({defaultValue})</button>
      )}
    </div>
  );
}

function Attributes({ element, type, context, showDocumentation = true }) {
  const attributes = type.attributes || [];
  const extra = knownAttributeNames(element, attributes);
  if (!attributes.length && !extra.length) return null;
  return (
    <div className="field-grid attributes">
      {attributes.map(spec => (
        <Field key={spec.name} spec={showDocumentation ? spec : { ...spec, documentation: '' }}
          types={context.types} value={element.getAttribute(spec.name)}
          readOnly={context.readOnly || (context.lockGuid && !element.parentElement && spec.name === context.idAttribute)}
          path={pathFor(element) + '/@' + spec.name} fieldErrors={context.fieldErrors}
          onChange={value => context.change(() => {
            if (value === '' && !spec.required) element.removeAttribute(spec.name);
            else element.setAttribute(spec.name, value);
          })} />
      ))}
      {extra.length > 0 && <p className="preserved field-wide">Дополнительные атрибуты: {extra.join(', ')}. Доступны во вкладке XML.</p>}
    </div>
  );
}

function TableGrid({ element, type, context }) {
  const rowTypes = type.particle.children;
  const rows = rowTypes.flatMap(spec => directElements(element, new Set([spec.name])).map((row, index, siblings) => {
    const rowType = context.types[spec.type];
    const cellSpec = rowType.particle.children[0];
    return { row, spec, rowType, cellSpec, cells: directElements(row, new Set([cellSpec.name])), siblings, index };
  }));
  const width = rows.reduce((maximum, row) => Math.max(maximum, row.cells.length), 0);
  const columns = Array.from({ length: width }, (_, index) => index);
  const ragged = rows.some(row => row.cells.length !== width);
  const editable = !context.readOnly;
  const addCell = entry => insertElement(entry.row, entry.cellSpec, entry.rowType.particle, context.types);
  const addRow = spec => {
    const row = insertElement(element, spec, type.particle, context.types);
    const rowType = context.types[spec.type];
    for (let column = 1; column < width; column++) {
      insertElement(row, rowType.particle.children[0], rowType.particle, context.types);
    }
  };
  const tablePath = pathFor(element);
  return (
    <div className="table-editor">
      {editable && (
        <div className="inline-actions">
          {rowTypes.map(spec => (
            <button className="button button-quiet" type="button" key={spec.name}
              data-xml-path={tablePath + '/' + spec.name}
              onClick={() => context.change(() => addRow(spec))}>
              {spec.name === 'TitleRow' ? 'Добавить строку заголовка' : 'Добавить строку таблицы'}
            </button>
          ))}
          <button className="button button-quiet" type="button" disabled={!rows.length || ragged}
            onClick={() => context.change(() => rows.forEach(addCell))}>Добавить столбец</button>
        </div>
      )}
      {editable && ragged && <p className="field-hint">Для общего столбца выровняйте число ячеек в строках.</p>}
      {rows.length > 0 && <div className="table-scroll" role="region" aria-label="Строки и ячейки таблицы" tabIndex={0}>
        <table className="xml-table">
          <thead>
            <tr>
              <th scope="col">Строка</th>
              {columns.map(column => (
                <th scope="col" key={column}>
                  Столбец {column + 1}
                  {editable && <button className="text-button" type="button"
                    aria-label={'Удалить столбец ' + (column + 1)}
                    disabled={rows.some(({ cells, cellSpec }) => cells[column] && cells.length <= cellSpec.min)}
                    onClick={() => context.change(() => rows.forEach(({ cells }) => cells[column]?.remove()))}>Удалить</button>}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {rows.map(entry => {
              const { row, spec, cells, cellSpec, siblings, index } = entry;
              const title = (spec.name === 'TitleRow' ? 'Заголовок ' : 'Строка ') + (index + 1);
              const rowPath = pathFor(row);
              const cellType = context.types[cellSpec.type];
              return (
                <tr key={context.keyFor(row)} data-xml-path={rowPath}>
                  <th scope="row">
                    {title}
                    {editable && <div className="table-row-actions">
                      <button className="text-button" type="button" data-xml-path={rowPath + '/Cell'}
                        aria-label={'Добавить ячейку — ' + title} onClick={() => context.change(() => addCell(entry))}>Добавить ячейку</button>
                      {siblings.length > 1 && <>
                        <button className="text-button" type="button" disabled={index === 0}
                          aria-label={'Переместить вверх — ' + title}
                          onClick={() => context.change(() => element.insertBefore(row, siblings[index - 1]))}>Вверх</button>
                        <button className="text-button" type="button" disabled={index === siblings.length - 1}
                          aria-label={'Переместить вниз — ' + title}
                          onClick={() => context.change(() => element.insertBefore(row, siblings[index + 1].nextSibling))}>Вниз</button>
                      </>}
                      <button className="text-button" type="button" disabled={siblings.length <= spec.min}
                        aria-label={'Удалить — ' + title} onClick={() => context.change(() => row.remove())}>Удалить</button>
                    </div>}
                  </th>
                  {cells.map((cell, column) => (
                    <td key={context.keyFor(cell)}>
                      <Field spec={{ name: 'Ячейка ' + (column + 1), type: cellType.textType }} types={context.types}
                        value={readElementText(cell)} path={pathFor(cell)} readOnly={context.readOnly} fieldErrors={context.fieldErrors}
                        onChange={value => context.change(() => writeElementText(cell, value))}
                        onRemove={cells.length > cellSpec.min ? () => context.change(() => cell.remove()) : undefined} />
                      <Attributes element={cell} type={cellType} context={context} showDocumentation={false} />
                    </td>
                  ))}
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>}
    </div>
  );
}

function ComplexContent({ element, type, context }) {
  const names = leafNames(type.particle);
  const unknown = Array.from(element.children).filter(child => !names.has(child.localName));
  const valueType = type.textType;
  const fields = !context.readOnly && fileFields(type);
  return (
    <>
      <DesktopLookup element={element} type={type} context={context} />
      {fields && <FileMetadataPicker element={element} type={type} fields={fields} context={context} />}
      {!context.readOnly && <ImagePicker element={element} type={type} context={context} />}
      <Attributes element={element} type={type} context={context} />
      {valueType && (
        <Field spec={{ name: 'Значение', type: valueType, required: true }} types={context.types} value={readElementText(element)}
          readOnly={context.readOnly} path={pathFor(element)} fieldErrors={context.fieldErrors}
          onChange={value => context.change(() => writeElementText(element, value))} />
      )}
      {type.particle && (context.familyId === 'explanatorynote' && type === context.types.tTable
        ? <TableGrid element={element} type={type} context={context} />
        : <ParticleEditor parent={element} spec={type.particle} parentParticle={type.particle} context={context} />)}
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
  const fields = !context.readOnly && canAdd && fileFields(type);
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
        readOnly={context.readOnly} path={path} fieldErrors={context.fieldErrors}
        onChange={value => updateSimple(node, value)}
        onRemove={node && spec.min === 0 ? () => context.change(() => node.remove()) : undefined} />
    );
  }

  if (!nodes.length && context.readOnly) return null;
  return (
    <div className="element-group field-wide" data-xml-path={pathFor(parent) + '/' + spec.name}>
      {nodes.map((element, index) => isComplex
        ? <ComplexInstance key={context.keyFor(element)} element={element} spec={spec} type={type} index={index}
            repeatable={repeatable || nodes.length > 1} removable={nodes.length > spec.min} context={context} />
        : <Field key={context.keyFor(element)} spec={fieldSpec} occurrence={index + 1} types={context.types}
            value={readElementText(element)} readOnly={context.readOnly} path={pathFor(element)} fieldErrors={context.fieldErrors}
            onChange={value => updateSimple(element, value)}
            onRemove={nodes.length > spec.min ? () => context.change(() => element.remove()) : undefined} />
      )}
      {fields && <FileMetadataPicker element={parent} type={type} fields={fields} context={context}
        spec={spec} parentParticle={parentParticle} />}
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
              fieldErrors={context.fieldErrors} onChange={value => context.change(() => writeElementText(element, value))}
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

function initializesBranch(spec) {
  if (!spec?.min || (spec.kind === 'choice' && spec.children?.length !== 1)) return false;
  return spec.kind === 'element' || Boolean(spec.children?.some(initializesBranch));
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
  const selectedBranch = selected === null ? null : branches[selected];
  const selectedType = context.types[selectedBranch?.type];
  const selectedFiles = !multiple && selectedBranch?.kind === 'element' && fileFields(selectedType);
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
    <div className="choice-block field-wide" data-choice-parent-path={pathFor(parent)}
      data-choice-elements={[...leafNames(spec)].join(' ')}>
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
          {(!selectedBranch || selectedBranch.kind === 'element' || initializesBranch(selectedBranch)) && (
            <button type="button" className="button button-quiet" disabled={selected === null} onClick={addSelected}>Добавить</button>
          )}
          {selectedFiles && <FileMetadataPicker key={selectedBranch.name} element={parent} type={selectedType}
            fields={selectedFiles} context={context} spec={selectedBranch} parentParticle={parentParticle} />}
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
        ) : <button type="button" className="text-button choice-switch" onClick={() => setClearing(true)}>Сменить вариант</button>
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
    <div className="schema-form" data-xml-path={pathFor(element)}>
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
