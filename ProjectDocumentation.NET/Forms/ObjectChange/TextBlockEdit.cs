using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class TextBlockEdit : XtraForm
    {
        private List<ItemsObj> _items = new List<ItemsObj>();

        public bool IsSave { get; private set; }
        public tTextBlock TextBlock { get; private set; }

        private class ItemsObj
        {
            public ItemsObj(ItemsChoiceType itemsChoiceType, int position = 1)
            {
                ItemsChoiceType = itemsChoiceType;
                Position = position;
            }

            public ItemsObj(object value, ItemsChoiceType itemsChoiceType, int position = 1) : this(itemsChoiceType, position)
            {
                Value = value;
            }

            public int Position { get; set; }
            public ItemsChoiceType ItemsChoiceType { get; private set; }
            public string ItemsChoiceTypeString => ItemsChoiceType.GetEnumDescription();
            public object Value { get; set; }
        }

        public TextBlockEdit()
        {
            InitializeComponent();
        }

        public TextBlockEdit(tTextBlock tTextBlock) : this()
        {
            TextBlock = tTextBlock;
        }

        private void TextBlockEdit_Load(object sender, EventArgs e)
        {
            if (TextBlock is null)
            {
                TextBlock = new tTextBlock();
            }

            memoEditTitle.EditValue = TextBlock.Title;

            if (TextBlock.Items != null && TextBlock.ItemsElementName != null)
            {
                if (TextBlock.Items.Count() == TextBlock.ItemsElementName.Count())
                {
                    for (int i = 0; i < TextBlock.Items.Count(); i++)
                    {
                        _items.Add(new ItemsObj(TextBlock.Items[i], TextBlock.ItemsElementName[i], i + 1));
                    }
                }
            }

            gridControlItems.DataSource = _items;
            foreach (GridColumn column in gridViewItems.Columns.Cast<GridColumn>())
            {
                column.Visible = false;
                column.OptionsColumn.AllowEdit = false;
                column.OptionsColumn.ReadOnly = true;
            }
            if (gridViewItems.Columns[nameof(ItemsObj.Value)] is GridColumn gridColumnValue)
            {
                gridColumnValue.Caption = "Значение";
                gridColumnValue.Visible = true;
                gridColumnValue.OptionsColumn.AllowEdit = true;
                gridColumnValue.OptionsColumn.ReadOnly = false;
            }
            if (gridViewItems.Columns[nameof(ItemsObj.ItemsChoiceTypeString)] is GridColumn gridColumnItemsChoiceTypeString)
            {
                gridColumnItemsChoiceTypeString.Caption = "Тип";
                gridColumnItemsChoiceTypeString.Visible = true;
                gridColumnItemsChoiceTypeString.Width = 100;
                gridColumnItemsChoiceTypeString.OptionsColumn.FixedWidth = true;
                gridColumnItemsChoiceTypeString.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                gridColumnItemsChoiceTypeString.OptionsColumn.ReadOnly = true;
            }
            if (gridViewItems.Columns[nameof(ItemsObj.Position)] is GridColumn gridColumnPosition)
            {
                gridColumnPosition.Caption = "Позиция";
                gridColumnPosition.Visible = true;
                gridColumnPosition.Width = 75;
                gridColumnPosition.OptionsColumn.FixedWidth = true;
                gridColumnPosition.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                gridColumnPosition.OptionsColumn.AllowEdit = true;
                gridColumnPosition.OptionsColumn.ReadOnly = false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TextBlock.Title = ConclusionEdit.GetStringValue(memoEditTitle.EditValue, memoEditTitle, "Заполните заголовок");
                
                if (_items.Count > 0)
                {
                    TextBlock.Items = _items.Select(s => s.Value).ToArray();
                    TextBlock.ItemsElementName = _items.Select(s => s.ItemsChoiceType).ToArray();
                }
                else
                {
                    TextBlock.Items = null;
                    TextBlock.ItemsElementName = null;
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

        private void AddItemsObj(ItemsChoiceType itemsChoiceType)
        {
            _items.Add(new ItemsObj(itemsChoiceType, _items.Count + 1));
            gridViewItems.RefreshData();
        }

        private void DeleteItemsObj(ItemsObj itemsObj)
        {
            _items.Remove(itemsObj);

            var position = 1;
            foreach (var item in _items.OrderBy(o => o.Position))
            {
                item.Position = position;
                position++;
            }
            gridViewItems.RefreshData();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnAddText_Click(object sender, EventArgs e)
        {
            AddItemsObj(ItemsChoiceType.Text);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnAddSubTitle_Click(object sender, EventArgs e)
        {
            AddItemsObj(ItemsChoiceType.SubTitle);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            AddItemsObj(ItemsChoiceType.Image);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            AddItemsObj(ItemsChoiceType.Table);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void gridViewItems_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (sender is GridView)
            {
                popupMenuItemsObj.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void barBtnItemsObjDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewItems.GetRow(gridViewItems.FocusedRowHandle) is ItemsObj obj)
            {
                if (XtraMessageBox.Show(
                    "Вы действительно хотите удалить выделенный объект?",
                    "Информационное сообщение",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DeleteItemsObj(obj);
                }
            }
        }

        private void gridViewItems_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == nameof(ItemsObj.Value))
            {
                if (sender is GridView gridView)
                {
                    if (gridView.GetRow(e.RowHandle) is ItemsObj itemsObj)
                    {
                        if (itemsObj.ItemsChoiceType == ItemsChoiceType.Image)
                        {
                            CreateRepositoryItemButtonEdit(sender, e, itemsObj);
                        }
                        else if (itemsObj.ItemsChoiceType == ItemsChoiceType.Table)
                        {
                            CreateRepositoryItemButtonEdit(sender, e, itemsObj);
                        }
                    }
                }
            }            
        }

        private void CreateRepositoryItemButtonEdit(object sender, CustomRowCellEditEventArgs e, ItemsObj itemsObj)
        {
            var buttonEdit = new RepositoryItemButtonEdit();
            buttonEdit.TextEditStyle = TextEditStyles.HideTextEditor;
            buttonEdit.Buttons[0].Kind = ButtonPredefines.Glyph;
            buttonEdit.Buttons[0].Caption = "Редактировать...";
            buttonEdit.ButtonPressed += (senderObj, eObj) => ImageButtonPressed(senderObj, eObj, itemsObj);
            e.RepositoryItem = buttonEdit;
        }

        private void ImageButtonPressed(object sender, ButtonPressedEventArgs e, ItemsObj itemsObj)
        {
            if (itemsObj.ItemsChoiceType == ItemsChoiceType.Image)
            {
                ImageEdit form;
                if (itemsObj.Value is tImage tImage)
                {
                    form = new ImageEdit(tImage);
                }
                else
                {
                    form = new ImageEdit();
                }
                form.ShowDialog();
                if (form.IsSave)
                {
                    itemsObj.Value = form.Image;
                }
            }
            else if (itemsObj.ItemsChoiceType == ItemsChoiceType.Table)
            {
                TableEdit form;
                if (itemsObj.Value is tTable tTable)
                {
                    form = new TableEdit(tTable);
                }
                else
                {
                    form = new TableEdit();
                }
                form.ShowDialog();
                if (form.IsSave)
                {
                    itemsObj.Value = form.Table;
                }
            }
        }
    }
}