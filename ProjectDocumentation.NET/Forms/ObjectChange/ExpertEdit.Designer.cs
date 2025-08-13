namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    partial class ExpertEdit
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
            this.layoutControlExpert = new DevExpress.XtraLayout.LayoutControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtFamilyName = new DevExpress.XtraEditors.TextEdit();
            this.txtFirstName = new DevExpress.XtraEditors.TextEdit();
            this.txtSecondName = new DevExpress.XtraEditors.TextEdit();
            this.cmbExpertType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtExpertCertificate = new DevExpress.XtraEditors.TextEdit();
            this.dateExpertCertificateBeginDate = new DevExpress.XtraEditors.DateEdit();
            this.dateExpertCertificateEndDate = new DevExpress.XtraEditors.DateEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemFamilyName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemFirstName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemSecondName = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItemExpertType = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItemExpertCertificate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemExpertCertificateBeginDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemExpertCertificateEndDate = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlExpert)).BeginInit();
            this.layoutControlExpert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFamilyName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirstName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSecondName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExpertType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpertCertificate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateBeginDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateBeginDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFamilyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFirstName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSecondName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificateBeginDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificateEndDate)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlExpert
            // 
            this.layoutControlExpert.Controls.Add(this.btnSave);
            this.layoutControlExpert.Controls.Add(this.btnCancel);
            this.layoutControlExpert.Controls.Add(this.txtFamilyName);
            this.layoutControlExpert.Controls.Add(this.txtFirstName);
            this.layoutControlExpert.Controls.Add(this.txtSecondName);
            this.layoutControlExpert.Controls.Add(this.cmbExpertType);
            this.layoutControlExpert.Controls.Add(this.txtExpertCertificate);
            this.layoutControlExpert.Controls.Add(this.dateExpertCertificateBeginDate);
            this.layoutControlExpert.Controls.Add(this.dateExpertCertificateEndDate);
            this.layoutControlExpert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlExpert.Location = new System.Drawing.Point(0, 0);
            this.layoutControlExpert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControlExpert.Name = "layoutControlExpert";
            this.layoutControlExpert.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize;
            this.layoutControlExpert.Root = this.Root;
            this.layoutControlExpert.Size = new System.Drawing.Size(686, 253);
            this.layoutControlExpert.TabIndex = 0;
            this.layoutControlExpert.Text = "layoutControlExpert";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(344, 225);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(166, 22);
            this.btnSave.StyleController = this.layoutControlExpert;
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(514, 225);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 22);
            this.btnCancel.StyleController = this.layoutControlExpert;
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtFamilyName
            // 
            this.txtFamilyName.Location = new System.Drawing.Point(69, 6);
            this.txtFamilyName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFamilyName.Name = "txtFamilyName";
            this.txtFamilyName.Size = new System.Drawing.Size(611, 20);
            this.txtFamilyName.StyleController = this.layoutControlExpert;
            this.txtFamilyName.TabIndex = 6;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(44, 30);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(636, 20);
            this.txtFirstName.StyleController = this.layoutControlExpert;
            this.txtFirstName.TabIndex = 7;
            // 
            // txtSecondName
            // 
            this.txtSecondName.Location = new System.Drawing.Point(74, 54);
            this.txtSecondName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSecondName.Name = "txtSecondName";
            this.txtSecondName.Size = new System.Drawing.Size(606, 20);
            this.txtSecondName.StyleController = this.layoutControlExpert;
            this.txtSecondName.TabIndex = 8;
            // 
            // cmbExpertType
            // 
            this.cmbExpertType.EditValue = "";
            this.cmbExpertType.Location = new System.Drawing.Point(216, 78);
            this.cmbExpertType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbExpertType.Name = "cmbExpertType";
            this.cmbExpertType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cmbExpertType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbExpertType.Size = new System.Drawing.Size(464, 20);
            this.cmbExpertType.StyleController = this.layoutControlExpert;
            this.cmbExpertType.TabIndex = 6;
            // 
            // txtExpertCertificate
            // 
            this.txtExpertCertificate.Location = new System.Drawing.Point(218, 102);
            this.txtExpertCertificate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExpertCertificate.Name = "txtExpertCertificate";
            this.txtExpertCertificate.Size = new System.Drawing.Size(462, 20);
            this.txtExpertCertificate.StyleController = this.layoutControlExpert;
            this.txtExpertCertificate.TabIndex = 6;
            // 
            // dateExpertCertificateBeginDate
            // 
            this.dateExpertCertificateBeginDate.EditValue = null;
            this.dateExpertCertificateBeginDate.Location = new System.Drawing.Point(93, 126);
            this.dateExpertCertificateBeginDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateExpertCertificateBeginDate.Name = "dateExpertCertificateBeginDate";
            this.dateExpertCertificateBeginDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateExpertCertificateBeginDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateExpertCertificateBeginDate.Size = new System.Drawing.Size(587, 20);
            this.dateExpertCertificateBeginDate.StyleController = this.layoutControlExpert;
            this.dateExpertCertificateBeginDate.TabIndex = 9;
            // 
            // dateExpertCertificateEndDate
            // 
            this.dateExpertCertificateEndDate.EditValue = null;
            this.dateExpertCertificateEndDate.Location = new System.Drawing.Point(183, 150);
            this.dateExpertCertificateEndDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateExpertCertificateEndDate.Name = "dateExpertCertificateEndDate";
            this.dateExpertCertificateEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateExpertCertificateEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateExpertCertificateEndDate.Size = new System.Drawing.Size(497, 20);
            this.dateExpertCertificateEndDate.StyleController = this.layoutControlExpert;
            this.dateExpertCertificateEndDate.TabIndex = 10;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemFamilyName,
            this.layoutControlItemFirstName,
            this.layoutControlItemSecondName,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem2,
            this.layoutControlItemExpertType,
            this.emptySpaceItem3,
            this.layoutControlItemExpertCertificate,
            this.layoutControlItemExpertCertificateBeginDate,
            this.layoutControlItemExpertCertificateEndDate});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(686, 253);
            this.Root.TextVisible = false;
            // 
            // layoutControlItemFamilyName
            // 
            this.layoutControlItemFamilyName.Control = this.txtFamilyName;
            this.layoutControlItemFamilyName.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemFamilyName.Name = "layoutControlItemFamilyName";
            this.layoutControlItemFamilyName.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemFamilyName.Text = "Фамилия *";
            this.layoutControlItemFamilyName.TextSize = new System.Drawing.Size(53, 13);
            // 
            // layoutControlItemFirstName
            // 
            this.layoutControlItemFirstName.Control = this.txtFirstName;
            this.layoutControlItemFirstName.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemFirstName.Name = "layoutControlItemFirstName";
            this.layoutControlItemFirstName.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemFirstName.Text = "Имя *";
            this.layoutControlItemFirstName.TextSize = new System.Drawing.Size(28, 13);
            // 
            // layoutControlItemSecondName
            // 
            this.layoutControlItemSecondName.Control = this.txtSecondName;
            this.layoutControlItemSecondName.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItemSecondName.Name = "layoutControlItemSecondName";
            this.layoutControlItemSecondName.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemSecondName.Text = "Отчество *";
            this.layoutControlItemSecondName.TextSize = new System.Drawing.Size(58, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 219);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(169, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnSave;
            this.layoutControlItem1.Location = new System.Drawing.Point(338, 219);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(170, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnCancel;
            this.layoutControlItem2.Location = new System.Drawing.Point(508, 219);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(170, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(169, 219);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(169, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItemExpertType
            // 
            this.layoutControlItemExpertType.Control = this.cmbExpertType;
            this.layoutControlItemExpertType.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItemExpertType.CustomizationFormText = "Результат экспертизы *";
            this.layoutControlItemExpertType.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItemExpertType.Name = "layoutControlItemExpertType";
            this.layoutControlItemExpertType.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemExpertType.Text = "Направление деятельности эксперта *";
            this.layoutControlItemExpertType.TextSize = new System.Drawing.Size(200, 13);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 168);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(678, 51);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItemExpertCertificate
            // 
            this.layoutControlItemExpertCertificate.Control = this.txtExpertCertificate;
            this.layoutControlItemExpertCertificate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItemExpertCertificate.CustomizationFormText = "Фамилия *";
            this.layoutControlItemExpertCertificate.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItemExpertCertificate.Name = "layoutControlItemExpertCertificate";
            this.layoutControlItemExpertCertificate.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemExpertCertificate.Text = "Номер квалификационного аттестата *";
            this.layoutControlItemExpertCertificate.TextSize = new System.Drawing.Size(202, 13);
            // 
            // layoutControlItemExpertCertificateBeginDate
            // 
            this.layoutControlItemExpertCertificateBeginDate.Control = this.dateExpertCertificateBeginDate;
            this.layoutControlItemExpertCertificateBeginDate.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItemExpertCertificateBeginDate.Name = "layoutControlItemExpertCertificateBeginDate";
            this.layoutControlItemExpertCertificateBeginDate.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemExpertCertificateBeginDate.Text = "Дата выдачи *";
            this.layoutControlItemExpertCertificateBeginDate.TextSize = new System.Drawing.Size(77, 13);
            // 
            // layoutControlItemExpertCertificateEndDate
            // 
            this.layoutControlItemExpertCertificateEndDate.Control = this.dateExpertCertificateEndDate;
            this.layoutControlItemExpertCertificateEndDate.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItemExpertCertificateEndDate.Name = "layoutControlItemExpertCertificateEndDate";
            this.layoutControlItemExpertCertificateEndDate.Size = new System.Drawing.Size(678, 24);
            this.layoutControlItemExpertCertificateEndDate.Text = "Дата окончания деятельности *";
            this.layoutControlItemExpertCertificateEndDate.TextSize = new System.Drawing.Size(167, 13);
            // 
            // ExpretEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 253);
            this.Controls.Add(this.layoutControlExpert);
            this.IconOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.icon;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(688, 285);
            this.Name = "ExpretEdit";
            this.Text = "Эксперт";
            this.Load += new System.EventHandler(this.ExpretEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlExpert)).EndInit();
            this.layoutControlExpert.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFamilyName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirstName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSecondName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExpertType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpertCertificate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateBeginDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateBeginDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateExpertCertificateEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFamilyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFirstName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSecondName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificateBeginDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpertCertificateEndDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlExpert;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit txtFamilyName;
        private DevExpress.XtraEditors.TextEdit txtFirstName;
        private DevExpress.XtraEditors.TextEdit txtSecondName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFamilyName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFirstName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSecondName;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbExpertType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExpertType;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraEditors.TextEdit txtExpertCertificate;
        private DevExpress.XtraEditors.DateEdit dateExpertCertificateBeginDate;
        private DevExpress.XtraEditors.DateEdit dateExpertCertificateEndDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExpertCertificate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExpertCertificateBeginDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExpertCertificateEndDate;
    }
}