using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExpertEngineeringSurveyEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tExpertEngineeringSurveys ExpertEngineeringSurvey { get; private set; }
        public List<tMismatch> _tMismatches = new List<tMismatch>();

        public ExpertEngineeringSurveyEdit()
        {
            InitializeComponent();
        }

        public ExpertEngineeringSurveyEdit(tExpertEngineeringSurveys tExpertEngineeringSurveys) : this()
        {
            ExpertEngineeringSurvey = tExpertEngineeringSurveys;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            cmbEngineeringSurveyType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType).ToList());
        }

        private async void Obj_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (ExpertEngineeringSurvey is null)
            {
                ExpertEngineeringSurvey = new tExpertEngineeringSurveys();
            }

            cmbEngineeringSurveyType.EditValue = cmbEngineeringSurveyType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType)
                    ?.FirstOrDefault(f => f.Value == ExpertEngineeringSurvey?.EngineeringSurveyType.GetEnumXmlAttributeValue().ToString());
            memoEngineeringSurveyConditions.EditValue = ExpertEngineeringSurvey.EngineeringSurveyConditions;
            memoEngineeringSurveyProgramNote.EditValue = ExpertEngineeringSurvey.EngineeringSurveyProgramNote;
            memoEngineeringSurveyMethods.EditValue = ExpertEngineeringSurvey.EngineeringSurveyMethods;
            memoEngineeringSurveyChanges.EditValue = ExpertEngineeringSurvey.EngineeringSurveyChanges;
            _tMismatches = ExpertEngineeringSurvey.Mismatches?.ToList() ?? new List<tMismatch>();
            gridControlMismatches.DataSource = _tMismatches;
            foreach (GridColumn column in gridViewMismatches.Columns)
            {
                column.Visible = false;
            }
            if (gridViewMismatches.Columns[nameof(tMismatch.Summary)] is GridColumn gridColumnSummary)
            {
                gridColumnSummary.Caption = "Вывод о несоответствии";
                gridColumnSummary.Visible = true;
            }
            if (gridViewMismatches.Columns[nameof(tMismatch.Part)] is GridColumn gridColumnPart)
            {
                gridColumnPart.Caption = "Ссылка на материалы";
                gridColumnPart.Visible = true;
            }
            if (gridViewMismatches.Columns[nameof(tMismatch.Link)] is GridColumn gridColumnLink)
            {
                gridColumnLink.Caption = "Ссылка на конкретное требование";
                gridColumnLink.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ExpertEngineeringSurvey.EngineeringSurveyType = ConclusionEdit.GetEnumByAttributeValue<tEngineeringSurveyType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbEngineeringSurveyType,
                    message: "Заполните поле вид инженерных изысканий")?.Value);
                ExpertEngineeringSurvey.EngineeringSurveyConditions = ConclusionEdit.GetStringValue(memoEngineeringSurveyConditions.EditValue,
                    memoEngineeringSurveyConditions, "Заполните поле.", isNullAble: true);
                ExpertEngineeringSurvey.EngineeringSurveyProgramNote = ConclusionEdit.GetStringValue(memoEngineeringSurveyProgramNote.EditValue,
                    memoEngineeringSurveyProgramNote, "Заполните поле.", isNullAble: true);
                ExpertEngineeringSurvey.EngineeringSurveyMethods = ConclusionEdit.GetStringValue(memoEngineeringSurveyMethods.EditValue,
                    memoEngineeringSurveyMethods, "Заполните поле.", isNullAble: true);
                ExpertEngineeringSurvey.EngineeringSurveyChanges = ConclusionEdit.GetStringValue(memoEngineeringSurveyChanges.EditValue,
                    memoEngineeringSurveyChanges, "Заполните поле.", isNullAble: true);
                if (_tMismatches.Count > 0)
                {
                    ExpertEngineeringSurvey.Mismatches = _tMismatches.ToArray();
                }
                else
                {
                    ExpertEngineeringSurvey.Mismatches = null;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonEdit_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }

        private void gridViewMismatches_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuMismatches.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnMismatchesAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tMismatches.Add(new tMismatch());
            gridControlMismatches.DataSource = _tMismatches.ToArray();
            gridControlMismatches.RefreshDataSource();
        }

        private void barBtnMismatchesDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewMismatches.GetRow(gridViewMismatches.FocusedRowHandle) is tMismatch obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tMismatches.Remove(obj);
                    gridControlMismatches.DataSource = _tMismatches.ToArray();
                    gridControlMismatches.RefreshDataSource();
                }
            }
        }
    }
}