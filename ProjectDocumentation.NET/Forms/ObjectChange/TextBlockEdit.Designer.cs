namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    partial class TextBlockEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutControlTextBlock = new DevExpress.XtraLayout.LayoutControl();
            this.gridControlItems = new DevExpress.XtraGrid.GridControl();
            this.gridViewItems = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAddTable = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddImage = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddSubTitle = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditTitle = new DevExpress.XtraEditors.MemoEdit();
            this.btnAddText = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItemSave = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItemClose = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemTitle = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupAddItem = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemAddText = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAddSubTitle = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAddImage = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAddTable = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemGridControlItems = new DevExpress.XtraLayout.LayoutControlItem();
            this.popupMenuItemsObj = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barBtnItemsObjDel = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTextBlock)).BeginInit();
            this.layoutControlTextBlock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupAddItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddSubTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGridControlItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuItemsObj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTextBlock
            // 
            this.layoutControlTextBlock.AllowCustomization = false;
            this.layoutControlTextBlock.Controls.Add(this.gridControlItems);
            this.layoutControlTextBlock.Controls.Add(this.btnAddTable);
            this.layoutControlTextBlock.Controls.Add(this.btnAddImage);
            this.layoutControlTextBlock.Controls.Add(this.btnAddSubTitle);
            this.layoutControlTextBlock.Controls.Add(this.btnClose);
            this.layoutControlTextBlock.Controls.Add(this.btnSave);
            this.layoutControlTextBlock.Controls.Add(this.memoEditTitle);
            this.layoutControlTextBlock.Controls.Add(this.btnAddText);
            this.layoutControlTextBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlTextBlock.Location = new System.Drawing.Point(0, 0);
            this.layoutControlTextBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControlTextBlock.Name = "layoutControlTextBlock";
            this.layoutControlTextBlock.Root = this.Root;
            this.layoutControlTextBlock.Size = new System.Drawing.Size(648, 468);
            this.layoutControlTextBlock.TabIndex = 0;
            this.layoutControlTextBlock.Text = "layoutControl1";
            // 
            // gridControlItems
            // 
            this.gridControlItems.Location = new System.Drawing.Point(6, 120);
            this.gridControlItems.MainView = this.gridViewItems;
            this.gridControlItems.Name = "gridControlItems";
            this.gridControlItems.Size = new System.Drawing.Size(636, 316);
            this.gridControlItems.TabIndex = 19;
            this.gridControlItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewItems});
            // 
            // gridViewItems
            // 
            this.gridViewItems.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewItems.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewItems.GridControl = this.gridControlItems;
            this.gridViewItems.Name = "gridViewItems";
            this.gridViewItems.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewItems.OptionsView.RowAutoHeight = true;
            this.gridViewItems.OptionsView.ShowGroupPanel = false;
            this.gridViewItems.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewItems_CustomRowCellEdit);
            // 
            // btnAddTable
            // 
            this.btnAddTable.AllowFocus = false;
            this.btnAddTable.Location = new System.Drawing.Point(482, 86);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(152, 22);
            this.btnAddTable.StyleController = this.layoutControlTextBlock;
            this.btnAddTable.TabIndex = 18;
            this.btnAddTable.Text = "Таблица";
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.AllowFocus = false;
            this.btnAddImage.Location = new System.Drawing.Point(326, 86);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(152, 22);
            this.btnAddImage.StyleController = this.layoutControlTextBlock;
            this.btnAddImage.TabIndex = 17;
            this.btnAddImage.Text = "Изображение";
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // btnAddSubTitle
            // 
            this.btnAddSubTitle.AllowFocus = false;
            this.btnAddSubTitle.Location = new System.Drawing.Point(170, 86);
            this.btnAddSubTitle.Name = "btnAddSubTitle";
            this.btnAddSubTitle.Size = new System.Drawing.Size(152, 22);
            this.btnAddSubTitle.StyleController = this.layoutControlTextBlock;
            this.btnAddSubTitle.TabIndex = 16;
            this.btnAddSubTitle.Text = "Подзаголовок";
            this.btnAddSubTitle.Click += new System.EventHandler(this.btnAddSubTitle_Click);
            // 
            // btnClose
            // 
            this.btnClose.AllowFocus = false;
            this.btnClose.Location = new System.Drawing.Point(486, 440);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(156, 22);
            this.btnClose.StyleController = this.layoutControlTextBlock;
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Отменить";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.AllowFocus = false;
            this.btnSave.Location = new System.Drawing.Point(326, 440);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(156, 22);
            this.btnSave.StyleController = this.layoutControlTextBlock;
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // memoEditTitle
            // 
            this.memoEditTitle.Location = new System.Drawing.Point(71, 6);
            this.memoEditTitle.MaximumSize = new System.Drawing.Size(0, 50);
            this.memoEditTitle.MinimumSize = new System.Drawing.Size(0, 30);
            this.memoEditTitle.Name = "memoEditTitle";
            this.memoEditTitle.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEditTitle.Size = new System.Drawing.Size(571, 47);
            this.memoEditTitle.StyleController = this.layoutControlTextBlock;
            this.memoEditTitle.TabIndex = 14;
            // 
            // btnAddText
            // 
            this.btnAddText.AllowFocus = false;
            this.btnAddText.Location = new System.Drawing.Point(14, 86);
            this.btnAddText.Name = "btnAddText";
            this.btnAddText.Size = new System.Drawing.Size(152, 22);
            this.btnAddText.StyleController = this.layoutControlTextBlock;
            this.btnAddText.TabIndex = 15;
            this.btnAddText.Text = "Текст";
            this.btnAddText.Click += new System.EventHandler(this.btnAddText_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItemSave,
            this.layoutControlItem2,
            this.emptySpaceItemClose,
            this.layoutControlItem1,
            this.layoutControlItemTitle,
            this.layoutControlGroupAddItem,
            this.layoutControlItemGridControlItems});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(648, 468);
            this.Root.TextVisible = false;
            // 
            // emptySpaceItemSave
            // 
            this.emptySpaceItemSave.AllowHotTrack = false;
            this.emptySpaceItemSave.Location = new System.Drawing.Point(0, 434);
            this.emptySpaceItemSave.Name = "emptySpaceItemSave";
            this.emptySpaceItemSave.Size = new System.Drawing.Size(160, 26);
            this.emptySpaceItemSave.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnSave;
            this.layoutControlItem2.Location = new System.Drawing.Point(320, 434);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(160, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItemClose
            // 
            this.emptySpaceItemClose.AllowHotTrack = false;
            this.emptySpaceItemClose.Location = new System.Drawing.Point(160, 434);
            this.emptySpaceItemClose.Name = "emptySpaceItemClose";
            this.emptySpaceItemClose.Size = new System.Drawing.Size(160, 26);
            this.emptySpaceItemClose.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnClose;
            this.layoutControlItem1.Location = new System.Drawing.Point(480, 434);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(160, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItemTitle
            // 
            this.layoutControlItemTitle.Control = this.memoEditTitle;
            this.layoutControlItemTitle.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemTitle.Name = "layoutControlItemTitle";
            this.layoutControlItemTitle.Size = new System.Drawing.Size(640, 51);
            this.layoutControlItemTitle.Text = "Заголовок";
            this.layoutControlItemTitle.TextSize = new System.Drawing.Size(53, 13);
            // 
            // layoutControlGroupAddItem
            // 
            this.layoutControlGroupAddItem.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAddText,
            this.layoutControlItemAddSubTitle,
            this.layoutControlItemAddImage,
            this.layoutControlItemAddTable});
            this.layoutControlGroupAddItem.Location = new System.Drawing.Point(0, 51);
            this.layoutControlGroupAddItem.Name = "layoutControlGroupAddItem";
            this.layoutControlGroupAddItem.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroupAddItem.Size = new System.Drawing.Size(640, 63);
            this.layoutControlGroupAddItem.Text = "Элементы, которые можно добавить";
            // 
            // layoutControlItemAddText
            // 
            this.layoutControlItemAddText.Control = this.btnAddText;
            this.layoutControlItemAddText.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAddText.Name = "layoutControlItemAddText";
            this.layoutControlItemAddText.Size = new System.Drawing.Size(156, 26);
            this.layoutControlItemAddText.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemAddText.TextVisible = false;
            // 
            // layoutControlItemAddSubTitle
            // 
            this.layoutControlItemAddSubTitle.Control = this.btnAddSubTitle;
            this.layoutControlItemAddSubTitle.Location = new System.Drawing.Point(156, 0);
            this.layoutControlItemAddSubTitle.Name = "layoutControlItemAddSubTitle";
            this.layoutControlItemAddSubTitle.Size = new System.Drawing.Size(156, 26);
            this.layoutControlItemAddSubTitle.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemAddSubTitle.TextVisible = false;
            // 
            // layoutControlItemAddImage
            // 
            this.layoutControlItemAddImage.Control = this.btnAddImage;
            this.layoutControlItemAddImage.Location = new System.Drawing.Point(312, 0);
            this.layoutControlItemAddImage.Name = "layoutControlItemAddImage";
            this.layoutControlItemAddImage.Size = new System.Drawing.Size(156, 26);
            this.layoutControlItemAddImage.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemAddImage.TextVisible = false;
            // 
            // layoutControlItemAddTable
            // 
            this.layoutControlItemAddTable.Control = this.btnAddTable;
            this.layoutControlItemAddTable.Location = new System.Drawing.Point(468, 0);
            this.layoutControlItemAddTable.Name = "layoutControlItemAddTable";
            this.layoutControlItemAddTable.Size = new System.Drawing.Size(156, 26);
            this.layoutControlItemAddTable.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemAddTable.TextVisible = false;
            // 
            // layoutControlItemGridControlItems
            // 
            this.layoutControlItemGridControlItems.Control = this.gridControlItems;
            this.layoutControlItemGridControlItems.Location = new System.Drawing.Point(0, 114);
            this.layoutControlItemGridControlItems.Name = "layoutControlItemGridControlItems";
            this.layoutControlItemGridControlItems.Size = new System.Drawing.Size(640, 320);
            this.layoutControlItemGridControlItems.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemGridControlItems.TextVisible = false;
            // 
            // popupMenuItemsObj
            // 
            this.popupMenuItemsObj.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnItemsObjDel)});
            this.popupMenuItemsObj.Manager = this.barManager;
            this.popupMenuItemsObj.Name = "popupMenuItemsObj";
            // 
            // barBtnItemsObjDel
            // 
            this.barBtnItemsObjDel.Caption = "Удалить";
            this.barBtnItemsObjDel.Id = 0;
            this.barBtnItemsObjDel.Name = "barBtnItemsObjDel";
            this.barBtnItemsObjDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnItemsObjDel_ItemClick);
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnItemsObjDel});
            this.barManager.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(648, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 468);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(648, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 468);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(648, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 468);
            // 
            // TextBlockEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 468);
            this.Controls.Add(this.layoutControlTextBlock);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.icon;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(650, 350);
            this.Name = "TextBlockEdit";
            this.Text = "Блок информации";
            this.Load += new System.EventHandler(this.TextBlockEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTextBlock)).EndInit();
            this.layoutControlTextBlock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupAddItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddSubTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGridControlItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuItemsObj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTextBlock;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.MemoEdit memoEditTitle;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTitle;
        private DevExpress.XtraEditors.SimpleButton btnAddTable;
        private DevExpress.XtraEditors.SimpleButton btnAddImage;
        private DevExpress.XtraEditors.SimpleButton btnAddSubTitle;
        private DevExpress.XtraEditors.SimpleButton btnAddText;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupAddItem;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddText;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddSubTitle;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddImage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddTable;
        private DevExpress.XtraGrid.GridControl gridControlItems;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewItems;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGridControlItems;
        private DevExpress.XtraBars.PopupMenu popupMenuItemsObj;
        private DevExpress.XtraBars.BarButtonItem barBtnItemsObjDel;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}