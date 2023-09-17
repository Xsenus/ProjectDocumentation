namespace ProjectDocumentation.NET.Forms
{
    partial class FileLoad
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
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.btnFile = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlItemFile = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItemSave = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItemCancel = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItemFooter = new DevExpress.XtraLayout.EmptySpaceItem();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItemSave = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSave)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.Controls.Add(this.btnFile);
            this.layoutControl.Controls.Add(this.txtName);
            this.layoutControl.Controls.Add(this.btnCancel);
            this.layoutControl.Controls.Add(this.btnSave);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(548, 110);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.Root.AppearanceItemCaption.Options.UseFont = true;
            this.Root.AppearanceItemCaption.Options.UseTextOptions = true;
            this.Root.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemFile,
            this.layoutControlItemName,
            this.emptySpaceItemFooter,
            this.emptySpaceItemSave,
            this.layoutControlItemCancel,
            this.layoutControlItemSave,
            this.emptySpaceItemCancel});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.Root.Size = new System.Drawing.Size(548, 110);
            this.Root.TextVisible = false;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(116, 33);
            this.btnFile.Name = "btnFile";
            this.btnFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.btnFile.Size = new System.Drawing.Size(425, 22);
            this.btnFile.StyleController = this.layoutControl;
            this.btnFile.TabIndex = 4;
            this.btnFile.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnFile_ButtonPressed);
            // 
            // layoutControlItemFile
            // 
            this.layoutControlItemFile.Control = this.btnFile;
            this.layoutControlItemFile.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItemFile.Name = "layoutControlItemFile";
            this.layoutControlItemFile.Size = new System.Drawing.Size(538, 26);
            this.layoutControlItemFile.Text = "Файл XML";
            this.layoutControlItemFile.TextSize = new System.Drawing.Size(97, 16);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(116, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(425, 22);
            this.txtName.StyleController = this.layoutControl;
            this.txtName.TabIndex = 5;
            // 
            // layoutControlItemName
            // 
            this.layoutControlItemName.Control = this.txtName;
            this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemName.Name = "layoutControlItemName";
            this.layoutControlItemName.Size = new System.Drawing.Size(538, 26);
            this.layoutControlItemName.Text = "Наименование";
            this.layoutControlItemName.TextSize = new System.Drawing.Size(97, 16);
            // 
            // emptySpaceItemSave
            // 
            this.emptySpaceItemSave.AllowHotTrack = false;
            this.emptySpaceItemSave.Location = new System.Drawing.Point(0, 69);
            this.emptySpaceItemSave.Name = "emptySpaceItemSave";
            this.emptySpaceItemSave.Size = new System.Drawing.Size(134, 31);
            this.emptySpaceItemSave.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItemCancel
            // 
            this.emptySpaceItemCancel.AllowHotTrack = false;
            this.emptySpaceItemCancel.Location = new System.Drawing.Point(134, 69);
            this.emptySpaceItemCancel.Name = "emptySpaceItemCancel";
            this.emptySpaceItemCancel.Size = new System.Drawing.Size(135, 31);
            this.emptySpaceItemCancel.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItemFooter
            // 
            this.emptySpaceItemFooter.AllowHotTrack = false;
            this.emptySpaceItemFooter.Location = new System.Drawing.Point(0, 52);
            this.emptySpaceItemFooter.Name = "emptySpaceItemFooter";
            this.emptySpaceItemFooter.Size = new System.Drawing.Size(538, 17);
            this.emptySpaceItemFooter.TextSize = new System.Drawing.Size(0, 0);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(410, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 27);
            this.btnCancel.StyleController = this.layoutControl;
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // layoutControlItemCancel
            // 
            this.layoutControlItemCancel.Control = this.btnCancel;
            this.layoutControlItemCancel.Location = new System.Drawing.Point(403, 69);
            this.layoutControlItemCancel.Name = "layoutControlItemCancel";
            this.layoutControlItemCancel.Size = new System.Drawing.Size(135, 31);
            this.layoutControlItemCancel.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemCancel.TextVisible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(276, 76);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 27);
            this.btnSave.StyleController = this.layoutControl;
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // layoutControlItemSave
            // 
            this.layoutControlItemSave.Control = this.btnSave;
            this.layoutControlItemSave.Location = new System.Drawing.Point(269, 69);
            this.layoutControlItemSave.Name = "layoutControlItemSave";
            this.layoutControlItemSave.Size = new System.Drawing.Size(134, 31);
            this.layoutControlItemSave.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemSave.TextVisible = false;
            // 
            // FileLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 110);
            this.Controls.Add(this.layoutControl);
            this.MinimumSize = new System.Drawing.Size(450, 150);
            this.Name = "FileLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузка файла";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSave)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.ButtonEdit btnFile;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFile;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemFooter;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSave;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemCancel;
    }
}