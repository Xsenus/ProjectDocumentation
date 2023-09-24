using Core.NET.AutoModels;
using Core.NET.Extensions;
using DevExpress.XtraEditors;
using System;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class AddressEdit : XtraForm
    {
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

        private void Form_Load(object sender, EventArgs e)
        {
            foreach(tRegionsRF type in Enum.GetValues(typeof(tRegionsRF)))
            {
                cmbRegion.Properties.Items.Add(type.GetEnumXmlAttributeValue());
            }

            if (Address is null)
            {
                Address = new tAddress();
            }
            else
            {
                txtCountry.Text = Address.Country;
                cmbRegion.EditValue = Address.Region.GetEnumXmlAttributeValue();
                txtDistrict.EditValue = Address.District;
                txtCity.EditValue = Address.City;
                //txtSettlement.EditValue = Address.Settlement;
                txtStreet.EditValue = Address.Street;
                txtBuilding.EditValue = Address.Building;
                txtRoom.EditValue = Address.Room;
                txtNote.EditValue = Address.Note;
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Address.Country = txtCountry.Text;
            Address.District = txtDistrict.Text;
            Address.City = txtCity.Text;
            //Address.Settlement = txtSettlement.EditValue;
            Address.Street = txtStreet.Text;
            Address.Building = txtBuilding.Text;
            Address.Room = txtRoom.Text;
            Address.Note = txtNote.Text;

            foreach (tRegionsRF type in Enum.GetValues(typeof(tRegionsRF)))
            {
                if (type.GetEnumXmlAttributeValue()?.ToString()?.Equals(cmbRegion.Text) is true)
                {
                    Address.Region = type;
                    break;
                }
            }

            IsSave = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}