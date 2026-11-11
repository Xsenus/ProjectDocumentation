using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using ProjectDocumentation.NET.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class FileLoad : XtraForm
    {
        public XmlObjXPO ObjXPO { get; private set; }

        public FileLoad() : this(null) { }

        internal FileLoad(ConclusionSchemaService conclusionSchema)
        {
            this.conclusionSchema = conclusionSchema;
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnFile_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    using (var ofd = new OpenFileDialog() { Filter = "*.xml|*.xml" })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            buttonEdit.EditValue = ofd.FileName;
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            try
            {
                await ImportSelectedFileAsync();
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(this, error.Message, "Ошибка загрузки XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    btnSave.Enabled = true;
                }
            }
        }

        private async Task ImportSelectedFileAsync()
        {
            var name = txtName.EditValue?.ToString();
            var filePath = btnFile.EditValue?.ToString();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                XtraMessageBox.Show("Укажите путь к файлу для сохранения.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                btnFile.Focus();
                return;
            }

            if (FileController.CheckFile(filePath))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    XtraMessageBox.Show("Укажите наименование для сохранения.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                    txtName.Focus();
                    return;
                }

                var isConclusion = radioGroupType.EditValue?.ToString() == "Conclusion";
                ImportResult result;
                switch (radioGroupType.EditValue?.ToString())
                {
                    case "Conclusion":
                        var document = Conclusion0104Document.ReadFile(filePath);
                        var version = (string)document.Root.Attribute("SchemaVersion");
                        if (version == Conclusion0104Document.CurrentVersion)
                        {
                            await ImportCurrentConclusionAsync(document, name);
                            return;
                        }

                        if (version != null && version != Conclusion0104Document.LegacyVersion)
                        {
                            throw new InvalidOperationException($"Версия заключения {version} не поддерживается.");
                        }

                        result = await ConclusionController.ImportConclusionAsync(filePath, name, existing => AskImportConflict(existing,
                            existing is ConclusionXPO && Conclusion0104Document.ReadVersion(existing.GetObj()) != Conclusion0104Document.CurrentVersion));
                        break;
                    case "ExplanatoryNote":
                        var noteVersion = ExplanatoryNote0107Document.ReadFileVersion(filePath);
                        if (noteVersion == ExplanatoryNote0107Document.CurrentVersion || noteVersion == ExplanatoryNote0107Document.CompatibleVersion)
                        {
                            await ImportExplanatoryNoteAsync(ExplanatoryNote0107Document.ReadFile(filePath), noteVersion, filePath, name);
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(noteVersion) && noteVersion != ExplanatoryNote0107Document.LegacyVersion)
                        {
                            throw new InvalidOperationException($"Версия пояснительной записки {noteVersion} не поддерживается.");
                        }

                        result = await ExplanatoryNoteController.ImportExplanatoryNoteAsync(filePath, name, existing => AskImportConflict(existing,
                            existing is ExplanatoryNoteXPO && ExplanatoryNoteController.IsLegacyXml(existing.GetObj())));
                        break;
                    default:
                        result = new ImportResult();
                        break;
                }

                if (result.IsCancelled)
                {
                    return;
                }

                ObjXPO = result.Document;
                if (ObjXPO is null)
                {
                    XtraMessageBox.Show("Ошибка загрузки файла: файл не соответствует выбранному типу документа или повреждён.",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                var message = result.IsReplaced
                    ? "Содержимое существующего документа заменено."
                    : result.IsCopy
                        ? $"Файл загружен как копия с новым ID: {ObjXPO.Guid}."
                        : "Файл успешно обработан.";

                if (result.Warnings.Count > 0)
                {
                    // Раньше эти значения удалялись молча.
                    message += Environment.NewLine + Environment.NewLine
                        + "При чтении удалены значения, которых нет в схеме:" + Environment.NewLine
                        + string.Join(Environment.NewLine, result.Warnings);
                }

                XtraMessageBox.Show(message,
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                result.Warnings.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                Close();
            }
            else
            {
                XtraMessageBox.Show("Не найден файл по указанному пути.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        /// <summary>
        /// В базе уже есть документ с тем же ID: заменить его (только тот же тип), загрузить копию или отменить.
        /// </summary>
        private static ImportConflictAction AskImportConflict(XmlObjXPO existing, bool isSameType)
        {
            var header = $"Документ с ID {existing.Guid} уже загружен: «{existing.Name}» от {existing.DateCreate:dd.MM.yyyy HH:mm}.";

            if (isSameType)
            {
                var answer = XtraMessageBox.Show(
                    header + Environment.NewLine + Environment.NewLine +
                    "Да — заменить его содержимое этим файлом." + Environment.NewLine +
                    "Нет — загрузить копию с новым ID." + Environment.NewLine +
                    "Отмена — не загружать.",
                    "Документ уже загружен",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                switch (answer)
                {
                    case DialogResult.Yes:
                        return ImportConflictAction.Replace;
                    case DialogResult.No:
                        return ImportConflictAction.CreateCopy;
                    default:
                        return ImportConflictAction.Cancel;
                }
            }

            var copyAnswer = XtraMessageBox.Show(
                header + Environment.NewLine + "Тип или версия существующего документа отличается. Загрузить файл как новый документ с новым ID?",
                "Документ уже загружен",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            return copyAnswer == DialogResult.OK ? ImportConflictAction.CreateCopy : ImportConflictAction.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
