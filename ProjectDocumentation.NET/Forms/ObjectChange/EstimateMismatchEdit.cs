using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.Linq;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class EstimateMismatchEdit : XtraForm
    {
        public bool IsSave { get; private set; }
        public tEstimateMismatches EstimateMismatch { get; private set; }

        public EstimateMismatchEdit()
        {
            InitializeComponent();
        }

        public EstimateMismatchEdit(tEstimateMismatches estimateMismatches) : this()
        {
            EstimateMismatch = estimateMismatches;
        }

        private async void ExpretEdit_Load(object sender, EventArgs e)
        {
            //var collection = await new XPQuery<ExpertTypeXPO>(SessionController.GetSession()).ToListAsync();
            //cmbExpertType.Properties.Items.AddRange(collection);

            if (EstimateMismatch is null)
            {
                EstimateMismatch = new tEstimateMismatches();
            }
            else
            {
                var projectDocumentsMismatchField = EstimateMismatch.ProjectDocumentsMismatch?.FirstOrDefault();

                if (projectDocumentsMismatchField is null)
                {
                    projectDocumentsMismatchField = new tMismatchExtended();
                }

                //var expertTypeValue = EstimateMismatch.ExpertType.GetEnumXmlAttributeValue();
                //cmbExpertType.EditValue = collection.FirstOrDefault(f => f.Value == expertTypeValue?.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //EstimateMismatch.FamilyName = txtFamilyName.Text;
            //EstimateMismatch.FirstName = txtFirstName.Text;
            //EstimateMismatch.SecondName = txtSecondName.Text;
            //EstimateMismatch.ExpertCertificate = txtExpertCertificate.Text;
            //EstimateMismatch.ExpertCertificateBeginDate = dateExpertCertificateBeginDate.DateTime;
            //EstimateMismatch.ExpertCertificateEndDate = dateExpertCertificateEndDate.DateTime;

            //if (cmbEstimateMismatches.EditValue is ExpertTypeXPO expertTypeXPO)
            //{
            //    foreach (tExpertType type in Enum.GetValues(typeof(tExpertType)))
            //    {
            //        if (type.GetEnumXmlAttributeValue()?.ToString()?.Equals(expertTypeXPO.Value) is true)
            //        {
            //            EstimateMismatch.ExpertType = type;
            //            break;
            //        }
            //    }
            //}

            IsSave = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}