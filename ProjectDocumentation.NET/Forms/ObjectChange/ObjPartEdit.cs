using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.DaDataObj;
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
        private List<tTEI> _tTEIs;
        private Session Session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tObjectPart ObjectPart { get; private set; }

        public ObjPartEdit()
        {
            InitializeComponent();
        }

        public ObjPartEdit(tObjectPart tObjectPart) : this()
        {
            ObjectPart = tObjectPart;
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
                ObjectPart = new tObjectPart();
            }

            memoObjectName.EditValue = ObjectPart?.Name;
            //cmbObjectType.EditValue = cmbObjectType.Properties.Items
            //  ?.Cast<GeneralizedDictionaryXPO>()
            //  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ObjectType)
            //  ?.FirstOrDefault(f => f.ValueInt == ObjectPart?.Type.GetEnumXmlAttributeValueInt());
            cmbFunctionsClass.EditValue = ObjectPart?.Item;
            cmbObjectAddress.EditValue = ObjectPart?.Address?.FirstOrDefault();
            _tTEIs = ObjectPart?.TEI?.ToList() ?? new List<tTEI>();
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectPart.Name = ConclusionEdit.GetStringValue(memoObjectName.Text, memoObjectName, "Заполните поле наименование объекта капитального строительства");
                //ObjectPart.Type = ConclusionEdit.GetEnumByAttributeValue<tObjectType>(ConclusionEdit.GetValueGeneralizedDictionary(
                //   cmbObjectType,
                //   message: "Заполните поле вид объекта капитального строительства")?.Value);
                ObjectPart.Item = ConclusionEdit.GetStringValue(cmbFunctionsClass.EditValue?.ToString(), cmbFunctionsClass, "Функциональное назначение объекта капитального строительства (код по классификатору)");
                if (cmbObjectAddress.EditValue is tAddress objectAddress)
                {
                    ObjectPart.Address = new tAddress[] { objectAddress };
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
                    ObjectPart.TEI = _tTEIs.ToArray();
                }
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
        private async void cmbObjectAddress_ButtonClick(object sender, ButtonPressedEventArgs e)
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
        private void gridViewTEI_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuTEI.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnTEIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _tTEIs.Add(new tTEI());
            gridControlTEI.RefreshDataSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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
    }
}