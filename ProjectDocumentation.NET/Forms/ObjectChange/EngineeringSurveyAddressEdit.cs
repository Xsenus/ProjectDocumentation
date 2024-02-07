using Core.NET.AutoModels.ConclusionObj;
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
    public partial class EngineeringSurveyAddressEdit : XtraForm
    {
        private Session Session => SessionController.GetSession();
        public bool IsSave { get; private set; }
        public tEngineeringSurveyAddress EngineeringSurveyAddress { get; private set; }

        public EngineeringSurveyAddressEdit()
        {
            InitializeComponent();
        }

        public EngineeringSurveyAddressEdit(tEngineeringSurveyAddress address) : this()
        {
            EngineeringSurveyAddress = address;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbEngineeringSurveyRegion.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region).ToList());
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (EngineeringSurveyAddress is null)
            {
                EngineeringSurveyAddress = new tEngineeringSurveyAddress();
            }
            else
            {
                cmbEngineeringSurveyRegion.EditValue = cmbEngineeringSurveyRegion.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.Region)
                    ?.FirstOrDefault(f => f.ValueInt == EngineeringSurveyAddress?.EngineeringSurveyRegion.GetEnumXmlAttributeValueInt());

                memoEngineeringSurveyDistrict.EditValue = EngineeringSurveyAddress.EngineeringSurveyDistrict;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EngineeringSurveyAddress.EngineeringSurveyDistrict = memoEngineeringSurveyDistrict.Text;
                EngineeringSurveyAddress.EngineeringSurveyRegion = ConclusionEdit.GetEnumByAttributeValue<tRegionsRF>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbEngineeringSurveyRegion,
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