using Core.NET.Controllers;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComboBox = System.Windows.Forms.ComboBox;

namespace ProjectDocumentation.NET.Forms
{
    internal sealed class FileOperationHistoryForm : XtraForm
    {
        private readonly GridControl grid = new GridControl { Dock = DockStyle.Fill };
        private readonly GridView view;
        private readonly TextEdit search = new TextEdit { Width = 250, AccessibleName = "Поиск по истории файлов" };
        private readonly ComboBox kindFilter = new ComboBox { Width = 126, DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "Операция" };
        private readonly ComboBox statusFilter = new ComboBox { Width = 146, DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "Результат" };
        private readonly MemoEdit details = new MemoEdit { Dock = DockStyle.Fill, AccessibleName = "Подробности операции" };
        private readonly SimpleButton refresh = new SimpleButton { AutoSize = true, Text = "Обновить" };
        private readonly SimpleButton retry = new SimpleButton { AutoSize = true, Text = "Повторить импорт" };
        private readonly SimpleButton openFolder = new SimpleButton { AutoSize = true, Text = "Открыть папку" };
        private readonly SimpleButton copy = new SimpleButton { AutoSize = true, Text = "Копировать подробности" };
        private IReadOnlyList<FileOperationEntry> entries = Array.Empty<FileOperationEntry>();
        private bool loading;

        internal FileOperationEntry RetryEntry { get; private set; }

        internal FileOperationHistoryForm()
        {
            Text = "История файлов";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1050, 630);
            MinimumSize = new Size(730, 470);
            AutoScaleMode = AutoScaleMode.Font;

            view = new GridView(grid);
            grid.ViewCollection.Add(view);
            grid.MainView = view;
            view.OptionsBehavior.Editable = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsSelection.EnableAppearanceFocusedCell = false;
            var date = view.Columns.AddVisible(nameof(HistoryRow.OccurredAt), "Дата и время");
            date.DisplayFormat.FormatType = FormatType.DateTime;
            date.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            date.Width = 151;
            view.Columns.AddVisible(nameof(HistoryRow.Kind), "Операция").Width = 85;
            view.Columns.AddVisible(nameof(HistoryRow.Status), "Результат").Width = 112;
            view.Columns.AddVisible(nameof(HistoryRow.FileName), "Файл").Width = 230;
            view.Columns.AddVisible(nameof(HistoryRow.DocumentName), "Документ").Width = 220;
            view.Columns.AddVisible(nameof(HistoryRow.Version), "Схема").Width = 65;
            view.Columns.AddVisible(nameof(HistoryRow.Summary), "Подробности").Width = 290;
            view.FocusedRowChanged += (_, __) => ShowSelection();

            details.Properties.ReadOnly = true;
            details.Properties.WordWrap = true;
            search.Properties.NullValuePrompt = "Файл, документ или ошибка";
            kindFilter.Items.AddRange(new object[] { "Все операции", "Импорт", "Выгрузка" });
            statusFilter.Items.Add("Все результаты");
            foreach (FileOperationStatus status in Enum.GetValues(typeof(FileOperationStatus)))
                statusFilter.Items.Add(new StatusChoice(status));
            kindFilter.SelectedIndex = 0;
            statusFilter.SelectedIndex = 0;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                Size = new Size(1030, 550),
                Panel1MinSize = 125,
                Panel2MinSize = 105,
                SplitterDistance = 330,
                SplitterWidth = 6
            };
            split.Panel1.Controls.Add(grid);
            split.Panel2.Controls.Add(details);

            var close = new SimpleButton { AutoSize = true, Text = "Закрыть", DialogResult = DialogResult.Cancel };
            var searchLabel = new LabelControl { Text = "Поиск", Margin = new Padding(0, 6, 4, 0) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, Padding = new Padding(9) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(ButtonRow(searchLabel, search, kindFilter, statusFilter, refresh), 0, 0);
            layout.Controls.Add(split, 0, 1);
            layout.Controls.Add(ButtonRow(retry, openFolder, copy, close), 0, 2);
            Controls.Add(layout);
            CancelButton = close;

            search.EditValueChanged += (_, __) => ApplyFilters();
            kindFilter.SelectedIndexChanged += (_, __) => ApplyFilters();
            statusFilter.SelectedIndexChanged += (_, __) => ApplyFilters();
            refresh.Click += async (_, __) => await RefreshEntriesAsync();
            Shown += async (_, __) => await RefreshEntriesAsync();
            retry.Click += (_, __) =>
            {
                var entry = SelectedEntry;
                if (entry?.Kind != FileOperationKind.Import || string.IsNullOrWhiteSpace(entry.FilePath)) return;
                RetryEntry = entry;
                DialogResult = DialogResult.OK;
                Close();
            };
            openFolder.Click += (_, __) => OpenSelectedFolder();
            copy.Click += (_, __) =>
            {
                if (SelectedEntry == null) return;
                try { Clipboard.SetText(details.Text); }
                catch (ExternalException error)
                {
                    XtraMessageBox.Show(this, error.Message, "Буфер обмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            ShowSelection();
        }

        private FileOperationEntry SelectedEntry => (view.GetFocusedRow() as HistoryRow)?.Entry;

        private static FlowLayoutPanel ButtonRow(params Control[] controls)
        {
            var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(0, 2, 0, 2) };
            panel.Controls.AddRange(controls);
            return panel;
        }

        private async Task RefreshEntriesAsync()
        {
            if (loading) return;
            loading = true;
            refresh.Enabled = false;
            UseWaitCursor = true;
            try
            {
                entries = await FileOperationHistory.ReadAsync();
                if (!IsDisposed && !Disposing) ApplyFilters();
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                    XtraMessageBox.Show(this, error.Message, "Не удалось прочитать историю файлов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loading = false;
                if (!IsDisposed && !Disposing)
                {
                    refresh.Enabled = true;
                    UseWaitCursor = false;
                }
            }
        }

        private void ApplyFilters()
        {
            var text = search.Text.Trim();
            var filter = (statusFilter.SelectedItem as StatusChoice)?.Value;
            var kind = kindFilter.SelectedIndex == 1 ? FileOperationKind.Import
                : kindFilter.SelectedIndex == 2 ? (FileOperationKind?)FileOperationKind.Export : null;
            grid.DataSource = entries.Where(entry =>
                    (!kind.HasValue || entry.Kind == kind) && (!filter.HasValue || entry.Status == filter) &&
                    Matches(entry, text))
                .Select(entry => new HistoryRow(entry)).ToList();
            ShowSelection();
        }

        private static bool Matches(FileOperationEntry entry, string text)
        {
            if (text.Length == 0) return true;
            bool Contains(string value) => value != null && value.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
            return Contains(entry.FileName) || Contains(entry.FilePath) || Contains(entry.DocumentName) ||
                Contains(entry.Version) || Contains(entry.Summary) || Contains(entry.DocumentId?.ToString("D")) ||
                entry.Diagnostics.Any(issue => Contains(issue.Path) || Contains(issue.Message));
        }

        private void ShowSelection()
        {
            var entry = SelectedEntry;
            retry.Enabled = entry?.Kind == FileOperationKind.Import && !string.IsNullOrWhiteSpace(entry.FilePath);
            openFolder.Enabled = !string.IsNullOrWhiteSpace(entry?.FilePath);
            copy.Enabled = entry != null;
            if (entry == null)
            {
                details.Text = entries.Count == 0 ? "Операции с файлами пока не записаны."
                    : "Выберите операцию или измените условия поиска.";
                return;
            }

            var text = new StringBuilder()
                .AppendLine($"{KindName(entry.Kind)} — {StatusName(entry.Status)}")
                .AppendLine(entry.Summary);
            if (!string.IsNullOrWhiteSpace(entry.FilePath)) text.AppendLine($"Путь: {entry.FilePath}");
            else if (!string.IsNullOrWhiteSpace(entry.FileName)) text.AppendLine($"Файл: {entry.FileName}");
            if (!string.IsNullOrWhiteSpace(entry.DocumentName)) text.AppendLine($"Документ: {entry.DocumentName}");
            if (!string.IsNullOrWhiteSpace(entry.Family) || !string.IsNullOrWhiteSpace(entry.Version))
                text.AppendLine($"Формат: {FamilyName(entry.Family)} {entry.Version}".TrimEnd());
            if (entry.DocumentId.HasValue) text.AppendLine($"ID документа: {entry.DocumentId:D}");
            if (entry.Diagnostics.Count > 0)
            {
                text.AppendLine().AppendLine("Замечания и ошибки");
                foreach (var issue in entry.Diagnostics)
                    text.AppendLine(string.IsNullOrWhiteSpace(issue.Path) ? issue.Message : $"{issue.Path} — {issue.Message}");
            }
            details.Text = text.ToString().TrimEnd();
        }

        private void OpenSelectedFolder()
        {
            var path = SelectedEntry?.FilePath;
            if (string.IsNullOrWhiteSpace(path)) return;
            try
            {
                var folder = Path.GetDirectoryName(Path.GetFullPath(path));
                if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                    throw new DirectoryNotFoundException("Папка недоступна или была перемещена.");

                Process.Start(new ProcessStartInfo(folder) { UseShellExecute = true })?.Dispose();
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(this, error.Message, "Не удалось открыть папку", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string KindName(FileOperationKind kind) => kind == FileOperationKind.Import ? "Импорт" : "Выгрузка";

        private static string FamilyName(string family) => family == "conclusion" ? "Заключение"
            : family == "explanatorynote" ? "Пояснительная записка" : family;

        private static string StatusName(FileOperationStatus status)
        {
            switch (status)
            {
                case FileOperationStatus.Completed: return "Выполнено";
                case FileOperationStatus.Warning: return "С замечаниями";
                case FileOperationStatus.Rejected: return "Отклонено";
                case FileOperationStatus.Failed: return "Ошибка";
                case FileOperationStatus.Cancelled: return "Отменено";
                default: return status.ToString();
            }
        }

        private sealed class StatusChoice
        {
            internal StatusChoice(FileOperationStatus value) => Value = value;
            internal FileOperationStatus Value { get; }
            public override string ToString() => StatusName(Value);
        }

        private sealed class HistoryRow
        {
            internal HistoryRow(FileOperationEntry entry) => Entry = entry;
            internal FileOperationEntry Entry { get; }
            public DateTime OccurredAt => Entry.OccurredAt;
            public string Kind => KindName(Entry.Kind);
            public string Status => StatusName(Entry.Status);
            public string FileName => Entry.FileName ?? Entry.FilePath;
            public string DocumentName => Entry.DocumentName;
            public string Version => Entry.Version;
            public string Summary => Entry.Summary;
        }
    }
}
