using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class PostAddressEdit : XtraForm
    {
        private Session Session => SessionController.GetSession();
        public bool IsSave { get; private set; }
        public tPostAddress PostAddress { get; private set; }

        public PostAddressEdit()
        {
            InitializeComponent();
        }

        public PostAddressEdit(tPostAddress address) : this()
        {
            PostAddress = address;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbRegion.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region).ToList());
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (PostAddress is null)
            {
                PostAddress = new tPostAddress();
            }
            else
            {
                txtCountry.Text = PostAddress.Country;

                cmbRegion.EditValue = cmbRegion.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region)
                    ?.FirstOrDefault(f => f.ValueInt == PostAddress?.Region.GetEnumXmlAttributeValueInt());

                txtPostIndex.EditValue = PostAddress.PostIndex;
                txtDistrict.EditValue = PostAddress.District;
                txtCity.EditValue = PostAddress.City;
                txtSettlement.EditValue = PostAddress.Settlement;
                txtStreet.EditValue = PostAddress.Street;
                txtBuilding.EditValue = PostAddress.Building;
                txtRoom.EditValue = PostAddress.Room;
                txtNote.EditValue = PostAddress.Note;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PostAddress.Country = txtCountry.Text;
                PostAddress.PostIndex = txtPostIndex.Text;
                PostAddress.District = txtDistrict.Text;
                PostAddress.City = txtCity.Text;
                PostAddress.Settlement = txtSettlement.Text;
                PostAddress.Street = txtStreet.Text;
                PostAddress.Building = txtBuilding.Text;
                PostAddress.Room = txtRoom.Text;
                PostAddress.Note = txtNote.Text;
                PostAddress.Region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbRegion,
                    message: "Заполните поле региона Субъекта Российской Федерации")?.Value);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    ex.Message,
                    "Информационное сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void cmbRegion_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }
    }
}