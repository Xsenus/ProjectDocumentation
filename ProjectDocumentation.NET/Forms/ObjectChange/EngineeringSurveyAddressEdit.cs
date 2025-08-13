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
        public EngineeringSurveyAddressEdit()
        {
            InitializeComponent();
        }

        public EngineeringSurveyAddressEdit(TEngineeringSurveyAddress address) : this()
        {
            EngineeringSurveyAddress = address;
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
                EngineeringSurveyAddress.EngineeringSurveyDistrict = memoEngineeringSurveyDistrict.Text;
                EngineeringSurveyAddress.EngineeringSurveyRegion = ConclusionEdit.GetEnumByAttributeValue<TRegionsRf>(ConclusionEdit.GetValueGeneralizedDictionary(
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
                EngineeringSurveyAddress = new TEngineeringSurveyAddress();
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

        private Session Session => SessionController.GetSession();
        public TEngineeringSurveyAddress EngineeringSurveyAddress { get; private set; }
        public bool IsSave { get; private set; }
    }
}