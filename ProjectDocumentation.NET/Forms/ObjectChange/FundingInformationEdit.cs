using Core.NET.AutoModels;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
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
        private Session Session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tFinance Finance { get; private set; }

        public FundingInformationEdit()
        {
            InitializeComponent();
        }

        public FundingInformationEdit(tFinance obj) : this()
        {
            Finance = obj;
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
                Finance = new tFinance();
            }

            cmbFinanceType.EditValue = cmbFinanceType.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.FinanceType)
                  ?.FirstOrDefault(f => f.ValueInt == Finance?.FinanceType.GetEnumXmlAttributeValueInt());

            cmbBudgetType.EditValue = cmbBudgetType.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.BudgetType)
                  ?.FirstOrDefault(f => f.ValueInt == Finance?.BudgetType.GetEnumXmlAttributeValueInt());

            spinFinanceSize.Value = Finance.FinanceSize;

            if (Finance.FinanceOwner?.Item is tForeignOrganization tForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = tForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = tForeignOrganization.OrgKPP;
                txtFOrgINN.EditValue = tForeignOrganization.OrgINN;
                txtFEmail.EditValue = tForeignOrganization.Email;
                btnFAddress.EditValue = tForeignOrganization.Address;
            }
            else if (Finance.FinanceOwner?.Item is tOrganization tOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Entity;
                btnSearch.EditValue = tOrganization.OrgINN;
                txtOrgFullName.EditValue = tOrganization.OrgFullName;
                txtOrgOGRN.EditValue = tOrganization.OrgOGRN;
                txtOrgKPP.EditValue = tOrganization.OrgKPP;
                txtOrgINN.EditValue = tOrganization.OrgINN;
                txtEmail.EditValue = tOrganization.Email;
                btnAddress.EditValue = tOrganization.Address;
            }
            else
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Entity;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var financeType = ConclusionEdit.GetEnumByAttributeValue<tFinanceType>(ConclusionEdit.GetValueGeneralizedDictionary(
                   cmbFinanceType,
                   message: "Заполните поле вид источника финансирования")?.Value);

                Finance.FinanceType = financeType;
                Finance.FinanceOwner = null;

                switch (financeType)
                {
                    case tFinanceType.Item1:
                        Finance.BudgetType = ConclusionEdit.GetEnumByAttributeValue<tBudgetType>(ConclusionEdit.GetValueGeneralizedDictionary(
                           cmbBudgetType,
                           message: "Заполните поле вид бюджета")?.Value);
                        break;

                    case tFinanceType.Item2:
                        Finance.FinanceOwner = new tTechnicalCustomer();
                        switch (radioGroupTypeOrganization.SelectedIndex)
                        {
                            case (int)TypeOrganization.Entity:
                                var entity = new tOrganization
                                {
                                    OrgFullName = ConclusionEdit.GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации"),
                                    OrgOGRN = ConclusionEdit.GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации"),
                                    OrgKPP = ConclusionEdit.GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации"),
                                    OrgINN = ConclusionEdit.GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации"),
                                    Email = ConclusionEdit.GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true)
                                };
                                if (btnAddress.EditValue is tAddress address)
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
                                Finance.FinanceOwner.Item = entity;
                                break;

                            case (int)TypeOrganization.ForeignLegalEntity:
                                var foreignLegalEntity = new tForeignOrganization
                                {
                                    OrgFullName = ConclusionEdit.GetStringValue(txtFOrgFullName.EditValue, txtFOrgFullName, "Заполните наименование организации"),
                                    OrgKPP = ConclusionEdit.GetStringValue(txtFOrgKPP.EditValue, txtFOrgKPP, "Заполните КПП организации"),
                                    OrgINN = ConclusionEdit.GetStringValue(txtFOrgINN.EditValue, txtFOrgINN, "Заполните ИНН организации"),
                                    Email = ConclusionEdit.GetStringValue(txtFEmail.EditValue, txtFEmail, isNullAble: true)
                                };
                                if (btnFAddress.EditValue is tAddress addressF)
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
                                Finance.FinanceOwner.Item = foreignLegalEntity;
                                break;
                        }
                        break;
                }

                Finance.FinanceSize = spinFinanceSize.Value;
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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
                    var financeType = ConclusionEdit.GetEnumByAttributeValue<tFinanceType>(ConclusionEdit.GetValueGeneralizedDictionary(
                       cmbFinanceType,
                       message: "Заполните поле вид источника финансирования")?.Value);

                    switch (financeType)
                    {
                        case tFinanceType.Item1:
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Always;
                            tabbedControlGroup.Visibility = LayoutVisibility.Never;
                            break;

                        case tFinanceType.Item2:
                            cmbBudgetType.EditValue = null;
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Never;
                            tabbedControlGroup.Visibility = LayoutVisibility.Always;
                            break;

                        case tFinanceType.Item3:
                            cmbBudgetType.EditValue = null;
                            layoutControlItemBudgetType.Visibility = LayoutVisibility.Never;
                            tabbedControlGroup.Visibility = LayoutVisibility.Never;
                            break;
                    }
                }
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
                            txtOrgFullName.EditValue = data.name?.full_with_opf ?? data.name.ToString();
                            txtOrgOGRN.EditValue = data.ogrn;
                            txtOrgKPP.EditValue = data.kpp;
                            txtOrgINN.EditValue = data.inn;

                            var addressData = data.address?.data;
                            if (addressData != null)
                            {
                                var address = new tAddress()
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
                                    var region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", string.Empty));
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
        private async void btnAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    AddressEdit form;
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
                                var region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(addressData.region_kladr_id?.Replace("0", string.Empty));
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
    }
}