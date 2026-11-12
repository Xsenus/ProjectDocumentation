using Core.NET.Controllers;
using DevExpress.Images;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Layout;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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

namespace ProjectDocumentation.NET.Forms
{
    internal sealed class FileOperationHistoryForm : XtraForm
    {
        private readonly GridControl grid = new GridControl { Dock = DockStyle.Fill, AccessibleName = "Операции с файлами", TabIndex = 1 };
        private readonly GridView view;
        private readonly ButtonEdit search = new ButtonEdit { Dock = DockStyle.Fill, AccessibleName = "Поиск по истории файлов" };
        private readonly ComboBoxEdit kindFilter = new ComboBoxEdit { Dock = DockStyle.Fill, AccessibleName = "Операция" };
        private readonly ComboBoxEdit statusFilter = new ComboBoxEdit { Dock = DockStyle.Fill, AccessibleName = "Результат" };
        private readonly MemoEdit details = new MemoEdit { Dock = DockStyle.Fill, AccessibleName = "Подробности операции" };
        private readonly SimpleButton refresh = new SimpleButton { Dock = DockStyle.Fill, Text = "Обновить" };
        private readonly SimpleButton retry = new SimpleButton { AutoSize = true, Text = "Повторить импорт" };
        private readonly SimpleButton openFolder = new SimpleButton { AutoSize = true, Text = "Открыть папку" };
        private readonly SimpleButton copy = new SimpleButton { AutoSize = true, Text = "Копировать подробности" };
        private readonly GroupControl operations = new GroupControl { Dock = DockStyle.Fill, Text = "Операции", AccessibleName = "Операции" };
        private readonly SplitContainerControl split = new SplitContainerControl
        {
            Dock = DockStyle.Fill,
            FixedPanel = SplitFixedPanel.Panel2,
            ShowSplitGlyph = DefaultBoolean.True
        };
        private IReadOnlyList<FileOperationEntry> entries = Array.Empty<FileOperationEntry>();
        private bool loading;
        private bool arranged;

        internal FileOperationEntry RetryEntry { get; private set; }

        internal FileOperationHistoryForm()
        {
            Text = "История файлов";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1180, 680);
            MinimumSize = new Size(760, 520);
            AutoScaleMode = AutoScaleMode.Font;
            if (!SystemInformation.HighContrast) LookAndFeel.SetSkinStyle(SkinStyle.WXI);

            foreach (var style in new[]
            {
                grid.LookAndFeel, search.LookAndFeel, kindFilter.LookAndFeel, statusFilter.LookAndFeel, details.LookAndFeel,
                refresh.LookAndFeel, retry.LookAndFeel, openFolder.LookAndFeel, copy.LookAndFeel, operations.LookAndFeel, split.LookAndFeel
            })
                style.ParentLookAndFeel = LookAndFeel;

            view = new GridView(grid);
            grid.ViewCollection.Add(view);
            grid.MainView = view;
            ConfigureGrid();

            details.Properties.ReadOnly = true;
            details.Properties.WordWrap = true;
            details.Properties.Appearance.FontSizeDelta = 2;
            details.Properties.AppearanceReadOnly.FontSizeDelta = 2;
            details.Properties.BorderStyle = BorderStyles.NoBorder;
            search.ToolTip = "Поиск по истории (Ctrl+F)";
            refresh.ToolTip = "Обновить историю (F5)";
            search.Properties.NullValuePrompt = "Файл, документ или ошибка";
            search.Properties.ShowNullValuePrompt = ShowNullValuePromptOptions.EmptyValue | ShowNullValuePromptOptions.NullValue | ShowNullValuePromptOptions.EditorFocused;
            search.Properties.Buttons.Clear();
            search.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Delete) { Visible = false, ToolTip = "Очистить поиск" });
            search.ButtonClick += (_, __) => { search.EditValue = null; search.Focus(); };
            kindFilter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            statusFilter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            kindFilter.Properties.Items.AddRange(new object[] { "Все операции", "Импорт", "Выгрузка" });
            statusFilter.Properties.Items.Add("Все результаты");
            foreach (FileOperationStatus status in Enum.GetValues(typeof(FileOperationStatus)))
                statusFilter.Properties.Items.Add(new StatusChoice(status));
            kindFilter.SelectedIndex = 0;
            statusFilter.SelectedIndex = 0;

            SetButtonIcon(refresh, "xaf/action_refresh");
            SetButtonIcon(retry, "scheduling/import");
            SetButtonIcon(openFolder, "actions/open");
            SetButtonIcon(copy, "edit/copy");
            operations.Controls.Add(grid);
            operations.AppearanceCaption.FontSizeDelta = 1;
            operations.AppearanceCaption.FontStyleDelta = FontStyle.Bold;
            var detailGroup = new GroupControl
            {
                Dock = DockStyle.Fill,
                Text = "Подробности операции",
                Padding = new Padding(10, 8, 10, 10)
            };
            detailGroup.LookAndFeel.ParentLookAndFeel = LookAndFeel;
            detailGroup.AppearanceCaption.FontSizeDelta = 1;
            detailGroup.AppearanceCaption.FontStyleDelta = FontStyle.Bold;
            detailGroup.Controls.Add(details);
            split.Panel1.Controls.Add(operations);
            split.Panel2.Controls.Add(detailGroup);
            split.SizeChanged += (_, __) => ArrangePanels();

            var close = new SimpleButton { AutoSize = true, Text = "Закрыть", DialogResult = DialogResult.Cancel };
            close.LookAndFeel.ParentLookAndFeel = LookAndFeel;
            var layout = new TablePanel { Dock = DockStyle.Fill, Padding = new Padding(16, 13, 16, 9) };
            layout.Columns.Add(new TablePanelColumn(TablePanelEntityStyle.Relative, 1));
            layout.Rows.AddRange(new[]
            {
                new TablePanelRow(TablePanelEntityStyle.Absolute, 43),
                new TablePanelRow(TablePanelEntityStyle.Relative, 1),
                new TablePanelRow(TablePanelEntityStyle.Absolute, 46)
            });
            var filters = CreateFilters();
            var commands = CreateCommands(close);
            layout.Controls.AddRange(new Control[] { filters, split, commands });
            layout.SetCell(filters, 0, 0);
            layout.SetCell(split, 1, 0);
            layout.SetCell(commands, 2, 0);
            Controls.Add(layout);
            CancelButton = close;
            ActiveControl = grid;

            search.EditValueChanged += (_, __) =>
            {
                search.Properties.Buttons[0].Visible = !string.IsNullOrEmpty(search.Text);
                ApplyFilters();
            };
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
            InitializeContextMenus();
            ShowSelection();
        }

        private void InitializeContextMenus()
        {
            var manager = new BarManager { Form = this };
            var rowMenu = new PopupMenu { Manager = manager };
            var emptyMenu = new PopupMenu { Manager = manager };
            var retryLink = rowMenu.AddItem(CreateContextItem(manager, retry));
            var folderLink = rowMenu.AddItem(CreateContextItem(manager, openFolder));
            var copyLink = rowMenu.AddItem(CreateContextItem(manager, copy));
            var refreshItem = CreateContextItem(manager, refresh);
            rowMenu.AddItem(refreshItem).BeginGroup = true;
            emptyMenu.AddItem(refreshItem);

            GridContextMenus.Attach(view, hasRow =>
            {
                refreshItem.Enabled = refresh.Enabled;
                retryLink.Visible = retry.Enabled;
                folderLink.Visible = openFolder.Enabled;
                copyLink.Visible = copy.Enabled;
                return hasRow ? rowMenu : emptyMenu;
            });
            Disposed += (_, __) =>
            {
                rowMenu.Dispose();
                emptyMenu.Dispose();
                manager.Dispose();
            };
        }

        private static BarButtonItem CreateContextItem(BarManager manager, SimpleButton button)
        {
            var item = new BarButtonItem(manager, button.Text);
            item.ImageOptions.SvgImage = button.ImageOptions.SvgImage;
            item.ItemClick += (_, __) => button.PerformClick();
            return item;
        }

        private FileOperationEntry SelectedEntry => (view.GetFocusedRow() as HistoryRow)?.Entry;

        internal static DevExpress.Utils.Svg.SvgImage HistoryIcon =>
            ImageResourceCache.Default.GetSvgImage("svgimages/outlook%20inspired/list.svg");

        private static void SetButtonIcon(SimpleButton button, string resource)
        {
            button.ImageOptions.SvgImage = ImageResourceCache.Default.GetSvgImage($"svgimages/{resource}.svg");
            button.ImageOptions.SvgImageSize = new Size(16, 16);
        }

        private void ConfigureGrid()
        {
            view.OptionsBehavior.Editable = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowIndicator = false;
            view.OptionsView.ShowVerticalLines = DefaultBoolean.False;
            view.OptionsSelection.EnableAppearanceFocusedCell = false;
            view.OptionsView.ColumnAutoWidth = true;
            view.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold;
            view.Appearance.Row.FontSizeDelta = 1;
            view.RowHeight = 34;
            view.ColumnPanelRowHeight = 34;
            var date = view.Columns.AddVisible(nameof(HistoryRow.OccurredAt), "Дата и время");
            date.DisplayFormat.FormatType = FormatType.DateTime;
            date.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            date.Width = date.MinWidth = 142;
            date.OptionsColumn.FixedWidth = true;
            var kind = view.Columns.AddVisible(nameof(HistoryRow.Kind), "Операция");
            kind.Width = kind.MinWidth = 94;
            kind.OptionsColumn.FixedWidth = true;
            var file = view.Columns.AddVisible(nameof(HistoryRow.FileName), "Файл");
            file.Width = 250;
            file.MinWidth = 124;
            var status = view.Columns.AddVisible(nameof(HistoryRow.Status), "Результат");
            status.Width = status.MinWidth = 136;
            status.OptionsColumn.FixedWidth = true;
            var version = view.Columns.AddVisible(nameof(HistoryRow.Version), "Схема");
            version.Width = version.MinWidth = 68;
            version.OptionsColumn.FixedWidth = true;
            view.Columns.AddField(nameof(HistoryRow.DocumentName)).Caption = "Документ";
            view.Columns.AddField(nameof(HistoryRow.Summary)).Caption = "Подробности";
            view.FocusedRowChanged += (_, __) => ShowSelection();
            view.RowCellStyle += (_, e) =>
            {
                if (SystemInformation.HighContrast || e.Column != status || e.RowHandle == view.FocusedRowHandle ||
                    !(view.GetRow(e.RowHandle) is HistoryRow row)) return;
                var color = StatusColor(row.Entry.Status);
                if (color.IsEmpty) return;
                e.Appearance.ForeColor = color;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            };
            view.CustomDrawEmptyForeground += (_, e) =>
            {
                if (view.RowCount > 0) return;
                var message = loading ? "Загрузка истории…" : entries.Count == 0
                    ? "Операции с файлами пока не записаны."
                    : "По выбранным условиям ничего не найдено.";
                TextRenderer.DrawText(e.Graphics, message, grid.Font, e.Bounds, grid.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                e.Handled = true;
            };
        }

        private static Color StatusColor(FileOperationStatus status)
        {
            switch (status)
            {
                case FileOperationStatus.Completed: return Color.FromArgb(22, 111, 75);
                case FileOperationStatus.Warning: return Color.FromArgb(126, 76, 10);
                case FileOperationStatus.Rejected:
                case FileOperationStatus.Failed: return Color.FromArgb(163, 44, 58);
                default: return Color.Empty;
            }
        }

        private TablePanel CreateFilters()
        {
            var filters = new TablePanel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 8) };
            filters.Columns.AddRange(new[]
            {
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 50),
                new TablePanelColumn(TablePanelEntityStyle.Relative, 1),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 153),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 171),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 115)
            });
            filters.Rows.Add(new TablePanelRow(TablePanelEntityStyle.Relative, 1));
            var label = new LabelControl { Text = "&Поиск", Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 10, 0), AllowHtmlString = false };
            label.LookAndFeel.ParentLookAndFeel = LookAndFeel;
            search.Margin = new Padding(0, 0, 9, 0);
            kindFilter.Margin = new Padding(0, 0, 9, 0);
            statusFilter.Margin = new Padding(0, 0, 9, 0);
            refresh.Margin = Padding.Empty;
            refresh.MinimumSize = new Size(0, 30);
            var fields = new Control[] { label, search, kindFilter, statusFilter, refresh };
            filters.Controls.AddRange(fields);
            for (var column = 0; column < fields.Length; column++) filters.SetCell(fields[column], 0, column);
            return filters;
        }

        private TablePanel CreateCommands(SimpleButton close)
        {
            var commands = new TablePanel { Dock = DockStyle.Fill, Margin = new Padding(0, 9, 0, 0) };
            commands.Columns.AddRange(new[]
            {
                new TablePanelColumn(TablePanelEntityStyle.Relative, 1),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 94)
            });
            commands.Rows.Add(new TablePanelRow(TablePanelEntityStyle.Relative, 1));
            var actions = new StackPanel
            {
                Dock = DockStyle.Fill, LayoutDirection = StackPanelLayoutDirection.LeftToRight, Margin = Padding.Empty
            };
            foreach (var button in new[] { retry, openFolder, copy })
            {
                button.Padding = new Padding(5, 0, 5, 0);
                button.MinimumSize = new Size(0, 32);
                button.Margin = new Padding(0, 0, 8, 0);
                actions.Controls.Add(button);
            }
            close.MinimumSize = new Size(94, 32);
            close.Margin = Padding.Empty;
            close.Dock = DockStyle.Fill;
            commands.Controls.AddRange(new Control[] { actions, close });
            commands.SetCell(actions, 0, 0);
            commands.SetCell(close, 0, 1);
            return commands;
        }

        private void ArrangePanels()
        {
            if (split.Width == 0 || split.Height == 0) return;
            var wide = split.Width >= ScaleLogical(950);
            if (arranged && split.Horizontal == wide) return;
            arranged = true;
            split.Horizontal = wide;
            split.Panel1.MinSize = ScaleLogical(wide ? 590 : 145);
            split.Panel2.MinSize = ScaleLogical(wide ? 285 : 165);
            split.SplitterPosition = wide ? (int)(split.Width * .39) : (int)(split.Height * .55);
        }

        private int ScaleLogical(int value) => (int)Math.Round(value * DeviceDpi / 96d);

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                search.Focus();
                search.SelectAll();
                return true;
            }
            if (keyData == Keys.F5)
            {
                refresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async Task RefreshEntriesAsync()
        {
            if (loading) return;
            loading = true;
            refresh.Enabled = false;
            UseWaitCursor = true;
            grid.Invalidate();
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
                    grid.Invalidate();
                }
            }
        }

        private void ApplyFilters()
        {
            var text = search.Text.Trim();
            var filter = (statusFilter.SelectedItem as StatusChoice)?.Value;
            var kind = kindFilter.SelectedIndex == 1 ? FileOperationKind.Import
                : kindFilter.SelectedIndex == 2 ? (FileOperationKind?)FileOperationKind.Export : null;
            var previous = SelectedEntry;
            var rows = entries.Where(entry =>
                    (!kind.HasValue || entry.Kind == kind) && (!filter.HasValue || entry.Status == filter) &&
                    Matches(entry, text))
                .Select(entry => new HistoryRow(entry)).ToList();
            grid.DataSource = rows;
            operations.Text = rows.Count == entries.Count ? $"Операции ({rows.Count})" : $"Операции ({rows.Count} из {entries.Count})";
            if (previous != null)
            {
                var previousIndex = rows.FindIndex(row => ReferenceEquals(row.Entry, previous));
                if (previousIndex >= 0) view.FocusedRowHandle = view.GetRowHandle(previousIndex);
            }
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
                .AppendLine(entry.Summary)
                .AppendLine();
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
