using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExpertEdit : XtraForm
    {
        public ExpertEdit()
        {
            InitializeComponent();
            ClassicRequiredFields.HighlightExisting(this);
            ClassicRequiredFields.Unmark(txtSecondName);
        }

        public ExpertEdit(TExpert expert) : this()
        {
            Expert = expert;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Expert.FamilyName = ConclusionEdit.GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните поле фамилию");
                Expert.FirstName = ConclusionEdit.GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните поле имя");
                Expert.SecondName = ConclusionEdit.GetStringValue(txtSecondName.EditValue, txtSecondName, isNullAble: true);
                Expert.ExpertType = ConclusionEdit.GetEnumByAttributeValue<TExpertType>(ConclusionEdit.GetValueGeneralizedDictionary(
                        cmbExpertType,
                        message: "Заполните поле направление деятельности эксперта")?.Value);
                Expert.ExpertCertificate = ConclusionEdit.GetStringValue(txtExpertCertificate.EditValue, txtExpertCertificate, "Заполните номер сертификата");
                if (dateExpertCertificateBeginDate.EditValue is null)
                {
                    dateExpertCertificateBeginDate.Focus();
                    XtraMessageBox.Show("Заполните поле дата выдачи",
                        "Информационное сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    Expert.ExpertCertificateBeginDate = dateExpertCertificateBeginDate.DateTime.Date;
                }
                if (dateExpertCertificateEndDate.EditValue is null)
                {
                    dateExpertCertificateEndDate.Focus();
                    XtraMessageBox.Show("Заполните поле дата окончания деятельности",
                        "Информационное сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    Expert.ExpertCertificateEndDate = dateExpertCertificateEndDate.DateTime.Date;
                }
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

        private async void ExpretEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (Expert is null)
            {
                Expert = new TExpert();
            }
            else
            {
                txtFamilyName.EditValue = Expert.FamilyName;
                txtFirstName.EditValue = Expert.FirstName;
                txtSecondName.EditValue = Expert.SecondName;
                cmbExpertType.EditValue = cmbExpertType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType)
                    ?.FirstOrDefault(f => f.Value == Expert.ExpertType.GetEnumXmlAttributeValue()?.ToString());
                txtExpertCertificate.EditValue = Expert.ExpertCertificate;
                dateExpertCertificateBeginDate.EditValue = Expert.ExpertCertificateBeginDate;
                dateExpertCertificateEndDate.EditValue = Expert.ExpertCertificateEndDate;
            }
        }
        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbExpertType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType).ToList());
        }

        private Session Session => SessionController.GetSession();
        public TExpert Expert { get; private set; }
        public bool IsSave { get; private set; }
    }
}