using Core.NET.Models.XPO;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.NET.Controllers
{
    /// <summary>
    /// Что делать, если в базе уже есть документ с тем же идентификатором, что и импортируемый файл.
    /// </summary>
    public enum ImportConflictAction
    {
        /// <summary>Заменить содержимое существующего документа (только для документа того же типа).</summary>
        Replace,

        /// <summary>Загрузить копию с новым идентификатором.</summary>
        CreateCopy,

        /// <summary>Не загружать.</summary>
        Cancel
    }

    /// <summary>
    /// Результат импорта XML-файла.
    /// </summary>
    public sealed class ImportResult
    {
        /// <summary>Загруженный (созданный или заменённый) документ; <c>null</c>, если файл не прочитан или импорт отменён.</summary>
        public XmlObjXPO Document { get; internal set; }

        /// <summary>Пользователь отменил импорт из-за совпадения идентификатора.</summary>
        public bool IsCancelled { get; internal set; }

        /// <summary>Содержимое существующего документа заменено.</summary>
        public bool IsReplaced { get; internal set; }

        /// <summary>Документ загружен копией с новым идентификатором.</summary>
        public bool IsCopy { get; internal set; }

        /// <summary>Что было исправлено при чтении файла (например, удалённые значения, которых нет в схеме).</summary>
        public List<string> Warnings { get; } = new List<string>();
    }

    internal static class ImportController
    {
        /// <summary>
        /// Сохраняет прочитанный документ с учётом совпадения идентификатора.
        /// Раньше повторный импорт молча создавал вторую запись с тем же GUID.
        /// </summary>
        /// <param name="guid">Идентификатор из файла (содержимое или имя файла), если найден.</param>
        /// <param name="resolveConflict">Решение при совпадении идентификатора; если не задано — загружается копия.</param>
        /// <param name="create">Создание записи нужного типа.</param>
        /// <param name="serialize">XML документа; аргумент — новый идентификатор, если загружается копия (иначе <c>null</c>).</param>
        internal static async Task<ImportResult> SaveAsync<TXpo>(
            ImportResult result,
            string name,
            Guid? guid,
            Func<XmlObjXPO, ImportConflictAction> resolveConflict,
            Func<Session, TXpo> create,
            Func<Guid?, string> serialize) where TXpo : XmlObjXPO
        {
            var session = SessionController.GetSession();

            if (guid != null)
            {
                var existing = await new XPQuery<XmlObjXPO>(session).FirstOrDefaultAsync(f => f.Guid == guid);
                if (existing != null)
                {
                    var action = resolveConflict?.Invoke(existing) ?? ImportConflictAction.CreateCopy;
                    if (action == ImportConflictAction.Cancel)
                    {
                        result.IsCancelled = true;
                        return result;
                    }

                    if (action == ImportConflictAction.Replace && existing is TXpo)
                    {
                        var replacedXml = serialize(null);
                        if (string.IsNullOrWhiteSpace(replacedXml))
                        {
                            return result;
                        }

                        existing.SetDocument(name, replacedXml);
                        await session.SaveAsync(existing);

                        result.Document = existing;
                        result.IsReplaced = true;
                        return result;
                    }

                    // Копия: запись получит новый идентификатор в AfterConstruction.
                    guid = null;
                    result.IsCopy = true;
                }
            }

            var xpoObj = create(session);
            if (guid != null)
            {
                xpoObj.SetGuid(guid.Value);
            }

            var xml = serialize(result.IsCopy ? xpoObj.Guid : null);
            if (string.IsNullOrWhiteSpace(xml))
            {
                return result;
            }

            xpoObj.SetDocument(name, xml);
            xpoObj.SetStatus(DocumentStatus.InProgress);
            await session.SaveAsync(xpoObj);

            result.Document = xpoObj;
            return result;
        }
    }
}
