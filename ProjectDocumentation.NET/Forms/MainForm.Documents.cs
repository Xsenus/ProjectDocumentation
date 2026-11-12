using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ProjectDocumentation.NET.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class MainForm
    {
        private const string VersionField = "XmlVersion";
        private readonly Dictionary<int, string> documentVersions = new Dictionary<int, string>();
        private BarSubItem documentStatusMenu;
        private BarButtonItem markCompleted;
        private BarButtonItem markInProgress;
        private bool changingDocumentStatus;

        private BarSubItem CreateDocumentStatusMenu()
        {
            documentStatusMenu = new BarSubItem(barManager, "Статус");
            markCompleted = new BarButtonItem(barManager, "Проверить XML и завершить");
            markInProgress = new BarButtonItem(barManager, "Перевести в работу");
            markCompleted.ItemClick += async (_, __) => await ChangeDocumentStatusAsync(DocumentStatus.Completed);
            markInProgress.ItemClick += async (_, __) => await ChangeDocumentStatusAsync(DocumentStatus.InProgress);
            documentStatusMenu.AddItem(markCompleted);
            documentStatusMenu.AddItem(markInProgress);
            return documentStatusMenu;
        }

        private void UpdateDocumentStatusCommands(XmlObjXPO record)
        {
            documentStatusMenu.Enabled = record != null && !changingDocumentStatus;
            markCompleted.Enabled = documentStatusMenu.Enabled && record.Status != DocumentStatus.Completed;
            markInProgress.Enabled = documentStatusMenu.Enabled && record.Status != DocumentStatus.InProgress;
        }

        private async Task ChangeDocumentStatusAsync(DocumentStatus status)
        {
            if (changingDocumentStatus || !(gridView.GetFocusedRow() is XmlObjXPO record) || record.Status == status) return;

            changingDocumentStatus = true;
            UpdateDocumentStatusCommands(record);
            try
            {
                var originalName = record.Name;
                var originalXml = record.GetObj();
                var originalStatus = record.Status;
                if (status == DocumentStatus.Completed)
                {
                    var version = ReadDocumentVersion(record);
                    var validation = await conclusionSchema.ValidateAsync(originalXml);
                    SchemaXmlDocument.RequireValid(validation, version ?? validation.Version);
                }

                using (var unit = new UnitOfWork(Session.DataLayer))
                {
                    var current = unit.GetObjectByKey<XmlObjXPO>(record.Oid);
                    if (current == null || current.Guid != record.Guid || current.Name != originalName ||
                        current.GetObj() != originalXml || current.Status != originalStatus)
                        throw new InvalidOperationException("Документ изменился или был удалён. Обновите список документов.");

                    if (status == DocumentStatus.Completed)
                        current.SetValidatedCompletion(originalXml);
                    else
                        current.SetStatus(status);
                    await unit.CommitChangesAsync();
                }
                Session.GetObjectByKey<XmlObjXPO>(record.Oid, true);
                await UpdateGridDataSourceAsync(nameof(XmlObjXPO.Guid), record.Guid);
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                    XtraMessageBox.Show(this, error.Message, "Не удалось изменить статус", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                changingDocumentStatus = false;
                if (!IsDisposed && !Disposing) UpdateDocumentStatusCommands(gridView.GetFocusedRow() as XmlObjXPO);
            }
        }

        private void InitialSetupGridControl()
        {
            if (gridView.Columns.Count != 0) return;

            gridView.OptionsBehavior.AutoPopulateColumns = false;
            DialogLayout.ShowRowFocus(gridView);
            gridView.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

            var name = gridView.Columns.AddVisible(nameof(XmlObjXPO.Name), "Наименование");
            name.MinWidth = 160;
            name.Width = 310;
            var nameEditor = new RepositoryItemMemoEdit { WordWrap = true };
            gridControl.RepositoryItems.Add(nameEditor);
            name.ColumnEdit = nameEditor;

            var version = AddDocumentColumn(VersionField, "Версия XML", 112);
            version.UnboundDataType = typeof(string);
            AddDocumentColumn(nameof(XmlObjXPO.Status), "Статус", 116);
            AddDocumentDateColumn(nameof(XmlObjXPO.DateCreate), "Дата создания");
            AddDocumentDateColumn(nameof(XmlObjXPO.DateModified), "Дата изменения");
            gridView.Columns.AddField(nameof(XmlObjXPO.Guid)).Caption = "ID";

            gridView.CustomUnboundColumnData += (_, e) =>
            {
                if (!e.IsGetData || e.Column != version || !(e.Row is XmlObjXPO record)) return;
                if (!documentVersions.TryGetValue(record.Oid, out var value))
                {
                    value = ReadDocumentVersion(record) ?? "—";
                    documentVersions[record.Oid] = value;
                }
                e.Value = value;
            };
            gridView.CustomColumnDisplayText += (_, e) =>
            {
                if (e.Column.FieldName == nameof(XmlObjXPO.Status) && e.Value is DocumentStatus status)
                    e.DisplayText = status == DocumentStatus.New ? "Новый" : status == DocumentStatus.Completed ? "Завершён" : "В работе";
                if ((e.Column.FieldName == nameof(XmlObjXPO.DateCreate) || e.Column.FieldName == nameof(XmlObjXPO.DateModified)) &&
                    (!(e.Value is DateTime date) || date == DateTime.MinValue))
                    e.DisplayText = "—";
            };
            gridView.RowStyle += DocumentRowStyle;
        }

        private GridColumn AddDocumentColumn(string fieldName, string caption, int width)
        {
            var column = gridView.Columns.AddVisible(fieldName, caption);
            column.MinWidth = width;
            column.Width = width;
            column.OptionsColumn.FixedWidth = true;
            column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            column.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            return column;
        }

        private void AddDocumentDateColumn(string fieldName, string caption)
        {
            var column = AddDocumentColumn(fieldName, caption, 124);
            column.DisplayFormat.FormatType = FormatType.DateTime;
            column.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm";
        }

        private static string ReadDocumentVersion(XmlObjXPO record)
        {
            var xml = record.GetObj();
            if (record is ConclusionXPO) return Conclusion0104Document.ReadVersion(xml);
            if (!(record is ExplanatoryNoteXPO)) return null;
            return ExplanatoryNote0107Document.ReadVersion(xml) ??
                (ExplanatoryNoteController.IsLegacyXml(xml) ? ExplanatoryNote0107Document.LegacyVersion : null);
        }

        private void DocumentRowStyle(object sender, RowStyleEventArgs e)
        {
            if (SystemInformation.HighContrast || !gridView.IsDataRow(e.RowHandle) ||
                (e.RowHandle == gridView.FocusedRowHandle && gridControl.ContainsFocus) ||
                !(gridView.GetRow(e.RowHandle) is XmlObjXPO record)) return;

            switch (record.Status)
            {
                case DocumentStatus.InProgress:
                    e.Appearance.BackColor = Color.FromArgb(255, 245, 215);
                    e.Appearance.ForeColor = Color.FromArgb(79, 57, 14);
                    break;
                case DocumentStatus.Completed:
                    e.Appearance.BackColor = Color.FromArgb(226, 246, 234);
                    e.Appearance.ForeColor = Color.FromArgb(22, 75, 52);
                    break;
            }
        }
    }
}
