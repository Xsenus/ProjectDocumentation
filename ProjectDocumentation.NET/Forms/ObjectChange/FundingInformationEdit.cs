using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
using ProjectDocumentation.NET.Services;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class FundingInformationEdit : XtraForm
    {
        public FundingInformationEdit()
        {
            InitializeComponent();
        }

        public FundingInformationEdit(TFinance obj) : this()
        {
            Finance = obj;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
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

                    using (form)
                    {
                        form.ShowDialog();
                        if (form.IsSave)
                        {
                            buttonEdit.EditValue = form.Address;
                        }
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue as string;
                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        var addressData = await LegacyDaDataLookup.FindAddressAsync(buttonEdit, obj);
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var financeType = ConclusionEdit.GetEnumByAttributeValue<TFinanceType>(ConclusionEdit.GetValueGeneralizedDictionary(
                   cmbFinanceType,
                   message: "Заполните поле вид источника финансирования")?.Value);

                Finance.FinanceType = financeType;
                Finance.FinanceOwner = null;
                // BudgetType — необязательный элемент: без флага Specified вид бюджета не попадал в XML.
                Finance.BudgetTypeSpecified = false;

                switch (financeType)
                {
                    case TFinanceType.Item1:
                        Finance.BudgetType = ConclusionEdit.GetEnumByAttributeValue<TBudgetType>(ConclusionEdit.GetValueGeneralizedDictionary(
                           cmbBudgetType,
                           message: "Заполните поле вид бюджета")?.Value);
                        Finance.BudgetTypeSpecified = true;
                        break;

                    case TFinanceType.Item2:
                        Finance.FinanceOwner = new TTechnicalCustomer();
                        switch (radioGroupTypeOrganization.SelectedIndex)
                        {
                            case (int)TypeOrganization.Entity:
                                var entity = new TOrganization
                                {
                                    OrgFullName = ConclusionEdit.GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации"),
                                    OrgOgrn = ConclusionEdit.GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации"),
                                    OrgKpp = ConclusionEdit.GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации"),
                                    OrgInn = ConclusionEdit.GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации"),
                                    Email = ConclusionEdit.GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true)
                                };
                                if (btnAddress.EditValue is TAddress address)
                                {
                                    entity.Address = address;
                                }
                                else
                                {
                                    btnAddress.Focus();
                                    XtraMessageBox.Show("Заполните адрес организации",
                                        "Информационное сообщение",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    return;
                                }
                                Finance.FinanceOwner.Organization = entity;
                                break;

                            case (int)TypeOrganization.ForeignLegalEntity:
                                var foreignLegalEntity = new TForeignOrganization
                                {
                                    OrgFullName = ConclusionEdit.GetStringValue(txtFOrgFullName.EditValue, txtFOrgFullName, "Заполните наименование организации"),
                                    OrgKpp = ConclusionEdit.GetStringValue(txtFOrgKPP.EditValue, txtFOrgKPP, "Заполните КПП организации"),
                                    OrgInn = ConclusionEdit.GetStringValue(txtFOrgINN.EditValue, txtFOrgINN, "Заполните ИНН организации"),
                                    Email = ConclusionEdit.GetStringValue(txtFEmail.EditValue, txtFEmail, isNullAble: true)
                                };
                                if (btnFAddress.EditValue is TAddress addressF)
                                {
                                    foreignLegalEntity.Address = addressF;
                                }
                                else
                                {
                                    btnFAddress.Focus();
                                    XtraMessageBox.Show("Заполните адрес организации",
                                        "Информационное сообщение",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    return;
                                }
                                Finance.FinanceOwner.ForeignOrganization = foreignLegalEntity;
                                break;
                        }
                        break;
                }

                Finance.FinanceSize = spinFinanceSize.Value;
                // Необязательный элемент: без флага Specified размер финансирования не попадал в XML.
                Finance.FinanceSizeSpecified = Finance.FinanceSize != 0;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message,
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
                        var data = await LegacyDaDataLookup.FindPartyAsync(buttonEdit, obj, txtOrgINN, txtOrgOGRN, txtOrgKPP);
                        if (data != null)
                        {
                            LegacyDaDataLookup.FillEmpty(txtOrgFullName, data.name?.full_with_opf ?? data.name?.full);
                            LegacyDaDataLookup.FillEmpty(txtOrgOGRN, data.ogrn);
                            LegacyDaDataLookup.FillEmpty(txtOrgKPP, data.kpp);
                            LegacyDaDataLookup.FillEmpty(txtOrgINN, data.inn);

                            var addressData = data.address?.data;
                            if (DaDataObj.IsRussianAddress(addressData) && addressData.qc == "0")
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

                                LegacyDaDataLookup.FillEmpty(btnAddress, address);
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
        private void clearEditValue_ButtonPressed(object sender, ButtonPressedEventArgs e)
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
        private void cmbFinanceType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is ComboBoxEdit comboBoxEdit)
            {
                if (comboBoxEdit.EditValue is GeneralizedDictionaryXPO)
                {
                    var financeType = ConclusionEdit.GetEnumByAttributeValue<TFinanceType>(ConclusionEdit.GetValueGeneralizedDictionary(
                       cmbFinanceType,
                       message: "Заполните поле вид источника финансирования")?.Value);

                    switch (financeType)
                    {
                        case TFinanceType.Item1:
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Always;
                            tabbedControlGroup.Visibility = LayoutVisibility.Never;
                            break;

                        case TFinanceType.Item2:
                            cmbBudgetType.EditValue = null;
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Never;
                            tabbedControlGroup.Visibility = LayoutVisibility.Always;
                            break;

                        case TFinanceType.Item3:
                            cmbBudgetType.EditValue = null;
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Never;
                            tabbedControlGroup.Visibility = LayoutVisibility.Never;
                            break;
                    }
                }
            }
        }

        private async Task FillFormControllersAsync()
        {
            radioGroupTypeOrganization.Properties.Items.AddEnum(typeof(TypeOrganization));
            var individualEntrepreneur = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.IndividualEntrepreneur);
            individualEntrepreneur.Enabled = false;
            var person = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.Person);
            person.Enabled = false;
            radioGroupTypeOrganization.Properties.Items.Remove(individualEntrepreneur);
            radioGroupTypeOrganization.Properties.Items.Remove(person);

            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbFinanceType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FinanceType).ToArray());

            cmbBudgetType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.BudgetType).ToArray());
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Finance is null)
            {
                Finance = new TFinance();
            }

            cmbFinanceType.EditValue = cmbFinanceType.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FinanceType)
                  ?.FirstOrDefault(f => f.ValueInt == Finance?.FinanceType.GetEnumXmlAttributeValueInt());

            cmbBudgetType.EditValue = Finance?.BudgetTypeSpecified != true ? null : cmbBudgetType.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.BudgetType)
                  ?.FirstOrDefault(f => f.ValueInt == Finance?.BudgetType.GetEnumXmlAttributeValueInt());

            spinFinanceSize.Value = Finance.FinanceSize;

            if (Finance.FinanceOwner?.ForeignOrganization is TForeignOrganization TForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = TForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = TForeignOrganization.OrgKpp;
                txtFOrgINN.EditValue = TForeignOrganization.OrgInn;
                txtFEmail.EditValue = TForeignOrganization.Email;
                btnFAddress.EditValue = TForeignOrganization.Address;
            }
            else if (Finance.FinanceOwner?.Organization is TOrganization TOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Entity;
                btnSearch.EditValue = TOrganization.OrgInn;
                txtOrgFullName.EditValue = TOrganization.OrgFullName;
                txtOrgOGRN.EditValue = TOrganization.OrgOgrn;
                txtOrgKPP.EditValue = TOrganization.OrgKpp;
                txtOrgINN.EditValue = TOrganization.OrgInn;
                txtEmail.EditValue = TOrganization.Email;
                btnAddress.EditValue = TOrganization.Address;
            }
            else
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Entity;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void radioGroupTypeOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is RadioGroup radioGroup)
            {
                switch (radioGroup.SelectedIndex)
                {
                    case (int)TypeOrganization.Entity:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Always;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Never;
                        break;

                    case (int)TypeOrganization.ForeignLegalEntity:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Always;
                        break;
                }
            }
        }

        private Session Session => SessionController.GetSession();
        public TFinance Finance { get; private set; }
        public bool IsSave { get; private set; }
    }
}