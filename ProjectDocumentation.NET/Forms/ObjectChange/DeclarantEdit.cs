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
    public partial class DeclarantEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tDeclarant Declarant { get; private set; }

        public DeclarantEdit()
        {
            InitializeComponent();
        }

        public DeclarantEdit(tDeclarant obj) : this()
        {
            Declarant = obj;
        }

        public DeclarantEdit(object obj) : this()
        {
            if (obj is tDeclarant tDeclarant)
            {
                Declarant = tDeclarant;
            }
        }

        private async Task FillFormControllersAsync()
        {
            radioGroupTypeOrganization.Properties.Items.AddEnum(typeof(TypeOrganization));
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Declarant is null)
            {
                Declarant = new tDeclarant();
            }

            if (Declarant?.Item is tForeignOrganization tForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = tForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = tForeignOrganization.OrgKPP;
                txtFOrgINN.EditValue = tForeignOrganization.OrgINN;
                txtFEmail.EditValue = tForeignOrganization.Email;
                btnFAddress.EditValue = tForeignOrganization.Address;
            }
            else if (Declarant?.Item is tOrganization tOrganization)
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
            else if (Declarant?.Item is tIP tIP)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.IndividualEntrepreneur;
                btnSearchIp.EditValue = tIP.OGRNIP;
                txtFamilyName.EditValue = tIP.FamilyName;
                txtFirstName.EditValue = tIP.FirstName;
                txtSecondName.EditValue = tIP.SecondName;
                txtOGRNIP.EditValue = tIP.OGRNIP;
                txtEmailIp.EditValue = tIP.Email;
                btnPostAddress.EditValue = tIP.PostAddress;
            }
            else if (Declarant?.Item is tPerson tPerson)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Person;
                txtFamilyNamePerson.EditValue = tPerson.FamilyName;
                txtFirstNamePerson.EditValue = tPerson.FirstName;
                txtSecondNamePerson.EditValue = tPerson.SecondName;
                txtSNILS.EditValue = tPerson.SNILS;
                txtEmailPerson.EditValue = tPerson.Email;
                btnPostAddressPerson.EditValue = tPerson.PostAddress;
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
                        Declarant.Item = entity;
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
                        Declarant.Item = foreignLegalEntity;
                        break;

                    case (int)TypeOrganization.IndividualEntrepreneur:
                        var tIP = new tIP();
                        tIP.FamilyName = ConclusionEdit.GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию");
                        tIP.FirstName = ConclusionEdit.GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя");
                        tIP.SecondName = ConclusionEdit.GetStringValue(txtSecondName.EditValue, txtSecondName, isNullAble: true);
                        tIP.OGRNIP = ConclusionEdit.GetStringValue(txtOGRNIP.EditValue, txtOrgINN, "Заполните ОГРНИП");
                        tIP.Email = ConclusionEdit.GetStringValue(txtEmailIp.EditValue, txtEmailIp, isNullAble: true);
                        if (btnPostAddress.EditValue is tPostAddress tPostAddress)
                        {
                            tIP.PostAddress = tPostAddress;
                        }
                        else
                        {
                            btnPostAddress.Focus();
                            XtraMessageBox.Show("Заполните адрес",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                        Declarant.Item = tIP;
                        break;

                    case (int)TypeOrganization.Person:
                        var tPerson = new tPerson();
                        tPerson.FamilyName = ConclusionEdit.GetStringValue(txtFamilyNamePerson.EditValue, txtFamilyNamePerson, "Заполните фамилию");
                        tPerson.FirstName = ConclusionEdit.GetStringValue(txtFirstNamePerson.EditValue, txtFirstNamePerson, "Заполните имя");
                        tPerson.SecondName = ConclusionEdit.GetStringValue(txtSecondNamePerson.EditValue, txtSecondNamePerson, isNullAble: true);
                        tPerson.SNILS = ConclusionEdit.GetStringValue(txtSNILS.EditValue, txtSNILS, "Заполните СНИЛС");
                        tPerson.Email = ConclusionEdit.GetStringValue(txtEmailPerson.EditValue, txtEmailPerson, isNullAble: true);
                        if (btnPostAddressPerson.EditValue is tPostAddress tPostAddressPerson)
                        {
                            tPerson.PostAddress = tPostAddressPerson;
                        }
                        else
                        {
                            btnPostAddressPerson.Focus();
                            XtraMessageBox.Show("Заполните адрес",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                        Declarant.Item = tPerson;
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
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Never;
                        layoutControlGroupPerson.Visibility = LayoutVisibility.Never;
                        break;

                    case (int)TypeOrganization.ForeignLegalEntity:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Always;
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Never;
                        layoutControlGroupPerson.Visibility = LayoutVisibility.Never;
                        break;

                    case (int)TypeOrganization.IndividualEntrepreneur:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Always;
                        layoutControlGroupPerson.Visibility = LayoutVisibility.Never;
                        break;

                    case (int)TypeOrganization.Person:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Never;
                        layoutControlGroupPerson.Visibility = LayoutVisibility.Always;
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

        private async void btnPostAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    var form = default(PostAddressEdit);
                    if (buttonEdit.EditValue is tPostAddress postAddress)
                    {
                        form = new PostAddressEdit(postAddress);
                    }
                    else
                    {
                        form = new PostAddressEdit();
                    }

                    form.ShowDialog();
                    if (form.IsSave)
                    {
                        buttonEdit.EditValue = form.PostAddress;
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
                            var postAddress = new tPostAddress()
                            {
                                PostIndex = addressData.postal_code,
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
                                postAddress.Region = region;
                            }
                            catch (Exception)
                            {
                                XtraMessageBox.Show("Регион не определен, введите вручную",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                            }

                            buttonEdit.EditValue = postAddress;
                        }
                    }
                }
            }
        }

        private async void btnSearchIp_ButtonPressed(object sender, ButtonPressedEventArgs e)
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
                            txtOGRNIP.EditValue = data.ogrn;
                            var splitsSplitsName = data.name?.full?.Split();
                            if (splitsSplitsName?.Length >= 3)
                            {
                                txtFamilyName.EditValue = splitsSplitsName[0];
                                txtFirstName.EditValue = splitsSplitsName[1];
                                txtSecondName.EditValue = splitsSplitsName[2];
                            }

                            var addressData = data.address?.data;
                            if (addressData != null)
                            {
                                var postAddress = new tPostAddress()
                                {
                                    PostIndex = addressData.postal_code,
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
                                    postAddress.Region = region;
                                }
                                catch (Exception)
                                {
                                    XtraMessageBox.Show("Регион не определен, введите вручную",
                                        "Информационное сообщение",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                                }

                                btnPostAddress.EditValue = postAddress;
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
    }
}