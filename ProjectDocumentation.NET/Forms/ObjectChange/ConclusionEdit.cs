using Core.NET.AutoModels;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ConclusionEdit : XtraForm
    {
        private Conclusion _conclusion;
        private List<tExpert> _experts = new List<tExpert>();

        public ConclusionEdit()
        {
            InitializeComponent();
        }

        public ConclusionEdit(Conclusion conclusion) : this()
        {
            _conclusion = conclusion;
        }

        private void ConclusionEdit_Load(object sender, EventArgs e)
        {
            if (_conclusion is null)
            {
                _conclusion = new Conclusion();
            }
            else
            {
                cmbExaminationResult.SelectedIndex = (int?)_conclusion.ExaminationObject?.ExaminationResult - 1 ?? -1;
                cmbExaminationForm.SelectedIndex = (int?)_conclusion.ExaminationObject?.ExaminationForm - 1 ?? -1;
                cmbExaminationObjectType.SelectedIndex = (int?)_conclusion.ExaminationObject?.ExaminationObjectType - 1 ?? -1;
                cmbExaminationStage.SelectedIndex = (int?)_conclusion.ExaminationObject?.ExaminationStage - 1 ?? -1;
                cmbConstructionType.SelectedIndex = (int?)_conclusion.ExaminationObject?.ConstructionType - 1 ?? -1;
                cmbExaminationType.SelectedIndex = (int?)_conclusion.ExaminationObject?.ExaminationType?.FirstOrDefault() - 1 ?? -1;
                memoName.Text = _conclusion.ExaminationObject?.Name;
                memoExaminationStageNote.Text = _conclusion.ExaminationObject?.ExaminationStageNote;

                _experts.AddRange(_conclusion.Experts);
            }
            
            gridControlExpert.DataSource = _experts;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _conclusion.ExaminationObject = new tExaminationObject();
            _conclusion.ExaminationObject.ExaminationResult = (tExaminationResult)cmbExaminationResult.SelectedIndex + 1;
            _conclusion.ExaminationObject.ExaminationForm = (tExaminationForm)cmbExaminationForm.SelectedIndex + 1;
            _conclusion.ExaminationObject.ExaminationObjectType = (tExaminationObjectType)cmbExaminationObjectType.SelectedIndex + 1;
            _conclusion.ExaminationObject.ExaminationStage = (tExaminationStage)cmbExaminationStage.SelectedIndex + 1;
            _conclusion.ExaminationObject.ConstructionType = (tConstractionType)cmbConstructionType.SelectedIndex + 1;            
            _conclusion.ExaminationObject.ExaminationType = new tExaminationType[] { (tExaminationType)cmbExaminationType.SelectedIndex + 1 };
            _conclusion.ExaminationObject.Name = memoName.Text;
            _conclusion.ExaminationObject.ExaminationStageNote = memoExaminationStageNote.Text;

            _conclusion.Experts = _experts.ToArray();



            var ss = _conclusion.SerializeObject();
        }


        private void gridViewExpert_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuExpert.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnExpertAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new ExpretEdit())
            {
                form.ShowDialog();
                if (form.IsSave && form.Expert != null)
                {
                    _experts.Add(form.Expert);
                }
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnExpertEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tExpert expert)
            {
                using (var form = new ExpretEdit(expert))
                {
                    form.ShowDialog();
                };

                gridControlExpert.RefreshDataSource();
            }
        }

        private void barBtnExpertDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tExpert expert)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _experts.Remove(expert);
                }
            }
        }
    }
}