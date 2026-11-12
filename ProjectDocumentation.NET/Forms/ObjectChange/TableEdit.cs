using Core.NET.AutoModels.ExplanatoryNoteObj;
using DevExpress.Utils;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class TableEdit : XtraForm
    {
        private readonly DataTable cells = new DataTable();
        private readonly Dictionary<DataRow, Dictionary<DataColumn, tCell>> originals =
            new Dictionary<DataRow, Dictionary<DataColumn, tCell>>();
        private readonly HashSet<DataRow> addedRows = new HashSet<DataRow>();
        private readonly ComboBoxEdit alignment = new ComboBoxEdit { Width = 132, AccessibleName = "Выравнивание выбранной ячейки" };
        private DataRow header;
        private int nextColumnId;
        private bool refreshingAlignment;

        public TableEdit()
        {
            InitializeComponent();
            var actions = new StackPanel
            {
                Dock = DockStyle.Top, LayoutDirection = StackPanelLayoutDirection.TopDown,
                AutoSize = true, AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink, Padding = new Padding(4)
            };
            alignment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            alignment.Properties.Items.AddRange(new[] { "По умолчанию", "По левому краю", "По правому краю", "По центру" });
            alignment.ToolTip = "Выравнивание выбранной ячейки";
            alignment.SelectedIndexChanged += (_, __) => ChangeAlignment();
            actions.Controls.Add(ActionRow(TableButton("Добавить столбец", () => AddColumn(true)), TableButton("Добавить строку", AddRow)));
            actions.Controls.Add(ActionRow(TableButton("Удалить столбец", DeleteColumn), TableButton("Удалить строку", DeleteRow), alignment));
            Controls.Add(actions);
            layoutControlTextBlock.BringToFront();
            btnSave.AllowFocus = true;
            btnClose.AllowFocus = true;
            gridView.OptionsCustomization.AllowSort = false;
            gridView.OptionsCustomization.AllowFilter = false;
            gridView.OptionsCustomization.AllowGroup = false;
            gridView.OptionsCustomization.AllowColumnMoving = false;
            gridView.OptionsView.ShowIndicator = true;
            gridView.IndicatorWidth = 85;
            gridView.CustomDrawRowIndicator += (_, e) =>
            {
                if (e.RowHandle >= 0) e.Info.DisplayText = e.RowHandle == 0 ? "Заголовок" : e.RowHandle.ToString();
            };
            gridView.FocusedRowChanged += (_, __) => RefreshAlignment();
            gridView.FocusedColumnChanged += (_, __) => RefreshAlignment();
            gridView.RowCellStyle += (_, e) =>
            {
                var row = gridView.GetDataRow(e.RowHandle);
                var column = cells.Columns[e.Column.FieldName];
                var cell = GetOriginalCell(row, column);
                if (cell?.alignSpecified != true) return;
                e.Appearance.Options.UseTextOptions = true;
                switch (cell.align)
                {
                    case tAlignTypes.left:
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
                        break;
                    case tAlignTypes.right:
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
                        break;
                    case tAlignTypes.center:
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                        break;
                }
            };
        }

        public TableEdit(tTable tTable) : this()
        {
            Table = tTable;
        }

        private static SimpleButton TableButton(string name, Action handler)
        {
            var button = new SimpleButton { Text = name, Width = 115 };
            button.Click += (_, __) => handler();
            return button;
        }

        private static StackPanel ActionRow(params Control[] controls)
        {
            var row = new StackPanel
            {
                LayoutDirection = StackPanelLayoutDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            };
            row.Controls.AddRange(controls);
            return row;
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnSave_Click(object sender, EventArgs e)
        {
            gridView.PostEditor();
            gridView.UpdateCurrentRow();
            var columns = cells.Columns.Cast<DataColumn>().ToArray();
            var rows = cells.Rows.Cast<DataRow>().Skip(1)
                .Where(row => addedRows.Contains(row) || columns.Any(column => HasCell(row, column))).ToList();
            var body = new List<tCell>();
            for (var index = 0; index < rows.Count; index++)
            {
                var row = rows[index];
                var width = index < rows.Count - 1 || addedRows.Contains(row) ? columns.Length
                    : Array.FindLastIndex(columns, column => HasCell(row, column)) + 1;
                for (var column = 0; column < width; column++) body.Add(ReadCell(row, columns[column]));
            }

            var hasHeader = columns.Length > 1 || columns.Any(column => HasCell(header, column));
            Table = new tTable
            {
                TitleRow = hasHeader ? columns.Select(column => ReadCell(header, column)).ToList() : Table?.TitleRow,
                Row = body.Count > 0 || Table?.Row != null ? body : null
            };
            IsSave = true;
            Close();
        }

        private void ImageEdit_Load(object sender, EventArgs e)
        {
            var headers = Table?.TitleRow;
            var body = Table?.Row;
            // The legacy format only stores the flattened cells. Without headers, one column preserves their order.
            var width = Math.Max(1, headers?.Count ?? 0);
            for (var index = 0; index < width; index++) AddColumn(false);
            header = cells.NewRow();
            cells.Rows.Add(header);
            if (headers != null)
            {
                for (var index = 0; index < headers.Count; index++) LoadCell(header, cells.Columns[index], headers[index]);
            }
            if (body != null)
            {
                for (var offset = 0; offset < body.Count; offset += width)
                {
                    var row = cells.NewRow();
                    cells.Rows.Add(row);
                    for (var index = 0; index < width && offset + index < body.Count; index++)
                        LoadCell(row, cells.Columns[index], body[offset + index]);
                }
            }
            if (cells.Rows.Count == 1) cells.Rows.Add(cells.NewRow());
            gridControl.DataSource = cells;
            RefreshColumns();
        }

        private void AddColumn(bool refresh)
        {
            if (refresh) gridView.PostEditor();
            cells.Columns.Add("Column" + ++nextColumnId, typeof(string));
            if (refresh) RefreshColumns();
        }

        private void AddRow()
        {
            gridView.PostEditor();
            var row = cells.NewRow();
            addedRows.Add(row);
            cells.Rows.Add(row);
            gridView.FocusedRowHandle = gridView.GetRowHandle(cells.Rows.Count - 1);
        }

        private void DeleteRow()
        {
            var row = gridView.GetFocusedDataRow();
            if (row == null || ReferenceEquals(row, header)) return;
            gridView.CloseEditor();
            originals.Remove(row);
            addedRows.Remove(row);
            cells.Rows.Remove(row);
        }

        private void DeleteColumn()
        {
            var field = gridView.FocusedColumn?.FieldName;
            if (cells.Columns.Count <= 1 || field == null || !cells.Columns.Contains(field)) return;
            gridView.CloseEditor();
            var column = cells.Columns[field];
            foreach (var row in originals.Values) row.Remove(column);
            cells.Columns.Remove(column);
            RefreshColumns();
        }

        private void RefreshColumns()
        {
            gridView.Columns.Clear();
            gridView.PopulateColumns();
            for (var index = 0; index < cells.Columns.Count; index++)
                gridView.Columns[cells.Columns[index].ColumnName].Caption = "Столбец " + (index + 1);
            RefreshAlignment();
        }

        private void RefreshAlignment()
        {
            var row = gridView.GetFocusedDataRow();
            var column = gridView.FocusedColumn == null ? null : cells.Columns[gridView.FocusedColumn.FieldName];
            var cell = GetOriginalCell(row, column);
            refreshingAlignment = true;
            try
            {
                alignment.Enabled = row != null && column != null;
                alignment.SelectedIndex = cell?.alignSpecified == true ? (int)cell.align + 1 : 0;
            }
            finally
            {
                refreshingAlignment = false;
            }
        }

        private void ChangeAlignment()
        {
            if (refreshingAlignment || !alignment.Enabled || alignment.SelectedIndex < 0) return;
            var row = gridView.GetFocusedDataRow();
            var column = gridView.FocusedColumn == null ? null : cells.Columns[gridView.FocusedColumn.FieldName];
            if (row == null || column == null) return;
            gridView.PostEditor();
            var cell = ReadCell(row, column) ?? new tCell();
            cell.alignSpecified = alignment.SelectedIndex > 0;
            if (cell.alignSpecified) cell.align = (tAlignTypes)(alignment.SelectedIndex - 1);
            LoadCell(row, column, cell);
            gridView.RefreshRow(gridView.FocusedRowHandle);
        }

        private tCell GetOriginalCell(DataRow row, DataColumn column)
        {
            return row != null && column != null && originals.TryGetValue(row, out var columns) &&
                columns.TryGetValue(column, out var cell) ? cell : null;
        }

        private void LoadCell(DataRow row, DataColumn column, tCell cell)
        {
            if (!originals.TryGetValue(row, out var columns))
            {
                columns = new Dictionary<DataColumn, tCell>();
                originals[row] = columns;
            }
            columns[column] = cell;
            if (cell?.Value != null) row[column] = cell.Value;
        }

        private bool HasCell(DataRow row, DataColumn column)
            => !row.IsNull(column) || originals.TryGetValue(row, out var columns) && columns.ContainsKey(column);

        private tCell ReadCell(DataRow row, DataColumn column)
        {
            tCell original = null;
            var hadCell = originals.TryGetValue(row, out var columns) && columns.TryGetValue(column, out original);
            var value = row.IsNull(column) ? null : (string)row[column];
            if (hadCell && original == null && value == null) return null;
            return new tCell { Value = value, align = original?.align ?? default, alignSpecified = original?.alignSpecified ?? false };
        }

        public bool IsSave { get; private set; }
        public tTable Table { get; private set; }
    }
}
