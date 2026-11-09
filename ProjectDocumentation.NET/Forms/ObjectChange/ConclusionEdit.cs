using Core.NET.AutoModels;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using ProjectDocumentation.NET.Models.ConclusionObj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ConclusionEdit : XtraForm
    {
        private List<CadastralNumber> _cadastralNumbers = new List<CadastralNumber>();
        private List<TExpert> _experts = new List<TExpert>();
        private List<TMismatch> _mismatch = new List<TMismatch>();
        private List<TDesigner> _tDesigners = new List<TDesigner>();
        private List<TDocument> _tDocuments = new List<TDocument>();
        private List<TEngineeringSurveyAddress> _tEngineeringSurveyAddresses = new List<TEngineeringSurveyAddress>();
        private List<TDeclarant> _tEngineeringSurveyDeveloper = new List<TDeclarant>();
        private List<TTechnicalCustomer> _tEngineeringSurveyTechnicalCustomer = new List<TTechnicalCustomer>();
        private List<TExpertEngineeringSurveys> _tExpertEngineeringSurveys = new List<TExpertEngineeringSurveys>();
        private List<TExpertProjectDocuments> _tExpertProjectDocuments = new List<TExpertProjectDocuments>();
        private List<TFinance> _tFinances = new List<TFinance>();
        private List<TObjectPart> _tObjectParts = new List<TObjectPart>();
        private List<TPreviousConclusion> _tPreviousConclusions = new List<TPreviousConclusion>();
        private List<TDeclarant> _tProjectDocumentsDevelopers = new List<TDeclarant>();
        private List<TTechnicalCustomer> _tProjectDocumentsTechnicalCustomer = new List<TTechnicalCustomer>();
        private List<Ttei> _Tteis = new List<Ttei>();
        private bool _isEepdUseInForm;

        public ConclusionEdit()
        {
            InitializeComponent();
        }

        public ConclusionEdit(Conclusion conclusion) : this()
        {
            Conclusion = conclusion;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnCadastralNumberAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _cadastralNumbers.Add(new CadastralNumber());
            gridControlCadastralNumber.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnCadastralNumberDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewCadastralNumber.GetRow(gridViewCadastralNumber.FocusedRowHandle) is CadastralNumber obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _cadastralNumbers.Remove(obj);
                    gridControlCadastralNumber.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDesignersAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DesignerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tDesigners.Add(form.Designer);
                gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
                gridControlDesigners.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDesignersDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDesigners.GetRow(gridViewDesigners.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TDesigner TDesigner)
                    {
                        _tDesigners.Remove(TDesigner);
                        gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
                        gridControlDesigners.RefreshDataSource();
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDesignersEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDesigners.GetRow(gridViewDesigners.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DesignerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners?.ToArray())?.ToArray();
                    gridControlDesigners.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DocumentEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tDocuments.Add(form.Document);
                gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjS(_tDocuments).ToArray();
                gridControlDocuments.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDocuments.GetRow(gridViewDocuments.FocusedRowHandle) is DocumentObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tDocuments.Remove(obj.Document);
                    gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjS(_tDocuments).ToArray();
                    gridControlDocuments.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewDocuments.GetRow(gridViewDocuments.FocusedRowHandle) is DocumentObj obj)
            {
                var form = new DocumentEdit(obj.Document);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjS(_tDocuments).ToArray();
                    gridControlDocuments.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyAddressAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new EngineeringSurveyAddressEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyAddresses.Add(form.EngineeringSurveyAddress);
                gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                gridControlEngineeringSurveyAddress.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyAddressDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyAddress.GetRow(gridViewEngineeringSurveyAddress.FocusedRowHandle) is EngineeringSurveyAddress obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tEngineeringSurveyAddresses.Remove(obj.EngineeringSurveyAddressObj);
                    gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                    gridControlEngineeringSurveyAddress.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyAddressEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyAddress.GetRow(gridViewEngineeringSurveyAddress.FocusedRowHandle) is EngineeringSurveyAddress obj)
            {
                var form = new EngineeringSurveyAddressEdit(obj.EngineeringSurveyAddressObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
                    gridControlEngineeringSurveyAddress.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DeclarantEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyDeveloper.Add(form.Declarant);
                gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tEngineeringSurveyDeveloper).ToArray();
                gridControlEngineeringSurveyDeveloper.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyDeveloperDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyDeveloper.GetRow(gridViewEngineeringSurveyDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TDeclarant TDeclarant)
                    {
                        _tEngineeringSurveyDeveloper.Remove(TDeclarant);
                        gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tEngineeringSurveyDeveloper).ToArray();
                        gridControlEngineeringSurveyDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyDeveloper.GetRow(gridViewEngineeringSurveyDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DeclarantEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tEngineeringSurveyDeveloper).ToArray();
                    gridControlEngineeringSurveyDeveloper.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new TechnicalCustomerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tEngineeringSurveyTechnicalCustomer.Add(form.TechnicalCustomer);
                gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyTechnicalCustomerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyTechnicalCustomer.GetRow(gridViewEngineeringSurveyTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TTechnicalCustomer TTechnicalCustomer)
                    {
                        _tEngineeringSurveyTechnicalCustomer.Remove(TTechnicalCustomer);
                        gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                        gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnEngineeringSurveyTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewEngineeringSurveyTechnicalCustomer.GetRow(gridViewEngineeringSurveyTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                var form = new TechnicalCustomerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
                    gridControlEngineeringSurveyTechnicalCustomer.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new ExpertEdit())
            {
                form.ShowDialog();
                if (form.IsSave && form.Expert != null)
                {
                    _experts.Add(form.Expert);
                }
            }
            ;

            gridControlExpert.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is TExpert expert)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is TExpert expert)
            {
                using (var form = new ExpertEdit(expert))
                {
                    form.ShowDialog();
                }
                ;

                gridControlExpert.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertEngineeringSurveysAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ExpertEngineeringSurveyEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tExpertEngineeringSurveys.Add(form.ExpertEngineeringSurvey);
                gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                gridControlExpertEngineeringSurveys.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertEngineeringSurveysDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertEngineeringSurveys.GetRow(gridViewExpertEngineeringSurveys.FocusedRowHandle) is ExpertEngineeringSurvey obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tExpertEngineeringSurveys.Remove(obj.ExpertEngineeringSurveys);
                    gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                    gridControlExpertEngineeringSurveys.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertEngineeringSurveysEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertEngineeringSurveys.GetRow(gridViewExpertEngineeringSurveys.FocusedRowHandle) is ExpertEngineeringSurvey obj)
            {
                var form = new ExpertEngineeringSurveyEdit(obj.ExpertEngineeringSurveys);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
                    gridControlExpertEngineeringSurveys.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertProjectDocumentsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ExpertProjectDocumentsEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tExpertProjectDocuments.Add(form.ExpertProjectDocuments);
                gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                gridControlExpertProjectDocuments.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertProjectDocumentsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertProjectDocuments.GetRow(gridViewExpertProjectDocuments.FocusedRowHandle) is ExpertProjectDocument obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tExpertProjectDocuments.Remove(obj.ExpertProjectDocuments);
                    gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                    gridControlExpertProjectDocuments.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnExpertProjectDocumentsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpertProjectDocuments.GetRow(gridViewExpertProjectDocuments.FocusedRowHandle) is ExpertProjectDocument obj)
            {
                var form = new ExpertProjectDocumentsEdit(obj.ExpertProjectDocuments);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
                    gridControlExpertProjectDocuments.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnFinanceAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new FundingInformationEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tFinances.Add(form.Finance);
                gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                gridControlFinances.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnFinanceDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewFinances.GetRow(gridViewFinances.FocusedRowHandle) is FinanceObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tFinances.Remove(obj.TFinance);
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnFinanceEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewFinances.GetRow(gridViewFinances.FocusedRowHandle) is FinanceObj obj)
            {
                var form = new FundingInformationEdit(obj.TFinance);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnMismatchesAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            }
            ;

            gridControlExpert.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnMismatchesDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is TMismatch TMismatch)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _mismatch.Remove(TMismatch);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnMismatchesEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            }

            gridControlExpert.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnObjectPartAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ObjPartEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tObjectParts.Add(form.ObjectPart);
                gridControlObjectPart.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnObjectPartDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is TObjectPart obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tObjectParts.Remove(obj);
                    gridControlObjectPart.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnObjectPartEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is TObjectPart obj)
            {
                var form = new ObjPartEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlObjectPart.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnPreviousConclusionsAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new PreviousConclusionEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tPreviousConclusions.Add(form.PreviousConclusion);
                gridControlPreviousConclusions.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnPreviousConclusionsDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewPreviousConclusions.GetRow(gridViewPreviousConclusions.FocusedRowHandle) is TPreviousConclusion obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tPreviousConclusions.Remove(obj);
                    gridControlPreviousConclusions.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnPreviousConclusionsEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewPreviousConclusions.GetRow(gridViewPreviousConclusions.FocusedRowHandle) is TPreviousConclusion obj)
            {
                var form = new PreviousConclusionEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlPreviousConclusions.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsDeveloperAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new DeclarantEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDocumentsDevelopers.Add(form.Declarant);
                gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tProjectDocumentsDevelopers).ToArray();
                gridControlProjectDocumentsDeveloper.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsDeveloperDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsDeveloper.GetRow(gridViewProjectDocumentsDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TDeclarant TDeclarant)
                    {
                        _tProjectDocumentsDevelopers.Remove(TDeclarant);
                        gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tProjectDocumentsDevelopers).ToArray();
                        gridControlProjectDocumentsDeveloper.RefreshDataSource();
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsDeveloperEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsDeveloper.GetRow(gridViewProjectDocumentsDeveloper.FocusedRowHandle) is PersonObj obj)
            {
                var form = new DeclarantEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tProjectDocumentsDevelopers).ToArray();
                    gridControlProjectDocumentsDeveloper.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsTechnicalCustomerAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new TechnicalCustomerEdit();
            form.ShowDialog();
            if (form.IsSave)
            {
                _tProjectDocumentsTechnicalCustomer.Add(form.TechnicalCustomer);
                gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsTechnicalCustomerDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsTechnicalCustomer.GetRow(gridViewProjectDocumentsTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj.BaseObj is TTechnicalCustomer TTechnicalCustomer)
                    {
                        _tProjectDocumentsTechnicalCustomer.Remove(TTechnicalCustomer);
                        gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                        gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnProjectDocumentsTechnicalCustomerEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewProjectDocumentsTechnicalCustomer.GetRow(gridViewProjectDocumentsTechnicalCustomer.FocusedRowHandle) is PersonObj obj)
            {
                var form = new TechnicalCustomerEdit(obj.BaseObj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
                    gridControlProjectDocumentsTechnicalCustomer.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnTEIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _Tteis.Add(new Ttei());
            gridControlTEI.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnTEIDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTEI.GetRow(gridViewTEI.FocusedRowHandle) is Ttei obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _Tteis.Remove(obj);
                    gridControlTEI.RefreshDataSource();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnAddress_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    AddressEdit form;
                    if (buttonEdit.EditValue is TAddress address)
                    {
                        form = new AddressEdit(address);
                    }
                    else
                    {
                        form = new AddressEdit();
                    }

                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Address;
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var addressData = await DaDataObj.GetAddressAsync(obj);
                        if (addressData != null)
                        {
                            var address = new TAddress()
                            {
                                Country = addressData.country,
                                District = addressData.city_district,
                                City = addressData.city,
                                Settlement = addressData.settlement,
                                Street = addressData.street,
                                Building = addressData.house,
                                Room = addressData.flat,
                                Note = addressData.result,
                            };

                            try
                            {
                                var region = ConclusionEdit.GetEnumByAttributeValue<TRegionsRf>(addressData.region_kladr_id.GetKladrRegionCode(), isNullAble: false);
                                address.Region = region;
                            }
                            catch (Exception)
                            {
                                XtraMessageBox.Show("Регион не определен, введите вручную",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            }

                            buttonEdit.EditValue = address;
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnDeclarant_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    DeclarantEdit form;
                    if (buttonEdit.EditValue is TDeclarant TDeclarant)
                    {
                        form = new DeclarantEdit(TDeclarant);
                    }
                    else
                    {
                        form = new DeclarantEdit();
                    }
                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Declarant;
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            FileName = txtFileName.Text;

            try
            {
                var schemaVersion = txtVersion.EditValue?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(schemaVersion))
                {
                    schemaVersion = "01.03";
                }
                Conclusion.SchemaVersion = schemaVersion;

                #region Page I
                Conclusion.ExaminationObject = new TExaminationObject
                {
                    ExaminationResult = GetEnumByAttributeValue<TExaminationResult>(GetValueGeneralizedDictionary(
                        cmbExaminationResult,
                        message: "Заполните поле результат экспертизы")?.Value),
                    ExaminationForm = GetEnumByAttributeValue<TExaminationForm>(GetValueGeneralizedDictionary(
                        cmbExaminationForm,
                        message: "Заполните поле формы экспертизы")?.Value),
                    ExaminationObjectType = GetEnumByAttributeValue<TExaminationObjectType>(GetValueGeneralizedDictionary(
                        cmbExaminationObjectType,
                        message: "Заполните поле вида объекта экспертизы")?.Value),
                    ExaminationStage = GetEnumByAttributeValue<TExaminationStage>(GetValueGeneralizedDictionary(
                        cmbExaminationStage,
                        message: "Заполните поле вид экспертизы")?.Value),
                    ConstructionType = GetEnumByAttributeValue<TConstractionType>(GetValueGeneralizedDictionary(
                        cmbConstructionType,
                        message: "Заполните поле вид работ")?.Value),
                    Name = GetStringValue(memoName.Text, memoName, "Заполните поле наименование объекта экспертизы")
                };

                var examinationType = GetCheckComboBoxItems<TExaminationType>(
                    checkCmbExaminationType,
                    "Заполните поле предмет экспертизы").ToArray();
                foreach (var item in examinationType)
                {
                    Conclusion.ExaminationObject.ExaminationType.Add(item);
                }

                //TODO: ЧТО ЗА ХУЙНЯ
                //if (cmbProjectDocumentationIM.EditValue is GeneralizedDictionaryXPO valueProjectDocumentationIM)
                //{
                //    Conclusion.ExaminationObject.ProjectDocumentationIMSpecified = true;

                //    if (valueProjectDocumentationIM.Value == TYesNo.да.GetEnumDescription())
                //    {
                //        Conclusion.ExaminationObject.ProjectDocumentationIM = TYesNo.да;
                //    }
                //    else
                //    {
                //        Conclusion.ExaminationObject.ProjectDocumentationIM = TYesNo.нет;
                //    }
                //}
                //else
                //{
                //    Conclusion.ExaminationObject.ProjectDocumentationIMSpecified = false;
                //}

                //if (cmbEngineeringSurveysIM.EditValue is GeneralizedDictionaryXPO valueEngineeringSurveysIM)
                //{
                //    Conclusion.ExaminationObject.EngineeringSurveysIMSpecified = true;

                //    if (valueEngineeringSurveysIM.Value == TYesNo.да.GetEnumDescription())
                //    {
                //        Conclusion.ExaminationObject.EngineeringSurveysIM = TYesNo.да;
                //    }
                //    else
                //    {
                //        Conclusion.ExaminationObject.EngineeringSurveysIM = TYesNo.нет;
                //    }
                //}
                //else
                //{
                //    Conclusion.ExaminationObject.EngineeringSurveysIMSpecified = false;
                //}

                Conclusion.ExaminationObject.ExaminationStageNote = GetStringValue(memoExaminationStageNote.Text, isNullAble: true);
                //Conclusion.ExpertiseResultNote = GetStringValue(memoExpertiseResultNote.Text, isNullAble: true) ?? string.Empty;
                #endregion

                #region Page I_2
                Conclusion.ExpertOrganization = new TOrganization
                {
                    OrgFullName = GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации"),
                    OrgOgrn = GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации"),
                    OrgKpp = GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации"),
                    OrgInn = GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации"),
                    Email = GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true)
                };
                if (btnAddress.EditValue is TAddress address)
                {
                    Conclusion.ExpertOrganization.Address = address;
                }
                else
                {
                    //btnAddress.Focus();
                    //XtraMessageBox.Show("Заполните адрес организации",
                    //    "Информационное сообщение",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Error);
                    //return;
                }
                Conclusion.Approver = new TWorkPerson
                {
                    FamilyName = GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию подписанта"),
                    FirstName = GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя подписанта"),
                    SecondName = GetStringValue(txtSecondName.EditValue, txtSecondName, "Заполните отчество подписанта"),
                    Position = GetStringValue(txtPosition.EditValue, txtPosition, "Заполните должность подписанта")
                };
                #endregion

                #region Page I_3
                if (btnDeclarant.EditValue is TDeclarant TDeclarant)
                {
                    Conclusion.Declarant = TDeclarant;
                }
                else
                {
                    //XtraMessageBox.Show("Укажите сведения о заявителе.", "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //btnDeclarant.Focus();
                    //return;
                }
                #endregion

                #region Page I_5
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.Documents.Clear();
                if (_tDocuments.Count > 0)
                {
                    foreach (var item in _tDocuments)
                    {
                        Conclusion.Documents.Add(item);
                    }
                }
                #endregion

                #region Page 1_6
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.PreviousConclusions.Clear();
                if (_tPreviousConclusions.Count > 0)
                {
                    foreach (var item in _tPreviousConclusions)
                    {
                        Conclusion.PreviousConclusions.Add(item);
                    }
                }
                #endregion

                #region Page 1_7
                Conclusion.EcologyExpertise = new TEcologyExpertise();
                Conclusion.EcologyExpertise.NeedExpertise = GetEnumByAttributeValue<TYesNo>(GetValueGeneralizedDictionary(cmbEcologyExpertiseNeedExpertise,
                    message: "Заполните поле требуется ли экологическая экспертиза")?.Value);
                Conclusion.EcologyExpertise.Comment = GetStringValue(memoEcologyExpertiseNote.EditValue,
                    memoEcologyExpertiseNote, "Заполните поле обоснование необходимости экологической экспертизы", isNullAble: true) ?? string.Empty;
                #endregion

                #region Page II_1
                Conclusion.Object = new TObject
                {
                    Name = GetStringValue(memoObjectName.Text, memoObjectName, "Заполните поле наименование объекта капитального строительства"),
                    Type = GetEnumByAttributeValue<TObjectType>(GetValueGeneralizedDictionary(
                        cmbObjectType,
                        message: "Заполните поле вид объекта капитального строительства")?.Value),
                    FunctionsClass = GetStringValue(cmbFunctionsClass.EditValue?.ToString(), cmbFunctionsClass, "Функциональное назначение объекта капитального строительства (код по классификатору)")
                };

                //Conclusion.FunctionsClass = GetStringValue(cmbFunctionsClass.EditValue?.ToString(), cmbFunctionsClass, "Функциональное назначение объекта капитального строительства (код по классификатору)");

                if (cmbObjectAddress.EditValue is TAddress objectAddress)
                {
                    Conclusion.Object.Address.Add(objectAddress);
                }
                else
                {
                    //XtraMessageBox.Show("Укажите адрес объекта.", "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cmbObjectAddress.Focus();
                    //return;
                }
                if (_Tteis.Count > 0)
                {
                    foreach (var item in _Tteis)
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                        {
                            //XtraMessageBox.Show("Наименование технико-экономического показателя не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Measure))
                        {
                            //XtraMessageBox.Show("Единица измерения технико-экономического показателя не может быть пустой.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Value))
                        {
                            //XtraMessageBox.Show("Значение технико-экономического показателя не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //gridViewTEI.Focus();
                            //return;
                        }
                    }

                    foreach (var item in _Tteis)
                    {
                        Conclusion.Object.Tei.Add(item);
                    }
                }

                if (_tObjectParts.Count > 0)
                {
                    foreach (var item in _tObjectParts)
                    {
                        Conclusion.Object.ObjectPart.Add(item);
                    }
                }
                #endregion

                #region Page II_2
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.Finance.Clear();
                if (_tFinances.Count > 0)
                {
                    foreach (var item in _tFinances)
                    {
                        Conclusion.Finance.Add(item);
                    }
                }
                Conclusion.FinanceComment = GetStringValue(memoFinanceComment.EditValue, isNullAble: true);
                #endregion

                #region Page II_3
                Conclusion.ClimateConditions = new TClimateConditions();

                var climateDistrict = GetCheckComboBoxItems<TClimateDistrict>(checkCmbClimateDistrict, isNullAble: true).ToArray();
                foreach (var item in climateDistrict)
                {
                    Conclusion.ClimateConditions.ClimateDistrict.Add(item);
                }

                var seismicActivity = GetCheckComboBoxItems<TSeismicActivity>(checkCmbSeismicActivity, isNullAble: true).ToArray();
                foreach (var item in seismicActivity)
                {
                    Conclusion.ClimateConditions.SeismicActivity.Add(item);
                }

                var windDistrict = GetCheckComboBoxItems<TWindDistrict>(checkCmbWindDistrict, isNullAble: true).ToArray();
                foreach (var item in windDistrict)
                {
                    Conclusion.ClimateConditions.WindDistrict.Add(item);
                }

                var geologicalConditions = GetCheckComboBoxItems<TGeologicalConditions>(checkCmbGeologicalConditions, isNullAble: true).ToArray();
                foreach (var item in geologicalConditions)
                {
                    Conclusion.ClimateConditions.GeologicalConditions.Add(item);
                }

                var snowDistrict = GetCheckComboBoxItems<TSnowDistrict>(checkCmbSnowDistrict, isNullAble: true).ToArray();
                foreach (var item in snowDistrict)
                {
                    Conclusion.ClimateConditions.SnowDistrict.Add(item);
                }

                Conclusion.ClimateConditionsNote = GetStringValue(memoClimateConditionsNote.EditValue, memoClimateConditionsNote, "Заполните поле дополнительных сведений о природных и техногенных условиях");
                #endregion

                #region Page II_4
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.Designer.Clear();
                if (_tDesigners.Count > 0)
                {
                    foreach (var item in _tDesigners)
                    {
                        Conclusion.Designer.Add(item);
                    }
                }
                #endregion

                #region Page II_5

                //var eEPDUse = Conclusion.EepdUse?.FirstOrDefault();
                //if (eEPDUse != null)
                //{
                //    var number = eEPDUse.EEPDNumber;
                //    if (number != null)
                //    {
                //        checkEEPDUse.EditValue = true;
                //        switch (number.ItemElementName)
                //        {
                //            case ItemChoiceType.EGRZ:
                //                radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                //                break;

                //            case ItemChoiceType.noEGRZ:
                //                radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                //                break;
                //        }
                //        txtEEPDNumber.EditValue = number.Item;
                //        dateEEPDDate.EditValue = eEPDUse.EEPDDate;
                //        memoEEPDNote.EditValue = eEPDUse.EEPDNote;
                //    }
                //}

                // Форма показывает только первую запись EepdUse: заменяем её, остальные записи из файла сохраняем.
                if (_isEepdUseInForm && Conclusion.EepdUse.Count > 0)
                {
                    Conclusion.EepdUse.RemoveAt(0);
                }
                _isEepdUseInForm = false;

                if (checkEEPDUse.Checked)
                {
                    var eEPDUse = new TeepdUse
                    {
                        EepdDate = dateEEPDDate.DateTime.Date,
                        EepdNote = GetStringValue(memoEEPDNote.EditValue, memoEEPDNote, "Заполните поле сведения о разделах представленной на экспертизу проектной документации"),
                        EepdNumber = new TConclusionNumber()
                    };

                    var number = new TConclusionNumber();
                    var itemChoiceType = (ItemChoiceType)radioGroupConclusionNumber.SelectedIndex;
                    switch (itemChoiceType)
                    {
                        case ItemChoiceType.EGRZ:
                            number.Egrz = ConclusionEdit.GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *");
                            break;

                        case ItemChoiceType.noEGRZ:
                            number.NoEgrz = ConclusionEdit.GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *");
                            break;
                    }

                    eEPDUse.EepdNumber = number;
                    Conclusion.EepdUse.Insert(0, eEPDUse);
                    _isEepdUseInForm = true;
                }
                #endregion

                #region Page II_6
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.CadastralNumber.Clear();
                if (_cadastralNumbers?.Count() > 0)
                {
                    var cadastralNumbers = new List<string>();
                    foreach (var obj in _cadastralNumbers)
                    {
                        if (string.IsNullOrWhiteSpace(obj.Number))
                        {
                            //gridControlCadastralNumber.Focus();
                            //XtraMessageBox.Show(
                            //    "Кадастровый номер не может быть пустым.",
                            //    "Информационное сообщение",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error);
                            //return;
                        }
                        else
                        {
                            cadastralNumbers.Add(obj.Number);
                        }
                    }

                    foreach (var item in cadastralNumbers)
                    {
                        Conclusion.CadastralNumber.Add(item);
                    }
                }
                #endregion

                #region Page II_7

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.ProjectDocumentsDeveloper.Clear();
                if (_tProjectDocumentsDevelopers?.Count() > 0)
                {
                    foreach (var item in _tProjectDocumentsDevelopers)
                    {
                        Conclusion.ProjectDocumentsDeveloper.Add(item);
                    }
                }

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.ProjectDocumentsTechnicalCustomer.Clear();
                if (_tProjectDocumentsTechnicalCustomer?.Count() > 0)
                {
                    foreach (var item in _tProjectDocumentsTechnicalCustomer)
                    {
                        Conclusion.ProjectDocumentsTechnicalCustomer.Add(item);
                    }
                }

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.EngineeringSurveyDeveloper.Clear();
                if (_tEngineeringSurveyDeveloper?.Count() > 0)
                {
                    foreach (var item in _tEngineeringSurveyDeveloper)
                    {
                        Conclusion.EngineeringSurveyDeveloper.Add(item);
                    }
                }

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.EngineeringSurveyTechnicalCustomer.Clear();
                if (_tEngineeringSurveyTechnicalCustomer?.Count() > 0)
                {
                    foreach (var item in _tEngineeringSurveyTechnicalCustomer)
                    {
                        Conclusion.EngineeringSurveyTechnicalCustomer.Add(item);
                    }
                }

                #endregion

                #region Page III
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.ExpertProjectDocuments.Clear();
                if (_tExpertProjectDocuments?.Count() > 0)
                {
                    foreach (var item in _tExpertProjectDocuments)
                    {
                        Conclusion.ExpertProjectDocuments.Add(item);
                    }
                }

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.EngineeringSurveyAddress.Clear();
                if (_tEngineeringSurveyAddresses?.Count() > 0)
                {
                    foreach (var item in _tEngineeringSurveyAddresses)
                    {
                        Conclusion.EngineeringSurveyAddress.Add(item);
                    }
                }

                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.ExpertEngineeringSurveys.Clear();
                if (_tExpertEngineeringSurveys?.Count() > 0)
                {
                    foreach (var item in _tExpertEngineeringSurveys)
                    {
                        Conclusion.ExpertEngineeringSurveys.Add(item);
                    }
                }
                #endregion

                #region Page V
                if (Conclusion.Summary is null)
                {
                    Conclusion.Summary = new TSummary();
                }

                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.EngineeringSurveySummary = GetOptionalEnum<TEngineeringSurveysSummary>(cmbEngineeringSurveysSummary, out bool engineeringSurveysSummarySpecified);
                Conclusion.Summary.EngineeringSurveySummarySpecified = engineeringSurveysSummarySpecified;
                
                Conclusion.Summary.EngineeringSurveySummaryDate = GetStringValue(memoEngineeringSurveySummaryDate.EditValue, memoEngineeringSurveySummaryDate, "Заполните поле сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы результатов инженерных изысканий)");

                var engineeringSurveyType = GetCheckComboBoxItems<TEngineeringSurveyType>(checkCmbEngineeringSurveyType, "Заполните поле вид инженерных изысканий, на соответствие которым проводилась экспертиза проектной документации").ToArray();
                Conclusion.Summary.EngineeringSurveyType.Clear();
                if (engineeringSurveyType.Count() > 0)
                {
                    foreach (var item in engineeringSurveyType)
                    {
                        Conclusion.Summary.EngineeringSurveyType.Add(item);
                    }
                }

                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.ProjectDocumentsSummary = GetOptionalEnum<TProjectDocumentationSummary>(cmbProjectDocumentationSummary, out bool projectDocumentationSummarySpecified);
                Conclusion.Summary.ProjectDocumentsSummarySpecified = projectDocumentationSummarySpecified;

                Conclusion.Summary.ProjectDocumentsSummaryDate = GetStringValue(memoProjectDocumentsSummaryDate.EditValue, memoProjectDocumentsSummaryDate, "Заполните поле сведения о дате, по состоянию на которую действовали требования, примененные в соответствии с частью 5.2 статьи 49 Градостроительного кодекса Российской Федерации (в части экспертизы проектной документации)");
                                
                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.EstimateNormsAndWorksSummary = GetOptionalEnum<TEstimateNormsAndWorksSummary>(cmbEstimateNormsAndWorksSummary, out bool estimateNormsAndWorksSummarySpecified);
                Conclusion.Summary.EstimateNormsAndWorksSummarySpecified = estimateNormsAndWorksSummarySpecified;

                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.EstimateSummary = GetOptionalEnum<TEstimateValidationSummary>(cmbEstimateSummary, out bool estimateSummarySpecified);
                Conclusion.Summary.EstimateSummarySpecified = estimateSummarySpecified;
                #endregion

                #region Page VI
                if (Conclusion.Summary is null)
                    Conclusion.Summary = new TSummary();

                if (Conclusion.Summary.ExaminationSummary is null)
                    Conclusion.Summary.ExaminationSummary = new TExaminationSummary();

                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.ExaminationSummary.EngineeringSurveysResultsSummary = GetOptionalEnum<TEngineeringSurveysResultsSummary>(cmbEngineeringSurveysResultsSummary, out bool engineeringSurveysResultsSummarySpecified);
                Conclusion.Summary.ExaminationSummary.EngineeringSurveysResultsSummarySpecified = engineeringSurveysResultsSummarySpecified;

                // Необязательный элемент схемы: без флага Specified значение не попадает в XML.
                Conclusion.Summary.ExaminationSummary.EstimateSummary = GetOptionalEnum<TEstimateValidationSummary>(cmbEstimateValidationSummary, out bool estimateValidationSummarySpecified);
                Conclusion.Summary.ExaminationSummary.EstimateSummarySpecified = estimateValidationSummarySpecified;

                Conclusion.Summary.EstimateSummary1315 =
                    GetStringValue(memoEstimateSummary1315.EditValue,
                                   memoEstimateSummary1315,
                                   "Вывод об оценке достоверности определения сметной стоимости (ПП РФ №1315)");

                // Блок выводов по ПД в схеме необязателен (например, экспертиза только изысканий):
                // если ни один вывод не выбран — блок не пишется; если выбран хотя бы один — обязательны все три.
                var isProjectDocumentsSummarySelected = new[]
                {
                    cmbProjectDocumentationEngineeringSurveysResultsSummary.EditValue,
                    cmbProjectDocumentationDesignAssignmentSummary.EditValue,
                    cmbProjectDocumentationTechnicalRequirementsSummary.EditValue
                }.Any(value => value is GeneralizedDictionaryXPO);

                if (isProjectDocumentsSummarySelected is false)
                {
                    Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary = null;
                }
                else
                {
                    if (Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary is null)
                        Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary = new TProjectDocumentsSummary();

                    Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary.ProjectDocumentationEngineeringSurveysResultsSummary =
                        GetEnumByAttributeValue<TProjectDocumentationEngineeringSurveysResultsSummary>(
                            GetValueGeneralizedDictionary(
                                cmbProjectDocumentationEngineeringSurveysResultsSummary,
                                message: "Заполните вывод о соответствии ПД результатам инженерных изысканий")?.Value);

                    Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary.ProjectDocumentationDesignAssignmentSummary =
                        GetEnumByAttributeValue<TProjectDocumentationDesignAssignmentSummary>(
                            GetValueGeneralizedDictionary(
                                cmbProjectDocumentationDesignAssignmentSummary,
                                message: "Заполните вывод о соответствии ПД заданию на проектирование")?.Value);

                    Conclusion.Summary.ExaminationSummary.ProjectDocumentsSummary.ProjectDocumentationTechnicalRequirementsSummary =
                        GetEnumByAttributeValue<TProjectDocumentationTechnicalRequirementsSummary>(
                            GetValueGeneralizedDictionary(
                                cmbProjectDocumentationTechnicalRequirementsSummary,
                                message: "Заполните вывод о соответствии ПД требованиям технических регламентов и иным требованиям")?.Value);
                }
                #endregion

                #region Page VII
                // Списки формы — единственный источник: очищаем коллекцию, иначе повторное сохранение дублирует записи.
                Conclusion.Experts.Clear();
                if (_experts.Count > 0)
                {
                    foreach (var item in _experts)
                    {
                        Conclusion.Experts.Add(item);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    ex.Message,
                    "Информационное сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnSearch_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();

                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var party = await DaDataObj.GetPartyAsync(obj);
                        var data = party?.suggestions?.FirstOrDefault()?.data;
                        if (data != null)
                        {
                            txtOrgFullName.EditValue = data.name?.full_with_opf ?? data.name?.full;
                            txtOrgOGRN.EditValue = data.ogrn;
                            txtOrgKPP.EditValue = data.kpp;
                            txtOrgINN.EditValue = data.inn;

                            var splitsSplitsName = data.management?.name?.Split();
                            if (splitsSplitsName?.Length >= 3)
                            {
                                txtFamilyName.EditValue = splitsSplitsName[0];
                                txtFirstName.EditValue = splitsSplitsName[1];
                                txtSecondName.EditValue = splitsSplitsName[2];
                            }
                            txtPosition.EditValue = data.management?.post;

                            var addressData = data.address?.data;
                            if (addressData != null)
                            {
                                var address = new TAddress()
                                {
                                    Country = addressData.country,
                                    District = addressData.city_district,
                                    City = addressData.city,
                                    Settlement = addressData.settlement,
                                    Street = addressData.street,
                                    Building = addressData.house,
                                    Room = addressData.flat,
                                    Note = addressData.result ?? data.address?.unrestricted_value,
                                };

                                try
                                {
                                    var region = ConclusionEdit.GetEnumByAttributeValue<TRegionsRf>(addressData.region_kladr_id.GetKladrRegionCode(), isNullAble: false);
                                    address.Region = region;
                                }
                                catch (Exception)
                                {
                                    XtraMessageBox.Show("Регион не определен, введите вручную",
                                        "Информационное сообщение",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                                }

                                btnAddress.EditValue = address;
                            }
                        }
                    }
                    else
                    {
                        btnSearch.Focus();
                        XtraMessageBox.Show("Введите ИНН или ОГРН организации",
                            "Информационное сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void checkEEPDUse_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckEdit checkEdit)
            {
                if (checkEdit.Checked)
                {
                    radioGroupConclusionNumber.SelectedIndex = 0;
                    layoutControlItemEEPDType.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDNumber.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDDate.Visibility = LayoutVisibility.Always;
                    layoutControlItemEEPDNote.Visibility = LayoutVisibility.Always;
                }
                else
                {
                    radioGroupConclusionNumber.SelectedIndex = -1;
                    layoutControlItemEEPDType.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDNumber.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDDate.Visibility = LayoutVisibility.Never;
                    layoutControlItemEEPDNote.Visibility = LayoutVisibility.Never;
                }
            }
        }

        private void ClearEditValue(object sender, ButtonPressedEventArgs e)
        {
            if (sender is CheckedComboBoxEdit checkedComboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    foreach (CheckedListBoxItem item in checkedComboBoxEdit.Properties.Items.Cast<CheckedListBoxItem>())
                    {
                        item.CheckState = CheckState.Unchecked;
                    }
                }
            }

            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void cmbObjectAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    AddressEdit form;
                    if (buttonEdit.EditValue is TAddress address)
                    {
                        form = new AddressEdit(address);
                    }
                    else
                    {
                        form = new AddressEdit();
                    }
                    form.ShowDialog();

                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.Address;
                        buttonEdit.Refresh();
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var addressData = await DaDataObj.GetAddressAsync(obj);
                        if (addressData != null)
                        {
                            var address = new TAddress()
                            {
                                Country = addressData.country,
                                District = addressData.area,
                                City = addressData.city,
                                Settlement = addressData.settlement,
                                Street = addressData.street,
                                Building = addressData.house,
                                Room = addressData.flat,
                                Note = addressData.result,
                            };

                            try
                            {
                                var region = GetEnumByAttributeValue<TRegionsRf>(addressData.region_kladr_id.GetKladrRegionCode(), isNullAble: false);
                                address.Region = region;
                            }
                            catch (Exception)
                            {
                                XtraMessageBox.Show("Регион не определен, введите вручную",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            }

                            buttonEdit.EditValue = address;
                        }
                    }
                }
            }
        }

        private async void ConclusionEdit_Load(object sender, EventArgs e)
        {
            txtFileName.EditValue = FileName;

            await FillFormControllersAsync();

            if (Conclusion is null)
            {
                Conclusion = new Conclusion();
            }

            txtVersion.EditValue = Conclusion.SchemaVersion;

            #region Page I
            cmbExaminationResult.EditValue = cmbExaminationResult.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationResult.GetEnumXmlAttributeValueInt());

            cmbExaminationForm.EditValue = cmbExaminationForm.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationForm)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationForm.GetEnumXmlAttributeValueInt());

            cmbExaminationObjectType.EditValue = cmbExaminationObjectType.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType)
                ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationObjectType.GetEnumXmlAttributeValueInt());

            cmbExaminationStage.EditValue = cmbExaminationStage.Properties.Items
               ?.Cast<GeneralizedDictionaryXPO>()
               ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationStage)
               ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ExaminationStage.GetEnumXmlAttributeValueInt());

            cmbConstructionType.EditValue = cmbConstructionType.Properties.Items
              ?.Cast<GeneralizedDictionaryXPO>()
              ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType)
              ?.FirstOrDefault(f => f.ValueInt == Conclusion?.ExaminationObject?.ConstructionType.GetEnumXmlAttributeValueInt());

            SetCheckComboBoxByItems(Conclusion?.ExaminationObject?.ExaminationType, checkCmbExaminationType);
            memoName.Text = Conclusion?.ExaminationObject?.Name;

            //if (Conclusion?.ExaminationObject?.ProjectDocumentationIMSpecified is true)
            //{
            //    cmbProjectDocumentationIM.EditValue = cmbProjectDocumentationIM.Properties.Items
            //      ?.Cast<GeneralizedDictionaryXPO>()
            //      ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
            //      ?.FirstOrDefault(f => f.Value == Conclusion?.ExaminationObject?.ProjectDocumentationIM.GetEnumXmlAttributeValue().ToString());
            //}

            //if (Conclusion?.ExaminationObject?.EngineeringSurveysIMSpecified is true)
            //{
            //    cmbEngineeringSurveysIM.EditValue = cmbEngineeringSurveysIM.Properties.Items
            //        ?.Cast<GeneralizedDictionaryXPO>()
            //        ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
            //        ?.FirstOrDefault(f => f.Value == Conclusion?.ExaminationObject?.EngineeringSurveysIM.GetEnumXmlAttributeValue().ToString());
            //}
            memoExaminationStageNote.Text = Conclusion?.ExaminationObject?.ExaminationStageNote;
            #endregion

            #region Page I_2
            btnSearch.EditValue = Conclusion?.ExpertOrganization?.OrgInn;
            txtOrgFullName.EditValue = Conclusion?.ExpertOrganization?.OrgFullName;
            txtOrgOGRN.EditValue = Conclusion?.ExpertOrganization?.OrgOgrn;
            txtOrgKPP.EditValue = Conclusion?.ExpertOrganization?.OrgKpp;
            txtOrgINN.EditValue = Conclusion?.ExpertOrganization?.OrgInn;
            txtEmail.EditValue = Conclusion?.ExpertOrganization?.Email;
            btnAddress.EditValue = Conclusion?.ExpertOrganization?.Address;
            txtFamilyName.EditValue = Conclusion?.Approver?.FamilyName;
            txtFirstName.EditValue = Conclusion?.Approver?.FirstName;
            txtSecondName.EditValue = Conclusion?.Approver?.SecondName;
            txtPosition.EditValue = Conclusion?.Approver?.Position;
            #endregion

            #region Page I_3
            btnDeclarant.EditValue = Conclusion?.Declarant;
            #endregion

            #region Page I_5
            _tDocuments = Conclusion.Documents?.ToList() ?? new List<TDocument>();
            gridControlDocuments.DataSource = DocumentObjController.GetDocumentObjS(_tDocuments).ToArray();
            foreach (GridColumn column in gridViewDocuments.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocChanges)] is GridColumn gridDocumentDocChanges)
            {
                gridDocumentDocChanges.Caption = "Примечание";
                gridDocumentDocChanges.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocDate)] is GridColumn gridDocumentDocDate)
            {
                gridDocumentDocDate.Caption = "Дата документа";
                gridDocumentDocDate.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocNumber)] is GridColumn gridDocumentDocNumber)
            {
                gridDocumentDocNumber.Caption = "Номер документа";
                gridDocumentDocNumber.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocName)] is GridColumn gridDocumentDocName)
            {
                gridDocumentDocName.Caption = "Наименование документа";
                gridDocumentDocName.Visible = true;
            }
            if (gridViewDocuments.Columns[nameof(DocumentObj.DocTypeName)] is GridColumn gridDocumentDocTypeName)
            {
                gridDocumentDocTypeName.Caption = "Тип документа";
                gridDocumentDocTypeName.Visible = true;
            }
            #endregion

            #region Page I_6
            _tPreviousConclusions = Conclusion?.PreviousConclusions?.ToList() ?? new List<TPreviousConclusion>();
            gridControlPreviousConclusions.DataSource = _tPreviousConclusions;
            foreach (GridColumn column in gridViewPreviousConclusions.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewPreviousConclusions.Columns[nameof(TPreviousConclusion.Name)] is GridColumn gridPreviousConclusionName)
            {
                gridPreviousConclusionName.Caption = "Наименование объекта экспертизы";
                gridPreviousConclusionName.Visible = true;
            }
            #endregion

            #region Page I_7
            cmbEcologyExpertiseNeedExpertise.EditValue = cmbEcologyExpertiseNeedExpertise.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
                    ?.FirstOrDefault(f => f.Value == Conclusion?.EcologyExpertise?.NeedExpertise.GetEnumXmlAttributeValue().ToString());

            if (cmbEcologyExpertiseNeedExpertise.EditValue is null)
                cmbEcologyExpertiseNeedExpertise.SelectedIndex = cmbEcologyExpertiseNeedExpertise.Properties.Items.Count - 1;

            memoEcologyExpertiseNote.Text = Conclusion?.EcologyExpertise?.Comment;
            #endregion

            //memoExpertiseResultNote.Text = Conclusion?.ExpertiseResultNote;

            #region Page II_1
            memoObjectName.EditValue = Conclusion?.Object?.Name;
            cmbObjectType.EditValue = cmbObjectType.Properties.Items
              ?.Cast<GeneralizedDictionaryXPO>()
              ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType)
              ?.FirstOrDefault(f => f.ValueInt == Conclusion?.Object?.Type.GetEnumXmlAttributeValueInt());
            cmbFunctionsClass.EditValue = Conclusion?.Object?.FunctionsClass;
            cmbObjectAddress.EditValue = Conclusion?.Object?.Address?.FirstOrDefault();
            _Tteis = Conclusion?.Object?.Tei?.ToList() ?? new List<Ttei>();
            gridControlTEI.DataSource = _Tteis;
            if (gridViewTEI.Columns[nameof(Ttei.Name)] is GridColumn gridTEIName)
            {
                gridTEIName.Caption = "Наименование показателя";
            }
            if (gridViewTEI.Columns[nameof(Ttei.Measure)] is GridColumn gridTEIMeasure)
            {
                gridTEIMeasure.Caption = "Единица измерения показателя";
            }
            if (gridViewTEI.Columns[nameof(Ttei.Value)] is GridColumn gridTEIValue)
            {
                gridTEIValue.Caption = "Значение показателя";
            }
            _tObjectParts = Conclusion?.Object?.ObjectPart?.ToList() ?? new List<TObjectPart>();
            gridControlObjectPart.DataSource = _tObjectParts;
            foreach (GridColumn column in gridViewObjectPart.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewObjectPart.Columns[nameof(TObjectPart.Name)] is GridColumn gridObjectPartName)
            {
                gridObjectPartName.Caption = "Наименование объекта";
                gridObjectPartName.Visible = true;
            }
            #endregion

            #region Page II_2
            _tFinances = Conclusion?.Finance?.ToList() ?? new List<TFinance>();
            //gridControlFinances.DataSource = _tFinances;
            gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
            foreach (GridColumn column in gridViewFinances.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.FinanceSize)] is GridColumn gridFinanceFinanceSize)
            {
                gridFinanceFinanceSize.Caption = "Размер финансирования (в % от общей суммы)";
                gridFinanceFinanceSize.Visible = true;

                gridFinanceFinanceSize.DisplayFormat.FormatType = FormatType.Numeric;
                gridFinanceFinanceSize.DisplayFormat.FormatString = "n5";
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.BudgetType)] is GridColumn gridFinanceBudgetType)
            {
                gridFinanceBudgetType.Caption = "Уровень бюджета (в случае бюджетного финансирования)";
                gridFinanceBudgetType.Visible = true;
            }
            if (gridViewFinances.Columns[nameof(FinanceObj.FinanceType)] is GridColumn gridFinanceFinanceType)
            {
                gridFinanceFinanceType.Caption = "Вид источника финансирования";
                gridFinanceFinanceType.Visible = true;
            }
            memoFinanceComment.EditValue = Conclusion.FinanceComment;
            #endregion

            #region Page II_3
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.ClimateDistrict, checkCmbClimateDistrict);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.SeismicActivity, checkCmbSeismicActivity);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.WindDistrict, checkCmbWindDistrict);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.GeologicalConditions, checkCmbGeologicalConditions);
            SetCheckComboBoxByItems(Conclusion?.ClimateConditions?.SnowDistrict, checkCmbSnowDistrict);
            memoClimateConditionsNote.EditValue = Conclusion?.ClimateConditionsNote;
            if (string.IsNullOrWhiteSpace(Conclusion?.ClimateConditionsNote))
            {
                memoClimateConditionsNote.EditValue = "Сведения отсутствуют.";
            }
            #endregion

            #region Page II_4
            _tDesigners = Conclusion?.Designer?.ToList() ?? new List<TDesigner>();
            gridControlDesigners.DataSource = PersonObjController.GetPersonObjByDesigner(_tDesigners)?.ToArray();
            foreach (GridColumn column in gridViewDesigners.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewDesigners.Columns[nameof(PersonObj.Name)] is GridColumn gridPersonObjName)
            {
                gridPersonObjName.Caption = "Наименование";
                gridPersonObjName.Visible = true;
            }
            #endregion

            #region Page II_5
            var eEPDUse = Conclusion?.EepdUse?.FirstOrDefault();
            if (eEPDUse != null)
            {
                var number = eEPDUse.EepdNumber;
                if (number != null)
                {
                    checkEEPDUse.EditValue = true;
                    _isEepdUseInForm = true;

                    if (!string.IsNullOrWhiteSpace(number.Egrz))
                    {
                        radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                        txtEEPDNumber.EditValue = number.Egrz;
                    }
                    else if (!string.IsNullOrWhiteSpace(number.NoEgrz))
                    {
                        radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                        txtEEPDNumber.EditValue = number.NoEgrz;
                    }

                    dateEEPDDate.EditValue = eEPDUse.EepdDate;
                    memoEEPDNote.EditValue = eEPDUse.EepdNote;
                }
            }
            #endregion

            #region Page II_6
            if (Conclusion?.CadastralNumber?.Count > 0)
            {
                _cadastralNumbers.AddRange(Conclusion?.CadastralNumber?.Select(s => new CadastralNumber() { Number = s }));
            }
            else
            {
                _cadastralNumbers = new List<CadastralNumber>();
            }
            gridControlCadastralNumber.DataSource = _cadastralNumbers;
            if (gridViewCadastralNumber.Columns[nameof(CadastralNumber.Number)] is GridColumn gridCadastralNumber)
            {
                gridCadastralNumber.Caption = "Кадастровый номер";
                gridCadastralNumber.Visible = true;
            }
            #endregion

            #region Page II_7

            if (Conclusion.EngineeringSurveyDeveloper.Count > 0)
            {
                _tEngineeringSurveyDeveloper.AddRange(Conclusion.EngineeringSurveyDeveloper);
            }

            if (Conclusion.EngineeringSurveyDeveloper.Count > 0)
            {
                _tEngineeringSurveyTechnicalCustomer.AddRange(Conclusion.EngineeringSurveyTechnicalCustomer);
            }

            gridControlEngineeringSurveyDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tEngineeringSurveyDeveloper).ToArray();
            foreach (GridColumn column in gridViewEngineeringSurveyDeveloper.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyDeveloper.Columns[nameof(PersonObj.Name)] is GridColumn gridEngineeringSurveyDeveloperName)
            {
                gridEngineeringSurveyDeveloperName.Caption = "Наименование";
                gridEngineeringSurveyDeveloperName.Visible = true;
            }
            gridControlEngineeringSurveyTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tEngineeringSurveyTechnicalCustomer).ToArray();
            foreach (GridColumn column in gridViewEngineeringSurveyTechnicalCustomer.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyTechnicalCustomer.Columns[nameof(PersonObj.Name)] is GridColumn gridEngineeringSurveyTechnicalCustomerName)
            {
                gridEngineeringSurveyTechnicalCustomerName.Caption = "Наименование";
                gridEngineeringSurveyTechnicalCustomerName.Visible = true;
            }

            if (Conclusion.ProjectDocumentsDeveloper.Count > 0)
            {
                _tProjectDocumentsDevelopers.AddRange(Conclusion.ProjectDocumentsDeveloper);
            }

            if (Conclusion.ProjectDocumentsTechnicalCustomer.Count > 0)
            {
                _tProjectDocumentsTechnicalCustomer.AddRange(Conclusion.ProjectDocumentsTechnicalCustomer);
            }

            gridControlProjectDocumentsDeveloper.DataSource = PersonObjController.GetPersonObjByDeclarator(_tProjectDocumentsDevelopers).ToArray();
            foreach (GridColumn column in gridViewProjectDocumentsDeveloper.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentsDeveloper.Columns[nameof(PersonObj.Name)] is GridColumn gridProjectDocumentsDevelopersName)
            {
                gridProjectDocumentsDevelopersName.Caption = "Наименование";
                gridProjectDocumentsDevelopersName.Visible = true;
            }
            gridControlProjectDocumentsTechnicalCustomer.DataSource = PersonObjController.GetPersonObjByTechnicalCustomer(_tProjectDocumentsTechnicalCustomer).ToArray();
            foreach (GridColumn column in gridViewProjectDocumentsTechnicalCustomer.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewProjectDocumentsTechnicalCustomer.Columns[nameof(PersonObj.Name)] is GridColumn gridProjectDocumentsTechnicalCustomerName)
            {
                gridProjectDocumentsTechnicalCustomerName.Caption = "Наименование";
                gridProjectDocumentsTechnicalCustomerName.Visible = true;
            }
            #endregion

            #region Page III
            _tExpertProjectDocuments = Conclusion?.ExpertProjectDocuments?.ToList() ?? new List<TExpertProjectDocuments>();
            gridControlExpertProjectDocuments.DataSource = ExpertProjectDocumentController.GetExpertProjectDocuments(_tExpertProjectDocuments).ToArray();
            foreach (GridColumn column in gridViewExpertProjectDocuments.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewExpertProjectDocuments.Columns[nameof(ExpertProjectDocument.ExpertType)] is GridColumn gridExpertProjectDocumentExpertType)
            {
                gridExpertProjectDocumentExpertType.Caption = "Направление деятельности";
                gridExpertProjectDocumentExpertType.Visible = true;
            }

            _tEngineeringSurveyAddresses = Conclusion?.EngineeringSurveyAddress?.ToList() ?? new List<TEngineeringSurveyAddress>();
            gridControlEngineeringSurveyAddress.DataSource = EngineeringSurveyAddressController.GetEngineeringSurveyAddress(_tEngineeringSurveyAddresses);
            foreach (GridColumn column in gridViewEngineeringSurveyAddress.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewEngineeringSurveyAddress.Columns[nameof(EngineeringSurveyAddress.EngineeringSurveyDistrict)] is GridColumn gridEngineeringSurveyDistrict)
            {
                gridEngineeringSurveyDistrict.Caption = "Описание района изысканий";
                gridEngineeringSurveyDistrict.Visible = true;
            }
            if (gridViewEngineeringSurveyAddress.Columns[nameof(EngineeringSurveyAddress.EngineeringSurveyRegion)] is GridColumn gridEngineeringSurveyRegion)
            {
                gridEngineeringSurveyRegion.Caption = "Регион";
                gridEngineeringSurveyRegion.Visible = true;
                gridEngineeringSurveyRegion.Width = 150;
                gridEngineeringSurveyRegion.OptionsColumn.FixedWidth = true;
            }

            _tExpertEngineeringSurveys = Conclusion.ExpertEngineeringSurveys?.ToList() ?? new List<TExpertEngineeringSurveys>();
            gridControlExpertEngineeringSurveys.DataSource = ExpertEngineeringSurveysController.GetExpertEngineeringSurveys(_tExpertEngineeringSurveys).ToArray();
            foreach (GridColumn column in gridViewExpertEngineeringSurveys.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewExpertEngineeringSurveys.Columns[nameof(ExpertEngineeringSurvey.EngineeringSurveyType)] is GridColumn gridEngineeringSurveyType)
            {
                gridEngineeringSurveyType.Caption = "Вид инженерных изысканий";
                gridEngineeringSurveyType.Visible = true;
            }
            #endregion

            #region Page V

            cmbEngineeringSurveysSummary.EditValue = Conclusion?.Summary?.EngineeringSurveySummarySpecified != true ? null : cmbEngineeringSurveysSummary.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveysSummary)
                ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.EngineeringSurveySummary.GetEnumXmlAttributeValue().ToString());

            memoEngineeringSurveySummaryDate.EditValue = Conclusion?.Summary?.EngineeringSurveySummaryDate;

            SetCheckComboBoxByItems(Conclusion?.Summary?.EngineeringSurveyType, checkCmbEngineeringSurveyType);

            cmbProjectDocumentationSummary.EditValue = Conclusion?.Summary?.ProjectDocumentsSummarySpecified != true ? null : cmbProjectDocumentationSummary.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationSummary)
                ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.ProjectDocumentsSummary.GetEnumXmlAttributeValue().ToString());

            memoProjectDocumentsSummaryDate.EditValue = Conclusion?.Summary?.ProjectDocumentsSummaryDate;

            cmbEstimateNormsAndWorksSummary.EditValue = Conclusion?.Summary?.EstimateNormsAndWorksSummarySpecified != true ? null : cmbEstimateNormsAndWorksSummary.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateNormsAndWorksSummary)
                ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.EstimateNormsAndWorksSummary.GetEnumXmlAttributeValue().ToString());

            cmbEstimateSummary.EditValue = Conclusion?.Summary?.EstimateSummarySpecified != true ? null : cmbEstimateSummary.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateSummary)
                ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.EstimateSummary.GetEnumXmlAttributeValue().ToString());
            #endregion

            #region Page VI

            memoEstimateSummary1315.EditValue = Conclusion?.Summary?.EstimateSummary1315;

            var pds = Conclusion?.Summary?.ExaminationSummary?.ProjectDocumentsSummary;

            cmbEngineeringSurveysResultsSummary.EditValue = Conclusion?.Summary?.ExaminationSummary?.EngineeringSurveysResultsSummarySpecified != true ? null :
                cmbEngineeringSurveysResultsSummary.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveysResultsSummary)
                    ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.ExaminationSummary?.EngineeringSurveysResultsSummary.GetEnumXmlAttributeValue().ToString());

            cmbProjectDocumentationEngineeringSurveysResultsSummary.EditValue =
                cmbProjectDocumentationEngineeringSurveysResultsSummary.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationEngineeringSurveysResultsSummary)
                    ?.FirstOrDefault(f => f.Value == pds?.ProjectDocumentationEngineeringSurveysResultsSummary.GetEnumXmlAttributeValue().ToString());

            cmbProjectDocumentationDesignAssignmentSummary.EditValue =
                cmbProjectDocumentationDesignAssignmentSummary.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationDesignAssignmentSummary)
                    ?.FirstOrDefault(f => f.Value == pds?.ProjectDocumentationDesignAssignmentSummary.GetEnumXmlAttributeValue().ToString());

            cmbProjectDocumentationTechnicalRequirementsSummary.EditValue =
                cmbProjectDocumentationTechnicalRequirementsSummary.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationTechnicalRequirementsSummary)
                    ?.FirstOrDefault(f => f.Value == pds?.ProjectDocumentationTechnicalRequirementsSummary.GetEnumXmlAttributeValue().ToString());

            cmbEstimateValidationSummary.EditValue = Conclusion?.Summary?.ExaminationSummary?.EstimateSummarySpecified != true ? null :
                cmbEstimateValidationSummary.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateValidationSummary)
                    ?.FirstOrDefault(f => f.Value == Conclusion?.Summary?.ExaminationSummary?.EstimateSummary.GetEnumXmlAttributeValue().ToString());
            #endregion

            #region Page VII
            _experts = Conclusion?.Experts?.ToList() ?? new List<TExpert>();
            gridControlExpert.DataSource = _experts;
            if (gridViewExpert.Columns[nameof(TExpert.FamilyName)] is GridColumn gridColumntExpertFamilyName)
            {
                gridColumntExpertFamilyName.Caption = "Фамилия";
            }
            if (gridViewExpert.Columns[nameof(TExpert.FirstName)] is GridColumn gridColumntExpertFirstName)
            {
                gridColumntExpertFirstName.Caption = "Имя";
            }
            if (gridViewExpert.Columns[nameof(TExpert.SecondName)] is GridColumn gridColumntExpertSecondName)
            {
                gridColumntExpertSecondName.Caption = "Отчество";
            }
            if (gridViewExpert.Columns[nameof(TExpert.ExpertType)] is GridColumn gridColumntExpertExpertType)
            {
                gridColumntExpertExpertType.Visible = false;
            }
            //if (gridViewExpert.Columns[nameof(TExpert.ExpertTypeString)] is GridColumn gridColumntExpertExpertTypeString)
            //{
            //    gridColumntExpertExpertTypeString.Caption = "Направление деятельности";
            //}
            if (gridViewExpert.Columns[nameof(TExpert.ExpertCertificate)] is GridColumn gridColumntExpertExpertCertificate)
            {
                gridColumntExpertExpertCertificate.Caption = "Номер аттестата";
            }
            if (gridViewExpert.Columns[nameof(TExpert.ExpertCertificateBeginDate)] is GridColumn gridColumntExpertExpertCertificateBeginDate)
            {
                gridColumntExpertExpertCertificateBeginDate.Caption = "Дата выдачи";
            }
            if (gridViewExpert.Columns[nameof(TExpert.ExpertCertificateEndDate)] is GridColumn gridColumntExpertExpertCertificateEndDate)
            {
                gridColumntExpertExpertCertificateEndDate.Caption = "Дата окончания";
            }
            #endregion
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            #region Page I
            cmbExaminationResult.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult).ToList());

            cmbExaminationForm.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationForm).ToList());

            cmbExaminationObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType).ToList());

            cmbExaminationStage.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationStage).ToList());

            cmbConstructionType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ConstructionType).ToList());

            checkCmbExaminationType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationType).ToArray());

            cmbProjectDocumentationIM.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());

            cmbEngineeringSurveysIM.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());

            cmbEcologyExpertiseNeedExpertise.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());
            #endregion

            #region Page II_1
            cmbObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType).ToArray());
            #endregion

            #region Page II_3
            checkCmbClimateDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ClimateDistrict).ToArray());
            checkCmbSeismicActivity.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SeismicActivity).ToArray());
            checkCmbWindDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.WindDistrict).ToArray());
            checkCmbGeologicalConditions.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.GeologicalConditions).ToArray());
            checkCmbSnowDistrict.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.SnowDistrict).ToArray());
            #endregion

            #region Page II_5
            radioGroupConclusionNumber.Properties.Items.AddEnum(typeof(ItemChoiceType));

            cmbEngineeringSurveysSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveysSummary).ToArray());

            cmbProjectDocumentationSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationSummary).ToArray());

            cmbEstimateNormsAndWorksSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateNormsAndWorksSummary).ToArray());

            cmbEstimateSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateSummary).ToArray());

            cmbEngineeringSurveysResultsSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveysResultsSummary).ToArray());

            cmbProjectDocumentationEngineeringSurveysResultsSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationEngineeringSurveysResultsSummary).ToArray());

            cmbProjectDocumentationDesignAssignmentSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationDesignAssignmentSummary).ToArray());

            cmbProjectDocumentationTechnicalRequirementsSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ProjectDocumentationTechnicalRequirementsSummary).ToArray());

            cmbEstimateValidationSummary.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EstimateValidationSummary).ToArray());
            #endregion

            #region Page V
            checkCmbEngineeringSurveyType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.EngineeringSurveyType).ToArray());
            #endregion
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewCadastralNumber_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuCadastralNumber.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewDesigners_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuDesigners.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewEngineeringSurveyAddress_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuEngineeringSurveyAddress.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewEngineeringSurveyDeveloper_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuEngineeringSurveyDeveloper.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewEngineeringSurveyTechnicalCustomer_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuEngineeringSurveyTechnicalCustomer.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewExpert_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuExpert.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewExpertEngineeringSurveys_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuExpertEngineeringSurveys.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewExpertProjectDocuments_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuExpertProjectDocuments.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewFinances_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuFinance.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewMismatches_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuMismatches.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewObjectPart_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuObjectPart.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewPreviousConclusions_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuPreviousConclusions.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewProjectDocumentsDeveloper_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuProjectDocumentsDeveloper.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewProjectDocumentsTechnicalCustomer_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuProjectDocumentsTechnicalCustomer.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewTEI_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuTEI.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void radioGroupConclusionNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is RadioGroup radioGroup)
            {
                switch (radioGroup.SelectedIndex)
                {
                    case (int)ItemChoiceType.EGRZ:
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
                        txtEEPDNumber.Properties.MaskSettings.Set("mask", "[0-9]{2}-[1-2]{1}-[1-2]{1}-[1-3]{1}-[0-9]{6}-[0-9]{4}");
                        break;
                    case (int)ItemChoiceType.noEGRZ:
                        txtEEPDNumber.Properties.ResetMaskSettings();
                        txtEEPDNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                        break;
                }
            }
        }

        private Session Session => SessionController.GetSession();

        public static ICollection<T> GetCheckComboBoxItems<T>(CheckedComboBoxEdit sender, string message = default, bool isNullAble = true) where T : Enum
        {
            var result = new List<T>();

            var items = sender.Properties.Items
                .Where(w => w.CheckState == CheckState.Checked);

            foreach (var item in items)
            {
                if (item.Value is GeneralizedDictionaryXPO obj)
                {
                    result.Add(GetEnumByAttributeValue<T>(GetValueGeneralizedDictionary(obj, message: message).Value));
                }
            }

            if (result.Count > 0)
            {
                return result;
            }

            if (isNullAble)
            {
                return result;
            }

            if (sender is Control control)
            {
                control.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        /// <summary>
        /// Значение перечисления по коду из XmlEnum (код хранится в <see cref="GeneralizedDictionaryXPO.Value"/>).
        /// Если код не найден: при <paramref name="isNullAble"/> = true возвращается default(T)
        /// (первое значение перечисления), иначе бросается исключение.
        /// </summary>
        public static T GetEnumByAttributeValue<T>(string value, bool isNullAble = true)
        {
            if (EnumExtension.TryGetEnumByXmlAttributeValue(value, out T result))
            {
                return result;
            }

            if (isNullAble)
            {
                return default;
            }

            throw new Exception($"Не найдено значение \"{value}\" в перечислении {typeof(T).Name}.");
        }

        /// <summary>
        /// Необязательное поле-перечисление (в модели есть свойство ...Specified):
        /// <paramref name="specified"/> = true, только если в списке выбрано значение из схемы.
        /// </summary>
        public static T GetOptionalEnum<T>(object sender, out bool specified)
        {
            var item = GetValueGeneralizedDictionary(sender, isNullAble: true);
            specified = item != null && EnumExtension.TryGetEnumByXmlAttributeValue(item.Value, out T _);
            return GetEnumByAttributeValue<T>(item?.Value);
        }

        public static string GetStringValue(object sender, object control = default, string message = default, bool isNullAble = true)
        {
            var result = sender?.ToString();
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            if (isNullAble)
            {
                return result;
            }

            if (control is Control obj)
            {
                obj.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        /// <summary>
        /// Выбранная запись справочника.
        /// Если <paramref name="isNullAble"/> не задан, поле обязательно, когда передан <paramref name="message"/>:
        /// пустой выбор иначе превращался бы в первое значение перечисления и попадал в XML как ложные данные.
        /// </summary>
        public static GeneralizedDictionaryXPO GetValueGeneralizedDictionary(object sender, string message = default, bool? isNullAble = null)
        {
            var allowNull = isNullAble ?? string.IsNullOrEmpty(message);

            if (sender is ButtonEdit buttonEdit)
            {
                if (buttonEdit.EditValue is GeneralizedDictionaryXPO result)
                {
                    return result;
                }
            }
            else
            {
                if (sender is GeneralizedDictionaryXPO result)
                {
                    return result;
                }
            }

            if (allowNull)
            {
                return default;
            }

            if (sender is Control control)
            {
                control.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        public static void SetCheckComboBoxByItems<T>(ICollection<T> collection, CheckedComboBoxEdit comboBoxEdit) where T : Enum
        {
            if (collection is null)
            {
                return;
            }

            foreach (var item in collection)
            {
                var checkItem = comboBoxEdit.Properties.Items
                    .FirstOrDefault(f => f.Value is GeneralizedDictionaryXPO obj && obj.Value == item.GetEnumXmlAttributeValue()?.ToString());
                if (checkItem != null)
                {
                    checkItem.CheckState = CheckState.Checked;
                    checkItem.CheckState = CheckState.Checked;
                }
            }
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        public Conclusion Conclusion { get; private set; }
        public string FileName { get; private set; }
        public bool IsSave { get; private set; }

        private class CadastralNumber
        {
            [RegExEditMask(@"\d+:\d+:\d+:\d+", AllowBlankInput = true, ShowPlaceholders = false)]
            public string Number { get; set; }
        }
    }
}