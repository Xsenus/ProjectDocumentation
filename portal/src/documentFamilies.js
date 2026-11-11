export const documentFamilies = {
  conclusion: {
    id: 'conclusion',
    root: 'Conclusion',
    title: 'Заключение экспертизы',
    collection: 'Заключения экспертизы',
    newTitle: 'Новое заключение',
    newAction: 'Новое заключение',
    createAction: 'Создать заключение',
    createProgress: 'Создаю заключение',
    sectionsTitle: 'Разделы заключения',
    previewTitle: 'Предпросмотр заключения',
    idAttribute: 'ConclusionGUID',
    latestVersion: '01.04',
    schemaVersions: ['01.03', '01.04'],
    formVersions: ['01.04'],
    migrationTargets: { '01.03': '01.04', '01.04': '01.03' },
    draftsKey: 'project-documentation:conclusion-drafts:v1',
  },
  explanatorynote: {
    id: 'explanatorynote',
    root: 'ExplanatoryNote',
    title: 'Пояснительная записка',
    collection: 'Пояснительные записки',
    newTitle: 'Новая пояснительная записка',
    newAction: 'Новая записка',
    createAction: 'Создать записку',
    createProgress: 'Создаю пояснительную записку',
    sectionsTitle: 'Разделы записки',
    previewTitle: 'Предпросмотр пояснительной записки',
    idAttribute: 'id',
    latestVersion: '01.07',
    schemaVersions: ['01.05', '01.07'],
    formVersions: ['01.05', '01.07'],
    migrationTargets: { '01.03': '01.07', '01.05': '01.07', '01.07': '01.05' },
    draftsKey: 'project-documentation:explanatorynote-drafts:v1',
  },
};

export function familyForId(id) {
  return Object.hasOwn(documentFamilies, id) ? documentFamilies[id] : documentFamilies.conclusion;
}

export function familyForRoot(rootName) {
  return Object.values(documentFamilies).find(family => family.root === rootName);
}

export function familyForDocument(document) {
  return familyForRoot(document?.documentElement?.localName);
}

export function familyRequest(path, family, version = family.latestVersion) {
  return path + '?' + new URLSearchParams({ family: family.id, version });
}
