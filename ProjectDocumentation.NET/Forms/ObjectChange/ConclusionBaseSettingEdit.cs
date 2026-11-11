using Core.NET.AutoModels.ConclusionObj;
using Core.NET.DaDataObj;
using ProjectDocumentation.NET.Services;
using Core.NET.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ConclusionBaseSettingEdit : XtraForm
    {
        public ConclusionBaseSettingEdit()
        {
            InitializeComponent();
        }

        public ConclusionBaseSettingEdit(TOrganization TOrganization, TWorkPerson person) : this()
        {
            Organization = TOrganization;
            Person = person;
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Organization.OrgFullName = ConclusionEdit.GetStringValue(txtOrgFullName.EditValue, txtOrgFullName, "Заполните наименование организации");
                Organization.OrgOgrn = ConclusionEdit.GetStringValue(txtOrgOGRN.EditValue, txtOrgOGRN, "Заполните ОГРН организации");
                Organization.OrgKpp = ConclusionEdit.GetStringValue(txtOrgKPP.EditValue, txtOrgKPP, "Заполните КПП организации");
                Organization.OrgInn = ConclusionEdit.GetStringValue(txtOrgINN.EditValue, txtOrgINN, "Заполните ИНН организации");
                Organization.Email = ConclusionEdit.GetStringValue(txtEmail.EditValue, txtEmail, isNullAble: true);
                if (btnAddress.EditValue is TAddress address)
                {
                    Organization.Address = address;
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

                Person.FamilyName = ConclusionEdit.GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию подписанта");
                Person.FirstName = ConclusionEdit.GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя подписанта");
                Person.SecondName = ConclusionEdit.GetStringValue(txtSecondName.EditValue, txtSecondName, "Заполните отчество подписанта");
                Person.Position = ConclusionEdit.GetStringValue(txtPosition.EditValue, txtPosition, "Заполните должность подписанта");
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

                            var splitsSplitsName = data.management?.name?.Split();
                            if (splitsSplitsName?.Length >= 3)
                            {
                                LegacyDaDataLookup.FillEmpty(txtFamilyName, splitsSplitsName[0]);
                                LegacyDaDataLookup.FillEmpty(txtFirstName, splitsSplitsName[1]);
                                LegacyDaDataLookup.FillEmpty(txtSecondName, splitsSplitsName[2]);
                            }
                            LegacyDaDataLookup.FillEmpty(txtPosition, data.management?.post);

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

        private void ConclusionBaseSettingEdit_Load(object sender, EventArgs e)
        {
            if (Organization is null)
            {
                Organization = new TOrganization
                {
                    OrgInn = "5405997054"
                };
            }

            btnSearch.EditValue = Organization.OrgInn;
            txtOrgFullName.EditValue = Organization.OrgFullName;
            txtOrgOGRN.EditValue = Organization.OrgOgrn;
            txtOrgKPP.EditValue = Organization.OrgKpp;
            txtOrgINN.EditValue = Organization.OrgInn;
            txtEmail.EditValue = Organization.Email;
            btnAddress.EditValue = Organization.Address;

            if (Person is null)
            {
                Person = new TWorkPerson();
            }

            txtFamilyName.EditValue = Person.FamilyName;
            txtFirstName.EditValue = Person.FirstName;
            txtSecondName.EditValue = Person.SecondName;
            txtPosition.EditValue = Person.Position;
        }

        public bool IsSave { get; private set; }
        public TOrganization Organization { get; private set; }
        public TWorkPerson Person { get; private set; }
    }
}