using Core.NET.AutoModels;
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
using ProjectDocumentation.NET.Models.ConclusionObj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ConclusionEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        private class Wrapper
        {
            public string Number { get; set; }
        }

        public string FileName { get; private set; }
        public bool IsSave { get; private set; }
        public Conclusion Conclusion { get; private set; }


        private List<tTEI> _tTEIs;
        private List<tObjectPart> _tObjectParts;
        private List<tFinance> _tFinances;


        private List<tExpert> _experts = new List<tExpert>();
        private List<tMismatch> _mismatch = new List<tMismatch>();
        private List<Wrapper> _cadastralNumbers = new List<Wrapper>();

        public ConclusionEdit()
        {
            InitializeComponent();
        }

        public ConclusionEdit(Conclusion conclusion) : this()
        {
            Conclusion = conclusion;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

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
            #endregion

            #region Page II_1
            cmbObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType).ToArray());
            #endregion

        }

        private async void ConclusionEdit_Load(object sender, EventArgs e)
        {
            txtFileName.EditValue = FileName;

            await FillFormControllersAsync();

            var climateDistrictCollection = await new XPQuery<ClimateDistrictXPO>(SessionController.GetSession()).ToListAsync();
            checkCmbClimateDistrict.Properties.Items.AddRange(climateDistrictCollection.ToArray());

            if (Conclusion is null)
            {
                Conclusion = new Conclusion();
            }
            else
            {
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
                memoName.Text = Conclusion.ExaminationObject?.Name;
                memoExaminationStageNote.Text = Conclusion.ExaminationObject?.ExaminationStageNote;
                #endregion

                #region Page II_1
                memoObjectName.EditValue = Conclusion.Object?.Name;
                cmbObjectType.EditValue = cmbObjectType.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType)
                  ?.FirstOrDefault(f => f.ValueInt == Conclusion?.Object?.Type.GetEnumXmlAttributeValueInt());
                cmbFunctionsClass.EditValue = Conclusion.Object?.Item;
                cmbObjectAddress.EditValue = Conclusion.Object?.Address?.FirstOrDefault();
                _tTEIs = Conclusion.Object?.TEI?.ToList() ?? new List<tTEI>();
                gridControlTEI.DataSource = _tTEIs;
                if (gridViewTEI.Columns[nameof(tTEI.Name)] is GridColumn gridTEIName)
                {
                    gridTEIName.Caption = "Наименование показателя";
                }
                if (gridViewTEI.Columns[nameof(tTEI.Measure)] is GridColumn gridTEIMeasure)
                {
                    gridTEIMeasure.Caption = "Единица измерения показателя";
                }
                if (gridViewTEI.Columns[nameof(tTEI.Value)] is GridColumn gridTEIValue)
                {
                    gridTEIValue.Caption = "Значение показателя";
                }
                _tObjectParts = Conclusion.Object?.ObjectPart?.ToList() ?? new List<tObjectPart>();
                gridControlObjectPart.DataSource = _tObjectParts;
                foreach (GridColumn column in gridViewObjectPart.Columns)
                {
                    column.Visible = false;
                }
                if (gridViewObjectPart.Columns[nameof(tObjectPart.Name)] is GridColumn gridObjectPartName)
                {
                    gridObjectPartName.Caption = "Наименование объекта";
                    gridObjectPartName.Visible = true;
                }
                #endregion

                _tFinances = Conclusion?.Finance?.ToList() ?? new List<tFinance>();
                //gridControlFinances.DataSource = _tFinances;
                gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);

                foreach (GridColumn column in gridViewFinances.Columns)
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


                //memoEstimateNorms.EditValue = Conclusion.ExpertEstimate?.EstimateNorms;
                //txtEstimatedCost.Text = Conclusion.EstimatedCost?.Currency;
                //txtEstimatedCompleteCostBefore.Text
                //txtEstimatedCompleteCostPost.Text


                memoObjectName.EditValue = Conclusion.Object?.Name;
                cmbObjectType.SelectedIndex = (int?)Conclusion?.Object.Type ?? -1;
                cmbFunctionsClass.EditValue = Conclusion.Object?.Item;
                cmbObjectAddress.EditValue = Conclusion.Object?.Address?.FirstOrDefault();

                memoEngineeringSurveySummary.EditValue = Conclusion.Summary?.EngineeringSurveySummary;
                memoEngineeringSurveySummaryDate.EditValue = Conclusion.Summary?.EngineeringSurveySummaryDate;
                memoProjectDocumentsSummary.EditValue = Conclusion.Summary?.ProjectDocumentsSummary;
                memoProjectDocumentsSummaryDate.EditValue = Conclusion.Summary?.ProjectDocumentsSummaryDate;
                memoEstimateNormsAndWorksSummary.EditValue = Conclusion.Summary?.EstimateNormsAndWorksSummary;
                memoEstimateSummary.EditValue = Conclusion.Summary?.EstimateSummary;
                memoExaminationSummary.EditValue = Conclusion.Summary?.ExaminationSummary;

                if (Conclusion.ClimateConditions != null)
                {
                    foreach (var item in Conclusion.ClimateConditions.ClimateDistrict)
                    {
                        var checkItem = checkCmbClimateDistrict.Properties.Items
                            .FirstOrDefault(f => f.Value is ClimateDistrictXPO obj && obj.Value == item.GetEnumXmlAttributeValue()?.ToString());
                        if (checkItem != null)
                        {
                            checkItem.CheckState = CheckState.Checked;
                        }
                    }
                }

                if (Conclusion.Experts != null)
                {
                    _experts.AddRange(Conclusion.Experts);
                }

                if (Conclusion.CadastralNumber?.Length > 0)
                {
                    _cadastralNumbers.AddRange(Conclusion.CadastralNumber?.Select(s => new Wrapper() { Number = s }));
                }
            }

            gridControlExpert.DataSource = _experts;
            gridControlCadastralNumber.DataSource = _cadastralNumbers;
        }

        public static void SetCheckComboBoxByItems<T>(ICollection<T> collection, CheckedComboBoxEdit comboBoxEdit) where T : Enum
        {
            foreach (var item in collection)
            {
                var checkItem = comboBoxEdit.Properties.Items
                    .FirstOrDefault(f => f.Value is GeneralizedDictionaryXPO obj && obj.Value == item.GetEnumXmlAttributeValue()?.ToString());
                if (checkItem != null)
                {
                    checkItem.CheckState = CheckState.Checked;
                }
            }
        }

        public static ICollection<T> GetCheckComboBoxItems<T>(CheckedComboBoxEdit sender, string message = default, bool isNullable = false) where T : Enum
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

            if (isNullable)
            {
                return result;
            }

            if (sender is Control control)
            {
                control.Focus();
            }

            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        public static GeneralizedDictionaryXPO GetValueGeneralizedDictionary(object sender, string message = default, bool isNullAble = false)
        {
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

            if (isNullAble)
            {
                return default;
            }

            if (sender is Control control)
            {
                control.Focus();
            }
            
            throw new Exception(message ?? "Не заполнен один из параметров.");
        }

        public static T GetEnumByAttributeValue<T>(string value, bool isNullAble = false) where T : Enum
        {
            foreach (T type in Enum.GetValues(typeof(T)))
            {
                if (type.GetEnumXmlAttributeValue()?.ToString()?.Equals(value) is true)
                {
                    return type;
                }
            }

            if (isNullAble)
            {
                return default;
            }

            throw new Exception();
        }

        public static string GetStringValue(object sender, object control = default, string message = default, bool isNullAble = false)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileName = txtFileName.Text;

            try
            {
                #region Page I
                Conclusion.ExaminationObject = new tExaminationObject();
                Conclusion.ExaminationObject.ExaminationResult = GetEnumByAttributeValue<tExaminationResult>(GetValueGeneralizedDictionary(
                    cmbExaminationResult,
                    message: "Заполните поле результат экспертизы").Value);
                Conclusion.ExaminationObject.ExaminationForm = GetEnumByAttributeValue<tExaminationForm>(GetValueGeneralizedDictionary(
                    cmbExaminationForm,
                    message: "Заполните поле формы экспертизы").Value);
                Conclusion.ExaminationObject.ExaminationObjectType = GetEnumByAttributeValue<tExaminationObjectType>(GetValueGeneralizedDictionary(
                    cmbExaminationObjectType,
                    message: "Заполните поле вида объекта экспертизы").Value);
                Conclusion.ExaminationObject.ExaminationStage = GetEnumByAttributeValue<tExaminationStage>(GetValueGeneralizedDictionary(
                   cmbExaminationStage,
                   message: "Заполните поле вид экспертизы").Value);
                Conclusion.ExaminationObject.ConstructionType = GetEnumByAttributeValue<tConstractionType>(GetValueGeneralizedDictionary(
                   cmbConstructionType,
                   message: "Заполните поле вид работ").Value);
                Conclusion.ExaminationObject.ExaminationType = GetCheckComboBoxItems<tExaminationType>(
                    checkCmbExaminationType,
                    "Заполните поле предмет экпертизы").ToArray();
                Conclusion.ExaminationObject.Name = GetStringValue(memoName.Text, memoName, "Заполните поле наименование объекта экспертизы");
                Conclusion.ExaminationObject.ExaminationStageNote = GetStringValue(memoExaminationStageNote.Text, isNullAble: true);
                #endregion

                #region Page II_1
                Conclusion.Object = new tObject();
                Conclusion.Object.Name = GetStringValue(memoObjectName.Text, memoObjectName, "Заполните поле наименование объекта капитального строительства");
                Conclusion.Object.Type = GetEnumByAttributeValue<tObjectType>(GetValueGeneralizedDictionary(
                   cmbObjectType,
                   message: "Заполните поле вид объекта капитального строительства").Value);
                Conclusion.Object.Item = GetStringValue(cmbFunctionsClass.EditValue?.ToString(), cmbFunctionsClass, "Функциональное назначение объекта капитального строительства (код по классификатору)");
                if (cmbObjectAddress.EditValue is tAddress objectAddress)
                {
                    Conclusion.Object.Address = new tAddress[] { objectAddress };
                }
                else
                {
                    XtraMessageBox.Show("Укажите адрес объекта.", "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbObjectAddress.Focus();
                    return;
                }
                if (_tTEIs.Count > 0)
                {
                    foreach (var item in _tTEIs)
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                        {
                            XtraMessageBox.Show("Наименование технико-экономического показателя не может быть пустым.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            gridViewTEI.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Measure))
                        {
                            XtraMessageBox.Show("Единица измерения технико-экономического показателя не может быть пустой.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            gridViewTEI.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(item.Value))
                        {
                            XtraMessageBox.Show("Значение технико-экономического показателя не может быть пустым.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            gridViewTEI.Focus();
                            return;
                        }
                    }
                    Conclusion.Object.TEI = _tTEIs.ToArray();
                }                
                if (_tObjectParts.Count > 0)
                {
                    Conclusion.Object.ObjectPart = _tObjectParts.ToArray();
                }
                #endregion

                if (_tFinances.Count > 0)
                {
                    Conclusion.Finance = _tFinances.ToArray();
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Conclusion.ExpertEstimate = new tExpertEstimate();
            Conclusion.ExpertEstimate.EstimateNorms = memoEstimateNorms.Text;
            Conclusion.EstimatedCost = new tEstimatedCost();
            Conclusion.EstimatedCost.Currency = txtEstimatedCost.Text;
            //txtEstimatedCompleteCostBefore.Text
            //txtEstimatedCompleteCostPost.Text

           
            Conclusion.Object.Item = cmbFunctionsClass.Text;

            if (cmbObjectAddress.EditValue is tAddress address)
            {
                Conclusion.Object.Address = new tAddress[] { address };
            }


            Conclusion.Summary = new tSummary();
            Conclusion.Summary.EngineeringSurveySummary = memoEngineeringSurveySummary.Text;
            Conclusion.Summary.EngineeringSurveySummaryDate = memoEngineeringSurveySummaryDate.Text;
            Conclusion.Summary.ProjectDocumentsSummary = memoProjectDocumentsSummary.Text;
            Conclusion.Summary.ProjectDocumentsSummaryDate = memoProjectDocumentsSummaryDate.Text;
            Conclusion.Summary.EstimateNormsAndWorksSummary = memoEstimateNormsAndWorksSummary.Text;
            Conclusion.Summary.EstimateSummary = memoEstimateSummary.Text;
            Conclusion.Summary.ExaminationSummary = memoExaminationSummary.Text;

            if (_experts.Count > 0)
            {
                Conclusion.Experts = _experts.ToArray();
            }

            if (_cadastralNumbers.Count() > 0)
            {
                Conclusion.CadastralNumber = _cadastralNumbers?.Select(s => s.Number).ToArray();
            }

            IsSave = true;
            Close();
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


        private void gridViewMismatches_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuMismatches.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnMismatchesAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnMismatchesEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var form = new EstimateMismatchEdit())
            {
                form.ShowDialog();
                //if (form.IsSave && form.EstimateMismatch != null)
                //{
                //    _experts.Add(form.Expert);
                //}
            };

            gridControlExpert.RefreshDataSource();
        }

        private void barBtnMismatchesDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewExpert.GetRow(gridViewExpert.FocusedRowHandle) is tMismatch tMismatch)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _mismatch.Remove(tMismatch);
                }
            }
        }

        private void cmbObjectAddress_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ComboBoxEdit comboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(AddressEdit);
                    if (comboBoxEdit.EditValue is tAddress address)
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
                        comboBoxEdit.EditValue = form.Address;
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    comboBoxEdit.EditValue = null;
                }
            }            
        }

        private void barBtnCadastralNumberAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _cadastralNumbers.Add(new Wrapper());
            gridControlCadastralNumber.RefreshDataSource();
        }

        private void barBtnCadastralNumberDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewCadastralNumber.GetRow(gridViewExpert.FocusedRowHandle) is Wrapper obj)
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

        private void gridViewCadastralNumber_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuCadastralNumber.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void clearEditValue_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is CheckedComboBoxEdit checkedComboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    foreach (CheckedListBoxItem item in checkedComboBoxEdit.Properties.Items)
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

        private async void cmbObjectAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(AddressEdit);
                    if (buttonEdit.EditValue is tAddress address)
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
                            var address = new tAddress()
                            {
                                Country = addressData.country,
                                District = addressData.country,
                                City = addressData.city,
                                Settlement = addressData.settlement,
                                Street = addressData.street,
                                Building = addressData.house,
                                Room = addressData.flat,
                                Note = addressData.result,
                            };

                            try
                            {
                                var region = GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", ""));
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

        private void gridViewTEI_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuTEI.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnTEIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tTEIs.Add(new tTEI());
            gridControlTEI.RefreshDataSource();
        }

        private void barBtnTEIDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewTEI.GetRow(gridViewTEI.FocusedRowHandle) is tTEI obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tTEIs.Remove(obj);
                    gridControlTEI.RefreshDataSource();
                }
            }
        }

        private void gridViewObjectPart_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuObjectPart.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

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

        private void barBtnObjectPartEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is tObjectPart obj)
            {
                var form = new ObjPartEdit(obj);
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlObjectPart.RefreshDataSource();
                }
            }
        }

        private void barBtnObjectPartDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewObjectPart.GetRow(gridViewObjectPart.FocusedRowHandle) is tObjectPart obj)
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

        private void gridViewFinances_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView gridView)
            {
                popupMenuFinance.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

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

        private void barBtnFinanceEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewFinances.GetRow(gridViewFinances.FocusedRowHandle) is FinanceObj obj)
            {
                var form = new FundingInformationEdit(obj.tFinance); 
                form.ShowDialog();
                if (form.IsSave)
                {
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }

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
                    _tFinances.Remove(obj.tFinance);
                    gridControlFinances.DataSource = FinanceObjController.GetFinanceObj(_tFinances);
                    gridControlFinances.RefreshDataSource();
                }
            }
        }
    }
}