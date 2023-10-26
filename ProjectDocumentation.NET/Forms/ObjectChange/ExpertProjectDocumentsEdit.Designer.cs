namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    partial class ExpertProjectDocumentsEdit
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlObjPart = new DevExpress.XtraLayout.LayoutControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.cmbExpertType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItemSave = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItemClose = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemExaminationResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
            this.richEditControlProjectDocumentsReview = new DevExpress.XtraRichEdit.RichEditControl();
            this.layoutControlItemProjectDocumentsReview = new DevExpress.XtraLayout.LayoutControlItem();
            this.richEditControlProjectDocumentsChanges = new DevExpress.XtraRichEdit.RichEditControl();
            this.layoutControlItemProjectDocumentsChanges = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlObjPart)).BeginInit();
            this.layoutControlObjPart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExpertType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExaminationResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProjectDocumentsReview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProjectDocumentsChanges)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(401, 540);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(193, 22);
            this.btnSave.StyleController = this.layoutControlObjPart;
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // layoutControlObjPart
            // 
            this.layoutControlObjPart.AllowCustomization = false;
            this.layoutControlObjPart.Controls.Add(this.richEditControlProjectDocumentsReview);
            this.layoutControlObjPart.Controls.Add(this.btnClose);
            this.layoutControlObjPart.Controls.Add(this.btnSave);
            this.layoutControlObjPart.Controls.Add(this.cmbExpertType);
            this.layoutControlObjPart.Controls.Add(this.richEditControlProjectDocumentsChanges);
            this.layoutControlObjPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlObjPart.Location = new System.Drawing.Point(0, 0);
            this.layoutControlObjPart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControlObjPart.Name = "layoutControlObjPart";
            this.layoutControlObjPart.Root = this.Root;
            this.layoutControlObjPart.Size = new System.Drawing.Size(798, 568);
            this.layoutControlObjPart.TabIndex = 0;
            this.layoutControlObjPart.Text = "layoutControl1";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(598, 540);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(194, 22);
            this.btnClose.StyleController = this.layoutControlObjPart;
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Отменить";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbExpertType
            // 
            this.cmbExpertType.EditValue = "";
            this.cmbExpertType.Location = new System.Drawing.Point(6, 22);
            this.cmbExpertType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbExpertType.Name = "cmbExpertType";
            this.cmbExpertType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cmbExpertType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbExpertType.Size = new System.Drawing.Size(786, 20);
            this.cmbExpertType.StyleController = this.layoutControlObjPart;
            this.cmbExpertType.TabIndex = 6;
            this.cmbExpertType.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit_ButtonPressed);
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
            this.layoutControlItemExaminationResult,
            this.splitterItem,
            this.layoutControlItemProjectDocumentsReview,
            this.layoutControlItemProjectDocumentsChanges});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(798, 568);
            this.Root.TextVisible = false;
            // 
            // emptySpaceItemSave
            // 
            this.emptySpaceItemSave.AllowHotTrack = false;
            this.emptySpaceItemSave.Location = new System.Drawing.Point(0, 534);
            this.emptySpaceItemSave.Name = "emptySpaceItemSave";
            this.emptySpaceItemSave.Size = new System.Drawing.Size(197, 26);
            this.emptySpaceItemSave.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnSave;
            this.layoutControlItem2.Location = new System.Drawing.Point(395, 534);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(197, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItemClose
            // 
            this.emptySpaceItemClose.AllowHotTrack = false;
            this.emptySpaceItemClose.Location = new System.Drawing.Point(197, 534);
            this.emptySpaceItemClose.Name = "emptySpaceItemClose";
            this.emptySpaceItemClose.Size = new System.Drawing.Size(198, 26);
            this.emptySpaceItemClose.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnClose;
            this.layoutControlItem1.Location = new System.Drawing.Point(592, 534);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(198, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItemExaminationResult
            // 
            this.layoutControlItemExaminationResult.Control = this.cmbExpertType;
            this.layoutControlItemExaminationResult.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItemExaminationResult.CustomizationFormText = "Результат экспертизы *";
            this.layoutControlItemExaminationResult.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemExaminationResult.Name = "layoutControlItemExaminationResult";
            this.layoutControlItemExaminationResult.Size = new System.Drawing.Size(790, 40);
            this.layoutControlItemExaminationResult.Text = "Направление деятельности в области экспертизы проектной документации *";
            this.layoutControlItemExaminationResult.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemExaminationResult.TextSize = new System.Drawing.Size(769, 13);
            // 
            // splitterItem
            // 
            this.splitterItem.AllowHotTrack = true;
            this.splitterItem.Location = new System.Drawing.Point(0, 271);
            this.splitterItem.Name = "splitterItem";
            this.splitterItem.Size = new System.Drawing.Size(790, 10);
            // 
            // richEditControlProjectDocumentsReview
            // 
            this.richEditControlProjectDocumentsReview.Location = new System.Drawing.Point(6, 62);
            this.richEditControlProjectDocumentsReview.Name = "richEditControlProjectDocumentsReview";
            this.richEditControlProjectDocumentsReview.Size = new System.Drawing.Size(786, 211);
            this.richEditControlProjectDocumentsReview.TabIndex = 14;
            this.richEditControlProjectDocumentsReview.Text = "richEditControl1";
            // 
            // layoutControlItemProjectDocumentsReview
            // 
            this.layoutControlItemProjectDocumentsReview.Control = this.richEditControlProjectDocumentsReview;
            this.layoutControlItemProjectDocumentsReview.Location = new System.Drawing.Point(0, 40);
            this.layoutControlItemProjectDocumentsReview.Name = "layoutControlItemProjectDocumentsReview";
            this.layoutControlItemProjectDocumentsReview.Size = new System.Drawing.Size(790, 231);
            this.layoutControlItemProjectDocumentsReview.Text = "Описание основных решений (мероприятий), принятых в проектной документации";
            this.layoutControlItemProjectDocumentsReview.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemProjectDocumentsReview.TextSize = new System.Drawing.Size(769, 13);
            // 
            // richEditControlProjectDocumentsChanges
            // 
            this.richEditControlProjectDocumentsChanges.Location = new System.Drawing.Point(6, 303);
            this.richEditControlProjectDocumentsChanges.Name = "richEditControlProjectDocumentsChanges";
            this.richEditControlProjectDocumentsChanges.Size = new System.Drawing.Size(786, 233);
            this.richEditControlProjectDocumentsChanges.TabIndex = 14;
            this.richEditControlProjectDocumentsChanges.Text = "richEditControl1";
            // 
            // layoutControlItemProjectDocumentsChanges
            // 
            this.layoutControlItemProjectDocumentsChanges.Control = this.richEditControlProjectDocumentsChanges;
            this.layoutControlItemProjectDocumentsChanges.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItemProjectDocumentsChanges.CustomizationFormText = "Описание основных решений (мероприятий), принятых в проектной документации";
            this.layoutControlItemProjectDocumentsChanges.Location = new System.Drawing.Point(0, 281);
            this.layoutControlItemProjectDocumentsChanges.Name = "layoutControlItemProjectDocumentsChanges";
            this.layoutControlItemProjectDocumentsChanges.Size = new System.Drawing.Size(790, 253);
            this.layoutControlItemProjectDocumentsChanges.Text = "Сведения об оперативных изменениях, внесенных заявителем в разделы (подразделы) п" +
    "роектной документации в процессе проведения экспертизы";
            this.layoutControlItemProjectDocumentsChanges.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemProjectDocumentsChanges.TextSize = new System.Drawing.Size(769, 13);
            // 
            // ExpertProjectDocumentsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 568);
            this.Controls.Add(this.layoutControlObjPart);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(800, 350);
            this.Name = "ExpertProjectDocumentsEdit";
            this.Text = "Сведения в части экспертизы проектной документации, подготовленные экспертом";
            this.Load += new System.EventHandler(this.ObjPartEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlObjPart)).EndInit();
            this.layoutControlObjPart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbExpertType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExaminationResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProjectDocumentsReview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProjectDocumentsChanges)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraLayout.LayoutControl layoutControlObjPart;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ComboBoxEdit cmbExpertType;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExaminationResult;
        private DevExpress.XtraLayout.SplitterItem splitterItem;
        private DevExpress.XtraRichEdit.RichEditControl richEditControlProjectDocumentsReview;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemProjectDocumentsReview;
        private DevExpress.XtraRichEdit.RichEditControl richEditControlProjectDocumentsChanges;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemProjectDocumentsChanges;
    }
}