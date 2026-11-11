using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExpertProjectDocumentsEdit : XtraForm
    {
        public List<MismatchObj> _mismatchObjS = new List<MismatchObj>();

        public ExpertProjectDocumentsEdit()
        {
            InitializeComponent();
            GridContextMenus.Attach(gridViewProjectDocumentsMismatches, popupMenuMismatches, barBtnAdd);

            richEditControlProjectDocumentsReview.ActiveViewType = RichEditViewType.Simple;
            richEditControlProjectDocumentsChanges.ActiveViewType = RichEditViewType.Simple;
            richEditControlDangerSolutions.ActiveViewType = RichEditViewType.Simple;
        }

        public ExpertProjectDocumentsEdit(TExpertProjectDocuments TExpertProjectDocuments) : this()
        {
            ExpertProjectDocuments = TExpertProjectDocuments;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mismatchObjS.Add(new MismatchObj());
            gridControlProjectDocumentsMismatches.DataSource = _mismatchObjS.ToArray();
            gridControlProjectDocumentsMismatches.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsMismatches.GetRow(gridViewProjectDocumentsMismatches.FocusedRowHandle) is MismatchObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _mismatchObjS.Remove(obj);
                    gridControlProjectDocumentsMismatches.DataSource = _mismatchObjS.ToArray();
                    gridControlProjectDocumentsMismatches.RefreshDataSource();
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
                gridViewProjectDocumentsMismatches.PostEditor();
                gridViewProjectDocumentsMismatches.UpdateCurrentRow();
                ExpertProjectDocuments.ExpertType = ConclusionEdit.GetEnumByAttributeValue<TExpertType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExpertType,
                    message: "Заполните поле направление деятельности в области экспертизы проектной документации.")?.Value);

                //ExpertProjectDocuments.ProjectDocumentsReview = ConclusionEdit.GetStringValue(richEditControlProjectDocumentsReview.Text, richEditControlProjectDocumentsReview, "Заполните поле описание основных решений (мероприятий), принятых в проектной документации.", isNullAble: true);
                //ExpertProjectDocuments.ProjectDocumentsChanges = ConclusionEdit.GetStringValue(richEditControlProjectDocumentsChanges.Text, richEditControlProjectDocumentsChanges, "Заполните поле сведения об оперативных изменениях.", isNullAble: true);

                //ExpertProjectDocuments.ProjectDocumentsReview = null;
                //ExpertProjectDocuments.ProjectDocumentsChanges = null;

                ExpertProjectDocuments.DangerSolutions = ConclusionEdit.GetStringValue(richEditControlDangerSolutions.Text, richEditControlDangerSolutions, "Заполните поле сведения о решениях о решениях, которые которые в случае их реализации могли привести к риску возникновения аварийных ситуаций, гибели людей, причинения значительного материального ущерба и которые в ходе проведения государственной экспертизы были приведены в соответствие с установленными требованиями.", isNullAble: true);

                if (string.IsNullOrWhiteSpace(ExpertProjectDocuments.DangerSolutions))
                {
                    ExpertProjectDocuments.DangerSolutions = null;
                }

                var dangerMismatch = memoDangerMismatch.Text;
                ExpertProjectDocuments.Mismatches = null;
                if (_mismatchObjS.Count > 0 || !string.IsNullOrWhiteSpace(dangerMismatch))
                {
                    var mismatches = new TProjectDocumentsMismatches
                    {
                        DangerMismatch = string.IsNullOrWhiteSpace(dangerMismatch) ? null : dangerMismatch
                    };
                    foreach (var row in _mismatchObjS)
                    {
                        var item = new TMismatch { Summary = row.Summary, Link = row.Link, Part = row.Part };
                        switch (row.Type)
                        {
                            case MismatchObjType.EngineeringSurveyMismatch:
                                mismatches.EngineeringSurveyMismatch.Add(item);
                                break;
                            case MismatchObjType.ProjectTaskMismatch:
                                mismatches.ProjectTaskMismatch.Add(item);
                                break;
                            case MismatchObjType.NormsMismatch:
                                mismatches.NormsMismatch.Add(item);
                                break;
                        }
                    }

                    ExpertProjectDocuments.Mismatches = mismatches;
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

            cmbExpertType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType).ToList());
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (ExpertProjectDocuments is null)
            {
                ExpertProjectDocuments = new TExpertProjectDocuments();
            }

            cmbExpertType.EditValue = cmbExpertType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType)
                    ?.FirstOrDefault(f => f.Value == ExpertProjectDocuments?.ExpertType.GetEnumXmlAttributeValue().ToString());

            //richEditControlProjectDocumentsReview.Text = ExpertProjectDocuments.ProjectDocumentsReview;
            //richEditControlProjectDocumentsChanges.Text = ExpertProjectDocuments.ProjectDocumentsChanges;
            richEditControlDangerSolutions.Text = ExpertProjectDocuments.DangerSolutions;

            if (ExpertProjectDocuments.Mismatches?.EngineeringSurveyMismatch?.Count() > 0)
            {
                _mismatchObjS.AddRange(ExpertProjectDocuments.Mismatches?.EngineeringSurveyMismatch
                    ?.Select(s => new MismatchObj(MismatchObjType.EngineeringSurveyMismatch, s)));
            }
            if (ExpertProjectDocuments.Mismatches?.ProjectTaskMismatch?.Count() > 0)
            {
                _mismatchObjS.AddRange(ExpertProjectDocuments.Mismatches?.ProjectTaskMismatch
                    ?.Select(s => new MismatchObj(MismatchObjType.ProjectTaskMismatch, s)));
            }
            if (ExpertProjectDocuments.Mismatches?.NormsMismatch?.Count() > 0)
            {
                _mismatchObjS.AddRange(ExpertProjectDocuments.Mismatches?.NormsMismatch
                    ?.Select(s => new MismatchObj(MismatchObjType.NormsMismatch, s)));
            }

            gridControlProjectDocumentsMismatches.DataSource = _mismatchObjS;
            var memoEdit = gridControlProjectDocumentsMismatches.RepositoryItems.Add(nameof(MemoEdit)) as RepositoryItemMemoEdit;
            foreach (GridColumn column in gridViewProjectDocumentsMismatches.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentsMismatches.Columns[nameof(MismatchObj.Summary)] is GridColumn gridColumnSummary)
            {
                gridColumnSummary.Caption = "Вывод о несоответствии";
                gridColumnSummary.Visible = true;
                gridColumnSummary.ColumnEdit = memoEdit;
            }
            if (gridViewProjectDocumentsMismatches.Columns[nameof(MismatchObj.Part)] is GridColumn gridColumnPart)
            {
                gridColumnPart.Caption = "Ссылка на материалы";
                gridColumnPart.Visible = true;
                gridColumnPart.ColumnEdit = memoEdit;
            }
            if (gridViewProjectDocumentsMismatches.Columns[nameof(MismatchObj.Link)] is GridColumn gridColumnLink)
            {
                gridColumnLink.Caption = "Ссылка на конкретное требование";
                gridColumnLink.Visible = true;
                gridColumnLink.ColumnEdit = memoEdit;
            }
            if (gridViewProjectDocumentsMismatches.Columns[nameof(MismatchObj.Type)] is GridColumn gridColumnType)
            {
                gridColumnType.Caption = "Тип";
                gridColumnType.Visible = true;
                gridColumnType.OptionsColumn.FixedWidth = true;
                gridColumnType.Width = 250;
            }

            memoDangerMismatch.EditValue = ExpertProjectDocuments.Mismatches?.DangerMismatch;
        }

        private Session Session => SessionController.GetSession();
        public TExpertProjectDocuments ExpertProjectDocuments { get; private set; }
        public bool IsSave { get; private set; }

        public class MismatchObj
        {
            public MismatchObj() { }

            public MismatchObj(MismatchObjType type)
            {
                Type = type;
            }

            public MismatchObj(MismatchObjType type, TMismatch TMismatch) : this(type)
            {
                Summary = TMismatch.Summary;
                Part = TMismatch.Part;
                Link = TMismatch.Link;
            }

            public string Link { get; set; }
            public string Part { get; set; }
            public string Summary { get; set; }
            public MismatchObjType Type { get; set; }
        }

        public enum MismatchObjType
        {
            /// <summary>
            /// Сведения о несоответствии проектной документации результату инженерных изысканий.
            /// </summary>
            [Description("Сведения о несоответствии проектной документации результату инженерных изысканий")]
            EngineeringSurveyMismatch,

            /// <summary>
            /// Сведения о несоответствии проектной документации заданию на проектирование.
            /// </summary>
            [Description("Сведения о несоответствии проектной документации заданию на проектирование")]
            ProjectTaskMismatch,

            /// <summary>
            /// Сведения о несоответствии проектной документации требованиям технических регламентов.
            /// </summary>
            [Description("Сведения о несоответствии проектной документации требованиям технических регламентов")]
            NormsMismatch
        }
    }
}