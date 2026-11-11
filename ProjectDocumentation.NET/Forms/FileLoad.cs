using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using ProjectDocumentation.NET.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ProjectDocumentation.NET.Forms
{
    public partial class FileLoad : XtraForm
    {
        public XmlObjXPO ObjXPO { get; private set; }

        private string suggestedName;
        private bool importing;

        public FileLoad() : this(null) { }

        internal FileLoad(ConclusionSchemaService conclusionSchema)
        {
            this.conclusionSchema = conclusionSchema;
            InitializeComponent();
            btnFile.EditValueChanged += (_, __) => txtDocumentType.EditValue = null;
            btnFile.Validated += (_, __) => DescribeSelectedFile();
            FormClosing += (_, e) =>
            {
                if (importing && ObjXPO == null && e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
            };
        }

        private void DescribeSelectedFile()
        {
            var path = btnFile.EditValue?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(path)) return;

            try
            {
                var (rootName, version) = SchemaXmlDocument.ReadFileHeader(path);
                DescribeFile(path, rootName, version);
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException ||
                error is XmlException || error is InvalidOperationException || error is ArgumentException || error is NotSupportedException)
            {
                txtDocumentType.EditValue = "Не удалось определить тип XML";
                txtDocumentType.ToolTip = error.Message;
            }
        }

        private void DescribeFile(string path, string rootName, string version)
        {
            string kind;
            switch (rootName)
            {
                case Conclusion0104Document.RootName:
                    kind = "Заключение";
                    break;
                case ExplanatoryNote0107Document.RootName:
                    kind = "Пояснительная записка";
                    break;
                default:
                    throw new InvalidOperationException("Выберите XML заключения или пояснительной записки.");
            }

            txtDocumentType.EditValue = string.IsNullOrWhiteSpace(version)
                ? $"{kind} (версия в XML не указана)"
                : $"{kind} {version}";
            txtDocumentType.ToolTip = string.Empty;
            if (string.IsNullOrWhiteSpace(txtName.Text) || txtName.Text == suggestedName)
            {
                suggestedName = Path.GetFileNameWithoutExtension(path);
                txtName.Text = suggestedName;
            }
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
                            DescribeSelectedFile();
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (importing) return;
            SetImporting(true);
            try
            {
                await ImportSelectedFileAsync();
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                    XtraMessageBox.Show(this, error.Message, "Ошибка загрузки XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    SetImporting(false);
                }
            }
        }

        private void SetImporting(bool active)
        {
            importing = active;
            btnSave.Enabled = !active;
            btnCancel.Enabled = !active;
            btnFile.Properties.ReadOnly = active;
            txtName.Properties.ReadOnly = active;
        }

        private async Task ImportSelectedFileAsync()
        {
            var filePath = btnFile.EditValue?.ToString()?.Trim();

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
                var document = SchemaXmlDocument.ReadFile(filePath);
                var rootName = document.Root.Name.NamespaceName.Length == 0 ? document.Root.Name.LocalName : null;
                var version = (string)document.Root.Attribute("SchemaVersion");
                DescribeFile(filePath, rootName, version);
                var name = txtName.Text.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    XtraMessageBox.Show("Укажите наименование для сохранения.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                    txtName.Focus();
                    return;
                }

                ImportResult result;
                switch (rootName)
                {
                    case Conclusion0104Document.RootName:
                        if (version == Conclusion0104Document.CurrentVersion)
                        {
                            await ImportCurrentConclusionAsync(document, name);
                            return;
                        }

                        if (Conclusion0104Document.IsHistoricalVersion(version))
                        {
                            await ImportHistoricalConclusionAsync(document, name);
                            return;
                        }

                        if (!Conclusion0104Document.CanUseLegacyEditor(version))
                        {
                            throw new InvalidOperationException($"Версия заключения {version} не поддерживается.");
                        }

                        Func<XmlObjXPO, ImportConflictAction> resolveConflict = existing => AskImportConflict(existing,
                            existing is ConclusionXPO && Conclusion0104Document.CanUseLegacyEditor(Conclusion0104Document.ReadVersion(existing.GetObj())));
                        result = version == Conclusion0104Document.LegacyVersion
                            ? await ConclusionController.ImportConclusionAsync(document, filePath, name, resolveConflict)
                            : await ConclusionController.ImportConclusionAsync(filePath, name, resolveConflict);
                        break;
                    case ExplanatoryNote0107Document.RootName:
                        if (version == ExplanatoryNote0107Document.CurrentVersion || version == ExplanatoryNote0107Document.CompatibleVersion)
                        {
                            await ImportExplanatoryNoteAsync(document, version, filePath, name);
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(version) && version != ExplanatoryNote0107Document.LegacyVersion)
                        {
                            throw new InvalidOperationException($"Версия пояснительной записки {version} не поддерживается.");
                        }

                        result = await ExplanatoryNoteController.ImportExplanatoryNoteAsync(filePath, name, existing => AskImportConflict(existing,
                            existing is ExplanatoryNoteXPO && ExplanatoryNoteController.IsLegacyXml(existing.GetObj())));
                        break;
                    default:
                        throw new InvalidOperationException("Выберите XML заключения или пояснительной записки.");
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
                        + "Замечания при чтении XML:" + Environment.NewLine
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
