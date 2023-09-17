using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.Linq;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExpretEdit : XtraForm
    {
        public bool IsSave { get; private set; }
        public tExpert Expert { get; private set; }

        public ExpretEdit()
        {
            InitializeComponent();
        }

        public ExpretEdit(tExpert expert) : this()
        {
            Expert = expert;
        }

        private async void ExpretEdit_Load(object sender, EventArgs e)
        {
            var collection = await new XPQuery<ExpertTypeXPO>(SessionController.GetSession()).ToListAsync();
            cmbExpertType.Properties.Items.AddRange(collection);

            if (Expert is null)
            {
                Expert = new tExpert();
            }
            else
            {
                txtFamilyName.EditValue = Expert.FamilyName;
                txtFirstName.EditValue = Expert.FirstName;
                txtSecondName.EditValue = Expert.SecondName;
                txtExpertCertificate.EditValue = Expert.ExpertCertificate;
                dateExpertCertificateBeginDate.EditValue = Expert.ExpertCertificateBeginDate;
                dateExpertCertificateEndDate.EditValue = Expert.ExpertCertificateEndDate;
                
                var expertTypeValue = Expert.ExpertType.GetEnumXmlAttributeValue();
                cmbExpertType.EditValue = collection.FirstOrDefault(f => f.Value == expertTypeValue?.ToString());
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Expert.FamilyName = txtFamilyName.Text;
            Expert.FirstName = txtFirstName.Text;
            Expert.SecondName = txtSecondName.Text;
            Expert.ExpertCertificate = txtExpertCertificate.Text;
            Expert.ExpertCertificateBeginDate = dateExpertCertificateBeginDate.DateTime;
            Expert.ExpertCertificateEndDate = dateExpertCertificateEndDate.DateTime;

            if (cmbExpertType.EditValue is ExpertTypeXPO expertTypeXPO)
            {
                foreach (tExpertType type in Enum.GetValues(typeof(tExpertType)))
                {
                    if (type.GetEnumXmlAttributeValue()?.ToString()?.Equals(expertTypeXPO.Value) is true)
                    {
                        Expert.ExpertType = type;
                        break;
                    }
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