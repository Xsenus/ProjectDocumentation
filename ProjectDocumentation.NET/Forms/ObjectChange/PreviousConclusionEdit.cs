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
    public partial class PreviousConclusionEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tPreviousConclusion PreviousConclusion { get; private set; }

        public PreviousConclusionEdit()
        {
            InitializeComponent();
        }

        public PreviousConclusionEdit(tPreviousConclusion tPreviousConclusion) : this()
        {
            PreviousConclusion = tPreviousConclusion;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            radioGroupConclusionNumber.Properties.Items.AddEnum(typeof(ItemChoiceType));

            cmbExaminationResult.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult).ToList());

            cmbExaminationObjectType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType).ToList());
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (PreviousConclusion is null)
            {
                PreviousConclusion = new tPreviousConclusion();
                PreviousConclusion.Number = new tConclusionNumber();
            }

            var number = PreviousConclusion.Number;
            if (number != null)
            {
                switch (number.ItemElementName)
                {
                    case ItemChoiceType.EGRZ:
                        radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                        break;

                    case ItemChoiceType.noEGRZ:
                        radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                        break;
                }
                txtEEPDNumber.EditValue = number.Item;
            }
            if (PreviousConclusion.Date != DateTime.MinValue)
            {
                dateEEPDDate.EditValue = PreviousConclusion.Date;
            }
            cmbExaminationResult.EditValue = cmbExaminationResult.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationResult)
                    ?.FirstOrDefault(f => f.ValueInt == PreviousConclusion?.Result.GetEnumXmlAttributeValueInt());
            cmbExaminationObjectType.EditValue = cmbExaminationObjectType.Properties.Items
                ?.Cast<GeneralizedDictionaryXPO>()
                ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExaminationObjectType)
                ?.FirstOrDefault(f => f.ValueInt == PreviousConclusion?.ExaminationObjectType.GetEnumXmlAttributeValueInt());
            memoName.EditValue = PreviousConclusion.Name;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PreviousConclusion.Number.ItemElementName = (ItemChoiceType)radioGroupConclusionNumber.SelectedIndex;
                PreviousConclusion.Number.Item = ConclusionEdit.GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *"); ;
                if (dateEEPDDate.EditValue is DateTime date)
                {
                    PreviousConclusion.Date = date;
                }
                else
                {
                    dateEEPDDate.Focus();
                    XtraMessageBox.Show("Заполните дату",
                        "Информационное сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                PreviousConclusion.Result = ConclusionEdit.GetEnumByAttributeValue<tExaminationResult>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExaminationResult,
                    message: "Заполните поле результат экспертизы").Value);
                PreviousConclusion.ExaminationObjectType = ConclusionEdit.GetEnumByAttributeValue<tExaminationObjectType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExaminationObjectType,
                    message: "Заполните поле вида объекта экспертизы").Value);
                PreviousConclusion.Name = ConclusionEdit.GetStringValue(memoName.Text, memoName, "Заполните поле наименование объекта экспертизы");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Информационное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void radioGroupConclusionNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is RadioGroup radioGroup)
            {
                switch (radioGroup.SelectedIndex)
                {
                    case (int)ItemChoiceType.EGRZ:
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
                        txtEEPDNumber.Properties.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
                        txtEEPDNumber.Properties.MaskSettings.Set("mask", "[0-9]{2}-[1-2]{1}-[1-2]{1}-[1-3]{1}-[0-9]{6}-[0-9]{4}");
                        break;
                    case (int)ItemChoiceType.noEGRZ:
                        txtEEPDNumber.Properties.ResetMaskSettings();
                        txtEEPDNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
                        break;
                }
            }
        }

        private void dateEEPDDate_ButtonPressed(object sender, ButtonPressedEventArgs e)
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