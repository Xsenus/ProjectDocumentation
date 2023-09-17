namespace ProjectDocumentation.NET.Forms
{
    partial class MainForm
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
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barBtnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDel = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnLoad = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnFill = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1022, 587);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(7, 7);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(1008, 573);
            this.gridControl.TabIndex = 4;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsView.RowAutoHeight = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridView_PopupMenuShowing);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.Root.Size = new System.Drawing.Size(1022, 587);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1012, 577);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnAdd),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnEdit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnDel),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnLoad, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnFill, true)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            // 
            // barBtnAdd
            // 
            this.barBtnAdd.Caption = "Добавить";
            this.barBtnAdd.Id = 0;
            this.barBtnAdd.ImageOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.addfile_16x16;
            this.barBtnAdd.ImageOptions.LargeImage = global::ProjectDocumentation.NET.Properties.Resources.addfile_32x32;
            this.barBtnAdd.Name = "barBtnAdd";
            // 
            // barBtnEdit
            // 
            this.barBtnEdit.Caption = "Изменить";
            this.barBtnEdit.Id = 1;
            this.barBtnEdit.ImageOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.edit_16x16;
            this.barBtnEdit.ImageOptions.LargeImage = global::ProjectDocumentation.NET.Properties.Resources.edit_32x32;
            this.barBtnEdit.Name = "barBtnEdit";
            this.barBtnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnEdit_ItemClick);
            // 
            // barBtnDel
            // 
            this.barBtnDel.Caption = "Удалить";
            this.barBtnDel.Id = 2;
            this.barBtnDel.ImageOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.deletelist_16x16;
            this.barBtnDel.ImageOptions.LargeImage = global::ProjectDocumentation.NET.Properties.Resources.deletelist_32x32;
            this.barBtnDel.Name = "barBtnDel";
            this.barBtnDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnDel_ItemClick);
            // 
            // barBtnLoad
            // 
            this.barBtnLoad.Caption = "Загрузить XML";
            this.barBtnLoad.Id = 4;
            this.barBtnLoad.ImageOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.publish_16x16;
            this.barBtnLoad.ImageOptions.LargeImage = global::ProjectDocumentation.NET.Properties.Resources.publish_32x32;
            this.barBtnLoad.Name = "barBtnLoad";
            this.barBtnLoad.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnLoad_ItemClick);
            // 
            // barBtnFill
            // 
            this.barBtnFill.Caption = "Сформировать XML";
            this.barBtnFill.Id = 3;
            this.barBtnFill.ImageOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.drilldownonarguments_pie_16x16;
            this.barBtnFill.ImageOptions.LargeImage = global::ProjectDocumentation.NET.Properties.Resources.drilldownonarguments_pie_32x32;
            this.barBtnFill.Name = "barBtnFill";
            this.barBtnFill.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnFill_ItemClick);
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnAdd,
            this.barBtnEdit,
            this.barBtnDel,
            this.barBtnFill,
            this.barBtnLoad});
            this.barManager.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1022, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 587);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1022, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 587);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1022, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 587);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 587);
            this.Controls.Add(this.layoutControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Экспертизы России";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarButtonItem barBtnAdd;
        private DevExpress.XtraBars.BarButtonItem barBtnEdit;
        private DevExpress.XtraBars.BarButtonItem barBtnDel;
        private DevExpress.XtraBars.BarButtonItem barBtnLoad;
        private DevExpress.XtraBars.BarButtonItem barBtnFill;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}

