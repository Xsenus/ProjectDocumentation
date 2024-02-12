using Core.NET.AutoModels;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class AuthorEdit : XtraForm
    {
        private Session Session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tAuthor Author { get; private set; }

        private List<tSRO> _tSROs = new List<tSRO>();

        public AuthorEdit()
        {
            InitializeComponent();
        }

        public AuthorEdit(tAuthor obj) : this()
        {
            Author = obj;
        }

        private async Task FillFormControllersAsync()
        {
            radioGroupTypeOrganization.Properties.Items.AddEnum(typeof(TypeOrganization));
            var person = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.Person);
            person.Enabled = false;
            radioGroupTypeOrganization.Properties.Items.Remove(person);

            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Author is null)
            {
                Author = new tAuthor();
            }

            if (Author?.Item is tForeignOrganization tForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = tForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = tForeignOrganization.OrgKPP;
                txtFOrgINN.EditValue = tForeignOrganization.OrgINN;
                txtFEmail.EditValue = tForeignOrganization.Email;
                btnFAddress.EditValue = tForeignOrganization.Address;
            }
            else if (Author?.Item is tOrganization tOrganization)
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
            else if (Author?.Item is tIP tIP)
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
            else
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.Entity;
            }

            _tSROs = Author?.SROMembership?.ToList() ?? new List<tSRO>();
            gridControlSROMembership.DataSource = _tSROs;
            foreach (GridColumn column in gridViewSROMembership.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
            }
            if (gridViewSROMembership.Columns[nameof(tSRO.Value)] is GridColumn gridModificationDate)
            {
                gridModificationDate.Caption = "Значение";
                gridModificationDate.Visible = true;
            }
            if (gridViewSROMembership.Columns[nameof(tSRO.SROType)] is GridColumn gridModificationNote)
            {
                gridModificationNote.Caption = "Тип";
                gridModificationNote.Visible = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
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
                        Author.Item = entity;
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
                        Author.Item = foreignLegalEntity;
                        break;

                    case (int)TypeOrganization.IndividualEntrepreneur:
                        var tIP = new tIP
                        {
                            FamilyName = ConclusionEdit.GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию"),
                            FirstName = ConclusionEdit.GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя"),
                            SecondName = ConclusionEdit.GetStringValue(txtSecondName.EditValue, txtSecondName, isNullAble: true),
                            OGRNIP = ConclusionEdit.GetStringValue(txtOGRNIP.EditValue, txtOrgINN, "Заполните ОГРНИП"),
                            Email = ConclusionEdit.GetStringValue(txtEmailIp.EditValue, txtEmailIp, isNullAble: true)
                        };
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
                        Author.Item = tIP;
                        break;
                }

                if (_tSROs.Count > 0)
                {
                    Author.SROMembership = _tSROs.ToArray();
                }
                else
                {
                    Author.SROMembership = null;
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
                        break;

                    case (int)TypeOrganization.ForeignLegalEntity:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Always;
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Never;
                        break;

                    case (int)TypeOrganization.IndividualEntrepreneur:
                        layoutControlGroupEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupForeignLegalEntity.Visibility = LayoutVisibility.Never;
                        layoutControlGroupIndividualEntrepreneur.Visibility = LayoutVisibility.Always;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnPostAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    PostAddressEdit form;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        private void gridViewSROMembership_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuSROMembership.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barBtnSROMembershipAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tSROs.Add(new tSRO());
            gridControlSROMembership.RefreshDataSource();
        }

        private void barBtnSROMembershipDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewSROMembership.GetRow(gridViewSROMembership.FocusedRowHandle) is tSRO obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _tSROs.Remove(obj);
                    gridControlSROMembership.RefreshDataSource();
                }
            }
        }
    }
}