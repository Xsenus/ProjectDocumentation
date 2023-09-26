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
    public partial class AddressEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();
        public bool IsSave { get; private set; }
        public tAddress Address { get; private set; }

        public AddressEdit()
        {
            InitializeComponent();
        }

        public AddressEdit(tAddress address) : this()
        {
            Address = address;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            cmbRegion.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region).ToList());
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Address is null)
            {
                Address = new tAddress();
            }
            else
            {
                txtCountry.Text = Address.Country;

                cmbRegion.EditValue = cmbRegion.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region)
                    ?.FirstOrDefault(f => f.ValueInt == Address?.Region.GetEnumXmlAttributeValueInt());

                txtDistrict.EditValue = Address.District;
                txtCity.EditValue = Address.City;
                txtSettlement.EditValue = Address.Settlement;
                txtStreet.EditValue = Address.Street;
                txtBuilding.EditValue = Address.Building;
                txtRoom.EditValue = Address.Room;
                txtNote.EditValue = Address.Note;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Address.Country = txtCountry.Text;
                Address.District = txtDistrict.Text;
                Address.City = txtCity.Text;
                Address.Settlement = txtSettlement.Text;
                Address.Street = txtStreet.Text;
                Address.Building = txtBuilding.Text;
                Address.Room = txtRoom.Text;
                Address.Note = txtNote.Text;
                Address.Region = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbRegion,
                    message: "Заполните поле региона Субъекта Российской Федерации").Value);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

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