using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
using Core.NET.Extensions;
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
using TTechnicalCustomer = Core.NET.AutoModels.ExplanatoryNoteObj.TTechnicalCustomer;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class TechnicalCustomerENEdit : XtraForm
    {
        private List<tSRO> _tSROs = new List<tSRO>();

        public TechnicalCustomerENEdit()
        {
            InitializeComponent();
        }

        public TechnicalCustomerENEdit(TTechnicalCustomer obj) : this()
        {
            TechnicalCustomer = obj;
        }

        public TechnicalCustomerENEdit(object obj) : this()
        {
            if (obj is TTechnicalCustomer TTechnicalCustomer)
            {
                TechnicalCustomer = TTechnicalCustomer;
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
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
                        TechnicalCustomer.Item = entity;
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
                        TechnicalCustomer.Item = foreignLegalEntity;
                        break;
                }

                if (_tSROs.Count > 0)
                {
                    TechnicalCustomer.SROMembership = _tSROs.ToArray();
                }
                else
                {
                    TechnicalCustomer.SROMembership = null;
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

        private async Task FillFormControllersAsync()
        {
            radioGroupTypeOrganization.Properties.Items.AddEnum(typeof(TypeOrganization));
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();
            var individualEntrepreneur = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.IndividualEntrepreneur);
            individualEntrepreneur.Enabled = false;
            var person = radioGroupTypeOrganization.Properties.Items?.FirstOrDefault(f => f.Value is TypeOrganization typeOrganization && typeOrganization == TypeOrganization.Person);
            person.Enabled = false;
            radioGroupTypeOrganization.Properties.Items.Remove(individualEntrepreneur);
            radioGroupTypeOrganization.Properties.Items.Remove(person);
        }

        private void gridViewSROMembership_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuSROMembership.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (TechnicalCustomer is null)
            {
                TechnicalCustomer = new TTechnicalCustomer();
            }

            if (TechnicalCustomer?.Item is TForeignOrganization TForeignOrganization)
            {
                radioGroupTypeOrganization.SelectedIndex = (int)TypeOrganization.ForeignLegalEntity;
                txtFOrgFullName.EditValue = TForeignOrganization.OrgFullName;
                txtFOrgKPP.EditValue = TForeignOrganization.OrgKpp;
                txtFOrgINN.EditValue = TForeignOrganization.OrgInn;
                txtFEmail.EditValue = TForeignOrganization.Email;
                btnFAddress.EditValue = TForeignOrganization.Address;
            }
            else if (TechnicalCustomer?.Item is TOrganization TOrganization)
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

            _tSROs = TechnicalCustomer?.SROMembership?.ToList() ?? new List<tSRO>();
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
        public bool IsSave { get; private set; }
        public TTechnicalCustomer TechnicalCustomer { get; private set; }
    }
}