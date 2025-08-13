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
        public TableEdit()
        {
            InitializeComponent();
        }

        public TableEdit(tTable tTable) : this()
        {
            Table = tTable;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        private List<List<T>> GetDivideList<T>(List<T> source, int chunkSize)
        {
            var dividedLists = new List<List<T>>();

            for (int i = 0; i < source.Count; i += chunkSize)
            {
                List<T> chunk = source.Skip(i).Take(chunkSize).ToList();
                dividedLists.Add(chunk);
            }

            return dividedLists;
        }

        private void ImageEdit_Load(object sender, EventArgs e)
        {
            if (Table is null)
            {
                Table = new tTable();
            }

            var dataTable = new DataTable();

            if (Table.TitleRow?.Count > 0)
            {
                foreach (var item in Table.TitleRow)
                {
                    var column = new DataColumn();

                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        column.Caption = item.Value;
                    }

                    dataTable.Columns.Add(column);
                }
            }

            var divideList = GetDivideList(Table.Row, Table.TitleRow.Count);
            foreach (var list in divideList)
            {
                var row = dataTable.NewRow();

                for (int i = 0; i < list.Count; i++)
                {
                    row[i] = list[i].Value;
                }

                dataTable.Rows.Add(row);
            }

            gridControl.DataSource = dataTable;
            gridView.RefreshData();
        }

        public bool IsSave { get; private set; }
        public tTable Table { get; private set; }
    }
}