using Core.NET.AutoModels.ConclusionObj;
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
        public List<TMismatch> _tMismatches = new List<TMismatch>();

        public ExpertEngineeringSurveyEdit()
        {
            InitializeComponent();
        }

        public ExpertEngineeringSurveyEdit(TExpertEngineeringSurveys TExpertEngineeringSurveys) : this()
        {
            ExpertEngineeringSurvey = TExpertEngineeringSurveys;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnMismatchesAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tMismatches.Add(new TMismatch());
            gridControlMismatches.DataSource = _tMismatches.ToArray();
            gridControlMismatches.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnMismatchesDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewMismatches.GetRow(gridViewMismatches.FocusedRowHandle) is TMismatch obj)
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
                ExpertEngineeringSurvey.EngineeringSurveyType = ConclusionEdit.GetEnumByAttributeValue<TEngineeringSurveyType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbEngineeringSurveyType,
                    message: "Заполните поле вид инженерных изысканий")?.Value);
                //ExpertEngineeringSurvey.EngineeringSurveyConditions = ConclusionEdit.GetStringValue(memoEngineeringSurveyConditions.EditValue,
                //    memoEngineeringSurveyConditions, "Заполните поле.", isNullAble: true);
                //ExpertEngineeringSurvey.EngineeringSurveyProgramNote = ConclusionEdit.GetStringValue(memoEngineeringSurveyProgramNote.EditValue,
                //    memoEngineeringSurveyProgramNote, "Заполните поле.", isNullAble: true);
                //ExpertEngineeringSurvey.EngineeringSurveyMethods = ConclusionEdit.GetStringValue(memoEngineeringSurveyMethods.EditValue,
                //    memoEngineeringSurveyMethods, "Заполните поле.", isNullAble: true);
                //ExpertEngineeringSurvey.EngineeringSurveyChanges = ConclusionEdit.GetStringValue(memoEngineeringSurveyChanges.EditValue,
                //    memoEngineeringSurveyChanges, "Заполните поле.", isNullAble: true);
                if (_tMismatches.Count > 0)
                {
                    foreach (var item in _tMismatches)
                    {
                        ExpertEngineeringSurvey.Mismatches.Add(item);
                    }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbEngineeringSurveyType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType).ToList());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewMismatches_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuMismatches.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private async void Obj_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (ExpertEngineeringSurvey is null)
            {
                ExpertEngineeringSurvey = new TExpertEngineeringSurveys();
            }

            cmbEngineeringSurveyType.EditValue = cmbEngineeringSurveyType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType)
                    ?.FirstOrDefault(f => f.Value == ExpertEngineeringSurvey?.EngineeringSurveyType.GetEnumXmlAttributeValue().ToString());
            //memoEngineeringSurveyConditions.EditValue = ExpertEngineeringSurvey.EngineeringSurveyConditions;
            //memoEngineeringSurveyProgramNote.EditValue = ExpertEngineeringSurvey.EngineeringSurveyProgramNote;
            //memoEngineeringSurveyMethods.EditValue = ExpertEngineeringSurvey.EngineeringSurveyMethods;
            //memoEngineeringSurveyChanges.EditValue = ExpertEngineeringSurvey.EngineeringSurveyChanges;
            _tMismatches = ExpertEngineeringSurvey.Mismatches?.ToList() ?? new List<TMismatch>();
            gridControlMismatches.DataSource = _tMismatches;
            foreach (GridColumn column in gridViewMismatches.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewMismatches.Columns[nameof(TMismatch.Summary)] is GridColumn gridColumnSummary)
            {
                gridColumnSummary.Caption = "Вывод о несоответствии";
                gridColumnSummary.Visible = true;
            }
            if (gridViewMismatches.Columns[nameof(TMismatch.Part)] is GridColumn gridColumnPart)
            {
                gridColumnPart.Caption = "Ссылка на материалы";
                gridColumnPart.Visible = true;
            }
            if (gridViewMismatches.Columns[nameof(TMismatch.Link)] is GridColumn gridColumnLink)
            {
                gridColumnLink.Caption = "Ссылка на конкретное требование";
                gridColumnLink.Visible = true;
            }
        }

        private Session Session => SessionController.GetSession();
        public TExpertEngineeringSurveys ExpertEngineeringSurvey { get; private set; }
        public bool IsSave { get; private set; }
    }
}