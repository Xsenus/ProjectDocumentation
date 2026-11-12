using Core.NET.AutoModels.ExplanatoryNoteObj;
using DevExpress.XtraEditors;
using System;
using System.IO;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ImageEdit : XtraForm
    {
        public bool IsSave { get; private set; }
        public tImage Image { get; private set; }

        public ImageEdit()
        {
            InitializeComponent();

            foreach (tImageTypes item in Enum.GetValues(typeof(tImageTypes)))
            {
                cmbImageType.Properties.Items.Add(item);
            }
            cmbImageType.EditValue = tImageTypes.png;
        }

        public ImageEdit(tImage tImage) : this()
        {
            Image = tImage;
        }

        private void LoadImage(string base64String)
        {
            if (!string.IsNullOrWhiteSpace(base64String))
            {
                var imageBytes = Convert.FromBase64String(base64String);
                var image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
                pictureEditImageData.Image = image;
            }            
        }

        private byte[] GetImageBytes(System.Drawing.Image image, tImageTypes tImageTypes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                switch (tImageTypes)
                {
                    case tImageTypes.jpg:
                        imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;

                    case tImageTypes.gif:
                        imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                        break;

                    case tImageTypes.png:
                        imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                        break;

                    case tImageTypes.jpeg:
                        imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                }

                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }

        private void LoadImage(byte[] base64String)
        {
            if (base64String != null)
            {
                var image = System.Drawing.Image.FromStream(new MemoryStream(base64String));
                pictureEditImageData.Image = image;
            }
        }

        private void ImageEdit_Load(object sender, EventArgs e)
        {
            if (Image is null)
            {
                Image = new tImage();
            }

            memoEditComment.EditValue = Image.Comment;
            cmbImageType.EditValue = Image.Type;
            LoadImage(Image.ImageData);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Image.Comment = ConclusionEdit.GetStringValue(memoEditComment.EditValue, memoEditComment, "Заполните комментарий.");
                if (cmbImageType.EditValue is tImageTypes tImageTypes)
                {
                    Image.Type = tImageTypes;
                }
                Image.ImageData = GetImageBytes(pictureEditImageData.Image, Image.Type);
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

        public string GetBase64StringFromFile(string filePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new XtraOpenFileDialog { CheckFileExists = true })
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*";
                openFileDialog.Title = "Select an Image File";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    var base64String = GetBase64StringFromFile(filePath);
                    LoadImage(base64String);

                    var fileExtension = new FileInfo(filePath);

                    switch (fileExtension.Extension?.ToLower()?.Replace(".", ""))
                    {
                        case "jpg":
                            cmbImageType.EditValue = tImageTypes.jpg;
                            break;

                        case "jpeg":
                            cmbImageType.EditValue = tImageTypes.jpg;
                            break;

                        case "png":
                            cmbImageType.EditValue = tImageTypes.png;
                            break;

                        case "gif":
                            cmbImageType.EditValue = tImageTypes.gif;
                            break;

                        default:
                            cmbImageType.EditValue = tImageTypes.png;
                            break;
                    }
                }
            }
        }
    }
}
