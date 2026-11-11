const checksumChunkBytes = 1024 * 1024;

const crc32Table = Uint32Array.from({ length: 256 }, (_, index) => {
  let value = index;
  for (let bit = 0; bit < 8; bit++) {
    value = value & 1 ? (value >>> 1) ^ 0xedb88320 : value >>> 1;
  }
  return value;
});

export async function crc32(file, signal) {
  let value = 0xffffffff;
  for (let offset = 0; offset < file.size; offset += checksumChunkBytes) {
    signal.throwIfAborted();
    const chunk = new Uint8Array(await file.slice(offset, offset + checksumChunkBytes).arrayBuffer());
    for (const byte of chunk) value = (value >>> 8) ^ crc32Table[(value ^ byte) & 0xff];
  }
  signal.throwIfAborted();
  return ((value ^ 0xffffffff) >>> 0).toString(16).padStart(8, '0');
}

function extension(file) {
  const dot = file.name.lastIndexOf('.');
  return dot < 0 ? '' : file.name.slice(dot + 1).toLowerCase();
}

export async function fileMetadata(file, family, signal) {
  const format = extension(file);
  const validFormat = family === 'conclusion' ? /^[a-z0-9]{1,4}$/.test(format)
    : Boolean(format.trim()) && Array.from(format).length <= 4;
  if (!validFormat) {
    throw new Error('«' + file.name + '»: расширение файла не подходит для поля «Формат».');
  }
  if (Array.from(file.name).length > 255) {
    throw new Error('«' + file.name + '»: имя файла должно содержать не больше 255 символов.');
  }
  return { FileName: file.name, FileFormat: format, FileChecksum: await crc32(file, signal) };
}

function startsWith(bytes, signature) {
  return signature.every((byte, index) => bytes[index] === byte);
}

export async function imageType(file, allowed) {
  const header = new Uint8Array(await file.slice(0, 8).arrayBuffer());
  let formats = [];
  if (startsWith(header, [0xff, 0xd8, 0xff])) {
    formats = extension(file) === 'jpeg' ? ['jpeg', 'jpg'] : ['jpg', 'jpeg'];
  } else if (startsWith(header, [0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a])) {
    formats = ['png'];
  } else if (startsWith(header, [0x47, 0x49, 0x46, 0x38]) &&
      (header[4] === 0x37 || header[4] === 0x39) && header[5] === 0x61) {
    formats = ['gif'];
  }
  const format = formats.find(value => allowed.includes(value));
  if (!format) throw new Error('Выберите изображение JPEG, PNG или GIF.');
  return format;
}

export function readBase64(file, signal) {
  signal.throwIfAborted();
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    const abort = () => reader.abort();
    signal.addEventListener('abort', abort, { once: true });
    reader.addEventListener('loadend', () => signal.removeEventListener('abort', abort), { once: true });
    reader.addEventListener('load', () => resolve(String(reader.result).split(',', 2)[1]), { once: true });
    reader.addEventListener('error', () => reject(new Error('Не удалось прочитать изображение.')), { once: true });
    reader.addEventListener('abort', () => reject(signal.reason), { once: true });
    reader.readAsDataURL(file);
  });
}
