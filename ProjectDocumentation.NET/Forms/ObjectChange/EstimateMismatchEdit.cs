using Core.NET.AutoModels.ConclusionObj;
using DevExpress.XtraEditors;
using System;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class EstimateMismatchEdit : XtraForm
    {
        public EstimateMismatchEdit()
        {
            InitializeComponent();
        }

        public EstimateMismatchEdit(TEstimateMismatches estimateMismatches) : this()
        {
            EstimateMismatch = estimateMismatches;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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

        private void ExpretEdit_Load(object sender, EventArgs e)
        {
            //var collection = await new XPQuery<ExpertTypeXPO>(SessionController.GetSession()).ToListAsync();
            //cmbExpertType.Properties.Items.AddRange(collection);

            if (EstimateMismatch is null)
            {
                EstimateMismatch = new TEstimateMismatches();
            }
            else
            {
                //var projectDocumentsMismatchField = (EstimateMismatch.ProjectDocumentsMismatch?.FirstOrDefault()) ?? new tMismatchExtended();

                //var expertTypeValue = EstimateMismatch.ExpertType.GetEnumXmlAttributeValue();
                //cmbExpertType.EditValue = collection.FirstOrDefault(f => f.Value == expertTypeValue?.ToString());
            }
        }

        public TEstimateMismatches EstimateMismatch { get; private set; }
        public bool IsSave { get; private set; }
    }
}