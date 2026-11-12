using DevExpress.Dialogs.Core.Localization;
using DevExpress.Utils.Localization;
using DevExpress.XtraEditors.Controls;
using System.Globalization;

namespace ProjectDocumentation.NET.Services
{
    internal static class FileDialogLocalization
    {
        private static bool IsRussian => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ru";

        internal static void Configure()
        {
            if (!IsRussian) return;
            DialogsLocalizer.Active = new RussianDialogLocalizer();
            XtraLocalizer.QueryLocalizedString += LocalizeSharedStrings;
        }

        private static void LocalizeSharedStrings(object sender, XtraLocalizer.QueryLocalizedStringEventArgs e)
        {
            if (!IsRussian || e.StringIDType != typeof(StringId)) return;
            switch ((StringId)e.StringID)
            {
                case StringId.SearchControlNullValuePrompt: e.Value = "Введите текст для поиска…"; break;
                case StringId.XtraMessageBoxYesButtonText: e.Value = "&Да"; break;
                case StringId.XtraMessageBoxNoButtonText: e.Value = "&Нет"; break;
            }
        }

        private sealed class RussianDialogLocalizer : DialogsLocalizer
        {
            public override string GetLocalizedString(DialogsStringId id)
            {
                if (!IsRussian) return base.GetLocalizedString(id);
                switch (id)
                {
                    case DialogsStringId.OpenFileDialogDefaultTitle: return "Открытие файла";
                    case DialogsStringId.SaveFileDialogDefaultTitle: return "Сохранение файла";
                    case DialogsStringId.OpenFileDialogOkButtonText: return "Открыть";
                    case DialogsStringId.SaveFileDialogOkButtonText: return "Сохранить";
                    case DialogsStringId.CancelButtonText: return "Отмена";
                    case DialogsStringId.FileNameLabelText: return "Имя файла:";
                    case DialogsStringId.NewFolderButtonText:
                    case DialogsStringId.MakeNewFolderButtonText: return "Создать папку";
                    case DialogsStringId.NewFolderText: return "Новая папка";
                    case DialogsStringId.DetailsView_NameColumnCaption: return "Имя";
                    case DialogsStringId.DetailsView_DateModifiedColumnCaption: return "Дата изменения";
                    case DialogsStringId.DetailsView_TypeColumnCaption: return "Тип";
                    case DialogsStringId.DetailsView_SizeColumnCaption: return "Размер";
                    case DialogsStringId.ExtraLargeViewMode: return "Огромные значки";
                    case DialogsStringId.LargeViewMode: return "Крупные значки";
                    case DialogsStringId.MediumViewMode: return "Обычные значки";
                    case DialogsStringId.SmallViewMode: return "Мелкие значки";
                    case DialogsStringId.ListViewMode: return "Список";
                    case DialogsStringId.DetailsViewMode: return "Таблица";
                    case DialogsStringId.TilesViewMode: return "Плитка";
                    case DialogsStringId.ContentViewMode: return "Содержимое";
                    case DialogsStringId.ItemsEnumerationViewText: return "Загрузка…";
                    case DialogsStringId.EmptyViewText: return "В этой папке нет файлов.";
                    case DialogsStringId.FileNotSelectedText: return "Выберите файл для предпросмотра.";
                    case DialogsStringId.NoPreviewAvailableText: return "Предпросмотр недоступен.";
                    case DialogsStringId.PromptMessagesCaption: return "Подтверждение сохранения";
                    case DialogsStringId.OverwritePromptMessage: return "Файл {0} уже существует.\nЗаменить его?";
                    case DialogsStringId.CreatePromptMessage: return "Файл {0} не существует.\nСоздать его?";
                    case DialogsStringId.FileNotFoundMessage: return "Файл не найден.\nПроверьте имя файла и повторите попытку.";
                    case DialogsStringId.InvalidFileNameMessage: return "Недопустимое имя файла.";
                    case DialogsStringId.InvalidFolderNameMessage: return "Недопустимое имя папки.";
                    case DialogsStringId.FileIsReadOnlyMessage: return "Файл доступен только для чтения.\nУкажите другое имя файла.";
                    case DialogsStringId.PathError_Caption: return "Проводник";
                    case DialogsStringId.PathError_MessageFormatString: return "Не удаётся найти «{0}».\nПроверьте путь и повторите попытку.";
                    case DialogsStringId.PathNotExistsMessage: return "Указанный путь не существует.\nПроверьте путь и повторите попытку.";
                    case DialogsStringId.SaveFileDialogFolderWritePermissionMessage:
                        return "{0}\nУ вас нет прав на сохранение в эту папку.\nОбратитесь к администратору.\n\nСохранить в папку {1}?";
                    case DialogsStringId.MessageBoxCaption: return "Ошибка";
                    case DialogsStringId.ContextMenuOpenCommandText: return "&Открыть";
                    case DialogsStringId.ContextMenuOpenAsCommandText: return "Открыть с &помощью";
                    case DialogsStringId.ContextMenuCutCommandText: return "&Вырезать";
                    case DialogsStringId.ContextMenuCopyCommandText: return "&Копировать";
                    case DialogsStringId.ContextMenuPasteCommandText: return "В&ставить";
                    case DialogsStringId.ContextMenuRenameCommandText: return "П&ереименовать";
                    case DialogsStringId.ContextMenuDeleteCommandText: return "&Удалить";
                    case DialogsStringId.ContextMenuPropertiesCommandText: return "Сво&йства";
                    default: return base.GetLocalizedString(id);
                }
            }
        }
    }
}
