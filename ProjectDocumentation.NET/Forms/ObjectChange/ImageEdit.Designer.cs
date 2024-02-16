namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    partial class ImageEdit
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.layoutControlTextBlock = new DevExpress.XtraLayout.LayoutControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditComment = new DevExpress.XtraEditors.MemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItemSave = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItemClose = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemComment = new DevExpress.XtraLayout.LayoutControlItem();
            this.cmbImageType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlItemtImageType = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItemAdd = new DevExpress.XtraLayout.LayoutControlItem();
            this.pictureEditImageData = new DevExpress.XtraEditors.PictureEdit();
            this.layoutControlItemImageData = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTextBlock)).BeginInit();
            this.layoutControlTextBlock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditComment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImageType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemtImageType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditImageData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemImageData)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTextBlock
            // 
            this.layoutControlTextBlock.AllowCustomization = false;
            this.layoutControlTextBlock.Controls.Add(this.btnClose);
            this.layoutControlTextBlock.Controls.Add(this.btnSave);
            this.layoutControlTextBlock.Controls.Add(this.memoEditComment);
            this.layoutControlTextBlock.Controls.Add(this.cmbImageType);
            this.layoutControlTextBlock.Controls.Add(this.btnAdd);
            this.layoutControlTextBlock.Controls.Add(this.pictureEditImageData);
            this.layoutControlTextBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlTextBlock.Location = new System.Drawing.Point(0, 0);
            this.layoutControlTextBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControlTextBlock.Name = "layoutControlTextBlock";
            this.layoutControlTextBlock.Root = this.Root;
            this.layoutControlTextBlock.Size = new System.Drawing.Size(448, 318);
            this.layoutControlTextBlock.TabIndex = 0;
            this.layoutControlTextBlock.Text = "layoutControl1";
            // 
            // btnClose
            // 
            this.btnClose.AllowFocus = false;
            this.btnClose.Location = new System.Drawing.Point(336, 290);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 22);
            this.btnClose.StyleController = this.layoutControlTextBlock;
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Отменить";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.AllowFocus = false;
            this.btnSave.Location = new System.Drawing.Point(226, 290);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 22);
            this.btnSave.StyleController = this.layoutControlTextBlock;
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // memoEditComment
            // 
            this.memoEditComment.Location = new System.Drawing.Point(106, 236);
            this.memoEditComment.MaximumSize = new System.Drawing.Size(0, 50);
            this.memoEditComment.MinimumSize = new System.Drawing.Size(0, 30);
            this.memoEditComment.Name = "memoEditComment";
            this.memoEditComment.Properties.AllowFocused = false;
            this.memoEditComment.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEditComment.Size = new System.Drawing.Size(336, 50);
            this.memoEditComment.StyleController = this.layoutControlTextBlock;
            this.memoEditComment.TabIndex = 14;
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
            this.layoutControlItemComment,
            this.layoutControlItemtImageType,
            this.layoutControlItemAdd,
            this.layoutControlItemImageData});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(448, 318);
            this.Root.TextVisible = false;
            // 
            // emptySpaceItemSave
            // 
            this.emptySpaceItemSave.AllowHotTrack = false;
            this.emptySpaceItemSave.Location = new System.Drawing.Point(0, 284);
            this.emptySpaceItemSave.Name = "emptySpaceItemSave";
            this.emptySpaceItemSave.Size = new System.Drawing.Size(110, 26);
            this.emptySpaceItemSave.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnSave;
            this.layoutControlItem2.Location = new System.Drawing.Point(220, 284);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(110, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItemClose
            // 
            this.emptySpaceItemClose.AllowHotTrack = false;
            this.emptySpaceItemClose.Location = new System.Drawing.Point(110, 284);
            this.emptySpaceItemClose.Name = "emptySpaceItemClose";
            this.emptySpaceItemClose.Size = new System.Drawing.Size(110, 26);
            this.emptySpaceItemClose.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnClose;
            this.layoutControlItem1.Location = new System.Drawing.Point(330, 284);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(110, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItemComment
            // 
            this.layoutControlItemComment.Control = this.memoEditComment;
            this.layoutControlItemComment.Location = new System.Drawing.Point(0, 230);
            this.layoutControlItemComment.Name = "layoutControlItemComment";
            this.layoutControlItemComment.Size = new System.Drawing.Size(440, 54);
            this.layoutControlItemComment.Text = "Комментарий";
            this.layoutControlItemComment.TextSize = new System.Drawing.Size(88, 13);
            // 
            // cmbImageType
            // 
            this.cmbImageType.EditValue = "";
            this.cmbImageType.Location = new System.Drawing.Point(106, 212);
            this.cmbImageType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbImageType.Name = "cmbImageType";
            this.cmbImageType.Properties.AllowFocused = false;
            this.cmbImageType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.cmbImageType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbImageType.Size = new System.Drawing.Size(336, 20);
            this.cmbImageType.StyleController = this.layoutControlTextBlock;
            this.cmbImageType.TabIndex = 6;
            // 
            // layoutControlItemtImageType
            // 
            this.layoutControlItemtImageType.Control = this.cmbImageType;
            this.layoutControlItemtImageType.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItemtImageType.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItemtImageType.Location = new System.Drawing.Point(0, 206);
            this.layoutControlItemtImageType.Name = "layoutControlItemtImageType";
            this.layoutControlItemtImageType.Size = new System.Drawing.Size(440, 24);
            this.layoutControlItemtImageType.Text = "Тип изображения";
            this.layoutControlItemtImageType.TextSize = new System.Drawing.Size(88, 13);
            // 
            // btnAdd
            // 
            this.btnAdd.AllowFocus = false;
            this.btnAdd.Location = new System.Drawing.Point(6, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(436, 22);
            this.btnAdd.StyleController = this.layoutControlTextBlock;
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // layoutControlItemAdd
            // 
            this.layoutControlItemAdd.Control = this.btnAdd;
            this.layoutControlItemAdd.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAdd.Name = "layoutControlItemAdd";
            this.layoutControlItemAdd.Size = new System.Drawing.Size(440, 26);
            this.layoutControlItemAdd.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemAdd.TextVisible = false;
            // 
            // pictureEditImageData
            // 
            this.pictureEditImageData.Location = new System.Drawing.Point(6, 32);
            this.pictureEditImageData.Name = "pictureEditImageData";
            this.pictureEditImageData.Properties.AllowFocused = false;
            this.pictureEditImageData.Properties.ShowEditMenuItem = DevExpress.Utils.DefaultBoolean.False;
            this.pictureEditImageData.Properties.ShowMenu = false;
            this.pictureEditImageData.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.False;
            this.pictureEditImageData.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEditImageData.Size = new System.Drawing.Size(436, 176);
            this.pictureEditImageData.StyleController = this.layoutControlTextBlock;
            this.pictureEditImageData.TabIndex = 17;
            // 
            // layoutControlItemImageData
            // 
            this.layoutControlItemImageData.Control = this.pictureEditImageData;
            this.layoutControlItemImageData.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItemImageData.Name = "layoutControlItemImageData";
            this.layoutControlItemImageData.Size = new System.Drawing.Size(440, 180);
            this.layoutControlItemImageData.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemImageData.TextVisible = false;
            // 
            // ImageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 318);
            this.Controls.Add(this.layoutControlTextBlock);
            this.IconOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.icon;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(450, 350);
            this.Name = "ImageEdit";
            this.Text = "Изображение";
            this.Load += new System.EventHandler(this.ImageEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTextBlock)).EndInit();
            this.layoutControlTextBlock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditComment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImageType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemtImageType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditImageData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemImageData)).EndInit();
            this.ResumeLayout(false);

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
        private DevExpress.XtraEditors.MemoEdit memoEditComment;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemComment;
        private DevExpress.XtraEditors.ComboBoxEdit cmbImageType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemtImageType;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAdd;
        private DevExpress.XtraEditors.PictureEdit pictureEditImageData;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemImageData;
    }
}