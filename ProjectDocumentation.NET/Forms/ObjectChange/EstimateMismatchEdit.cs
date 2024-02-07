using Core.NET.AutoModels.ConclusionObj;
using DevExpress.XtraEditors;
using System;

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

        private void ExpretEdit_Load(object sender, EventArgs e)
        {
            //var collection = await new XPQuery<ExpertTypeXPO>(SessionController.GetSession()).ToListAsync();
            //cmbExpertType.Properties.Items.AddRange(collection);

            if (EstimateMismatch is null)
            {
                EstimateMismatch = new tEstimateMismatches();
            }
            else
            {
                //var projectDocumentsMismatchField = (EstimateMismatch.ProjectDocumentsMismatch?.FirstOrDefault()) ?? new tMismatchExtended();

                //var expertTypeValue = EstimateMismatch.ExpertType.GetEnumXmlAttributeValue();
                //cmbExpertType.EditValue = collection.FirstOrDefault(f => f.Value == expertTypeValue?.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}