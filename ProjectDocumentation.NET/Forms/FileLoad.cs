using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    public partial class FileLoad : XtraForm
    {
        public ConclusionXPO ObjXPO { get; private set; }

        public FileLoad()
        {
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnFile_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
                else if (e.Button.Kind == ButtonPredefines.Ellipsis)
                {
                    using (var ofd = new OpenFileDialog() { Filter = "*.xml|*.xml" })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            buttonEdit.EditValue = ofd.FileName;
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private async void btnSave_Click(object sender, EventArgs e)
        {
            var name = txtName.EditValue?.ToString();
            var filePath = btnFile.EditValue?.ToString();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                XtraMessageBox.Show("Укажите путь к файлу для сохранения.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                btnFile.Focus();
                return;
            }

            if (FileController.CheckFile(filePath))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    XtraMessageBox.Show("Укажите наименование для сохранения.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                    txtName.Focus();
                    return;
                }

                ObjXPO = await ConclusionController.CreateConclusionByPath(filePath, name);

                if (ObjXPO is null)
                {
                    XtraMessageBox.Show("Ошибка загрузки файла.",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    XtraMessageBox.Show("Файл успешно обработан.",
                                    "Информационное сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    Close();
                }
            }
            else
            {
                XtraMessageBox.Show("Не найден файл по указанному пути.",
                                "Информационное сообщение",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}