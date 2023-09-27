using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
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
    public partial class TechnicalCustomerEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tTechnicalCustomer TechnicalCustomer { get; private set; }

        public TechnicalCustomerEdit()
        {
            InitializeComponent();
        }

        public TechnicalCustomerEdit(tTechnicalCustomer obj) : this()
        {
            TechnicalCustomer = obj;
        }

        public TechnicalCustomerEdit(object obj) : this()
        {
            if (obj is tTechnicalCustomer tTechnicalCustomer)
            {
                TechnicalCustomer = tTechnicalCustomer;
            }
        }

        private async Task FillFormControllersAsync()
        {
            radioGroupTypeOrganization.Properties.Items.AddEnum(typeof(TypeOrganization));
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync(); 
            var individualEntrepreneur = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.IndividualEntrepreneur);
            individualEntrepreneur.Enabled = false;
            var person = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.Person);
            person.Enabled = false;
            radioGroupTypeOrganization.Properties.Items.Remove(individualEntrepreneur);
            radioGroupTypeOrganization.Properties.Items.Remove(person);
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (TechnicalCustomer is null)
            {
                TechnicalCustomer = new tTechnicalCustomer();
            }

            if (TechnicalCustomer?.Item is tForeignOrganization tForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = tForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = tForeignOrganization.OrgKPP;
                txtFOrgINN.EditValue = tForeignOrganization.OrgINN;
                txtFEmail.EditValue = tForeignOrganization.Email;
                btnFAddress.EditValue = tForeignOrganization.Address;
            }
            else if (TechnicalCustomer?.Item is tOrganization tOrganization)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                switch (radioGroupTypeOrganization.SelectedIndex)
                {
                    case (int)TypeOrganization.Entity:
                        var entity = new tOrganization();
                        entity.OrgFullName = ConclusionEdit.GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации");
                        entity.OrgOGRN = ConclusionEdit.GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации");
                        entity.OrgKPP = ConclusionEdit.GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации");
                        entity.OrgINN = ConclusionEdit.GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации");
                        entity.Email = ConclusionEdit.GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true);
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
                        TechnicalCustomer.Item = entity;
                        break;

                    case (int)TypeOrganization.ForeignLegalEntity:
                        var foreignLegalEntity = new tForeignOrganization();
                        foreignLegalEntity.OrgFullName = ConclusionEdit.GetStringValue(txtFOrgFullName.EditValue, txtFOrgFullName, "Заполните наименование организации");
                        foreignLegalEntity.OrgKPP = ConclusionEdit.GetStringValue(txtFOrgKPP.EditValue, txtFOrgKPP, "Заполните КПП организации");
                        foreignLegalEntity.OrgINN = ConclusionEdit.GetStringValue(txtFOrgINN.EditValue, txtFOrgINN, "Заполните ИНН организации");
                        foreignLegalEntity.Email = ConclusionEdit.GetStringValue(txtFEmail.EditValue, txtFEmail, isNullAble: true);
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
                        TechnicalCustomer.Item = foreignLegalEntity;
                        break;                    
                }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private async void btnAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
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