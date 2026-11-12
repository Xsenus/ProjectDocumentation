namespace ProjectDocumentation.NET.Forms
{
    partial class ViewForm
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
            this.memoValue = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // memoValue
            // 
            this.memoValue.AccessibleName = "Текст заключения";
            this.memoValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoValue.Location = new System.Drawing.Point(0, 0);
            this.memoValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoValue.Name = "memoValue";
            this.memoValue.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.memoValue.Properties.ReadOnly = true;
            this.memoValue.Properties.WordWrap = true;
            this.memoValue.Size = new System.Drawing.Size(780, 457);
            this.memoValue.TabIndex = 0;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 457);
            this.Controls.Add(this.memoValue);
            this.IconOptions.Image = global::ProjectDocumentation.NET.Properties.Resources.icon;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ViewForm";
            this.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.Text = "Просмотр";
            this.Load += new System.EventHandler(this.ViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoValue.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoValue;
    }
}
