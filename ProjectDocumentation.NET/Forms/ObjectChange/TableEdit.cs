using Core.NET.AutoModels.ExplanatoryNoteObj;
using DevExpress.XtraEditors;
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
        private DataRow header;
        private int nextColumnId;

        public TableEdit()
        {
            InitializeComponent();
            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            actions.Controls.Add(TableButton("Добавить столбец", () => AddColumn(true)));
            actions.Controls.Add(TableButton("Добавить строку", AddRow));
            actions.Controls.Add(TableButton("Удалить столбец", DeleteColumn));
            actions.Controls.Add(TableButton("Удалить строку", DeleteRow));
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

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnSave_Click(object sender, EventArgs e)
        {
            gridView.PostEditor();
            gridView.UpdateCurrentRow();
            var columns = cells.Columns.Cast<DataColumn>().ToArray();
            var rows = cells.Rows.Cast<DataRow>().Skip(1)
                .Where(row => columns.Any(column => HasCell(row, column))).ToList();
            var body = new List<tCell>();
            for (var index = 0; index < rows.Count; index++)
            {
                var row = rows[index];
                var width = index < rows.Count - 1 ? columns.Length
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
            cells.Rows.Add(cells.NewRow());
            gridView.FocusedRowHandle = gridView.GetRowHandle(cells.Rows.Count - 1);
        }

        private void DeleteRow()
        {
            var row = gridView.GetFocusedDataRow();
            if (row == null || ReferenceEquals(row, header)) return;
            gridView.CloseEditor();
            originals.Remove(row);
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
