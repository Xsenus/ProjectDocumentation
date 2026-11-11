const databaseName = 'project-documentation:file-history';
const storeName = 'operations';
let connection;

function openHistory() {
  if (connection) return connection;
  const pending = new Promise((resolve, reject) => {
    const request = indexedDB.open(databaseName, 1);
    let blocked = false;
    request.onupgradeneeded = () => request.result.createObjectStore(storeName, { keyPath: 'id' });
    request.onblocked = () => {
      blocked = true;
      reject(new Error('Хранилище истории занято другой вкладкой.'));
    };
    request.onerror = () => reject(request.error);
    request.onsuccess = () => {
      const database = request.result;
      if (blocked) {
        database.close();
        return;
      }
      const forget = () => { if (connection === pending) connection = null; };
      database.onclose = forget;
      database.onversionchange = () => {
        database.close();
        forget();
      };
      resolve(database);
    };
  });
  connection = pending;
  pending.catch(() => { if (connection === pending) connection = null; });
  return pending;
}

async function transact(mode, action) {
  const database = await openHistory();
  return new Promise((resolve, reject) => {
    const transaction = database.transaction(storeName, mode);
    const request = action(transaction.objectStore(storeName));
    transaction.oncomplete = () => resolve(request.result);
    transaction.onabort = () => reject(transaction.error || request.error);
  });
}

export function loadFileHistory() {
  return transact('readonly', store => store.getAll());
}

export function saveFileOperation(entry) {
  return transact('readwrite', store => store.put(entry));
}

export function mergeFileHistory(stored, current) {
  const entries = new Map(stored.map(entry => [entry.id, entry]));
  for (const entry of current) {
    const known = entries.get(entry.id);
    if (!known || entry.finishedAt >= known.finishedAt) entries.set(entry.id, entry);
  }
  return [...entries.values()].sort((first, second) => second.createdAt - first.createdAt ||
    second.finishedAt - first.finishedAt);
}

export function historyDiagnostics(result, fallback) {
  if (!result?.errors?.length) return fallback ? [{ path: '/', message: fallback }] : [];
  return result.errors.map(({ path, message, line }) => ({
    path: path || '/',
    message,
    line,
  }));
}
