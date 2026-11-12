using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using ProjectDocumentation.NET.Services;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ObjPartEdit : XtraForm
    {
        private List<Ttei> _tTEIs;

        public ObjPartEdit()
        {
            InitializeComponent();
            ClassicRequiredFields.HighlightExisting(this);
            ClassicRequiredFields.Unmark(cmbObjectType);
            ClassicRequiredFields.Mark(cmbFunctionsClass);
            GridContextMenus.Attach(gridViewTEI, popupMenuTEI, barBtnTEIAdd);
        }

        public ObjPartEdit(TObjectPart TObjectPart) : this()
        {
            ObjectPart = TObjectPart;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnTEIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tTEIs.Add(new Ttei());
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
                    _tTEIs.Remove(obj);
                    gridControlTEI.RefreshDataSource();
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
                gridViewTEI.PostEditor();
                gridViewTEI.UpdateCurrentRow();
                if (!(cmbObjectAddress.EditValue is TAddress objectAddress))
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

                }

                ObjectPart.Name = ConclusionEdit.GetStringValue(memoObjectName.Text, memoObjectName, "Заполните поле наименование объекта капитального строительства");
                ObjectPart.FunctionsClass = ConclusionEdit.GetStringValue(cmbFunctionsClass.EditValue, cmbFunctionsClass,
                    "Функциональное назначение объекта капитального строительства (код по классификатору)");
                if (ObjectPart.Address.Count == 0) ObjectPart.Address.Add(objectAddress);
                else ObjectPart.Address[0] = objectAddress;
                ObjectPart.Tei.Clear();
                foreach (var item in _tTEIs) ObjectPart.Tei.Add(item);
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
        private void cmbObjectAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
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
                            buttonEdit.Refresh();
                        }
                    }
                }
                else if (e.Button.Kind == ButtonPredefines.Search)
                {
                    var obj = buttonEdit.EditValue;
                    if (obj == null || obj is string)
                    {
                        var addressData = LegacyDaDataLookup.FindAddress(buttonEdit);
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

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType).ToArray());
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (ObjectPart is null)
            {
                ObjectPart = new TObjectPart();
            }

            memoObjectName.EditValue = ObjectPart?.Name;
            //cmbObjectType.EditValue = cmbObjectType.Properties.Items
            //  ?.Cast<GeneralizedDictionaryXPO>()
            //  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType)
            //  ?.FirstOrDefault(f => f.ValueInt == ObjectPart?.Type.GetEnumXmlAttributeValueInt());
            cmbFunctionsClass.EditValue = ObjectPart?.FunctionsClass;
            cmbObjectAddress.EditValue = ObjectPart?.Address?.FirstOrDefault();
            _tTEIs = ObjectPart.Tei.Select(item => new Ttei
            {
                Name = item.Name,
                Measure = item.Measure,
                Value = item.Value
            }).ToList();
            gridControlTEI.DataSource = _tTEIs;
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
        }

        private Session Session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public TObjectPart ObjectPart { get; private set; }
    }
}
