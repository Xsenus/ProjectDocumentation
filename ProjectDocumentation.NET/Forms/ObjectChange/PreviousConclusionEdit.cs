using Core.NET.AutoModels;
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
    public partial class PreviousConclusionEdit : XtraForm
    {
        public PreviousConclusionEdit()
        {
            InitializeComponent();
            ClassicRequiredFields.HighlightExisting(this);
            ClassicRequiredFields.Unmark(memoName);
        }

        public PreviousConclusionEdit(TPreviousConclusion TPreviousConclusion) : this()
        {
            PreviousConclusion = TPreviousConclusion;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var itemChoiceType = (ItemChoiceType)radioGroupConclusionNumber.SelectedIndex;

                switch (itemChoiceType)
                {
                    case ItemChoiceType.EGRZ:
                        PreviousConclusion.Number.Egrz = ConclusionEdit.GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *");
                        break;

                    case ItemChoiceType.noEGRZ:
                        PreviousConclusion.Number.NoEgrz = ConclusionEdit.GetStringValue(txtEEPDNumber.EditValue, txtEEPDNumber, "Заполните поле номер заключения экспертизы *");
                        break;
                }

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
                PreviousConclusion.Result = ConclusionEdit.GetEnumByAttributeValue<TExaminationResult>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExaminationResult,
                    message: "Заполните поле результат экспертизы")?.Value);
                PreviousConclusion.ExaminationObjectType = ConclusionEdit.GetEnumByAttributeValue<TExaminationObjectType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExaminationObjectType,
                    message: "Заполните поле вида объекта экспертизы")?.Value);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

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
                PreviousConclusion = new TPreviousConclusion
                {
                    Number = new TConclusionNumber()
                };
            }

            var number = PreviousConclusion.Number;
            if (number != null)
            {
                if (!string.IsNullOrWhiteSpace(number.Egrz))
                {
                    radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.EGRZ;
                    txtEEPDNumber.EditValue = number.Egrz;
                }
                else if (!string.IsNullOrWhiteSpace(number.NoEgrz))
                {
                    radioGroupConclusionNumber.SelectedIndex = (int)ItemChoiceType.noEGRZ;
                    txtEEPDNumber.EditValue = number.NoEgrz;
                }
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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Стили именования", Justification = "<Ожидание>")]
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

        private Session Session => SessionController.GetSession();
        public bool IsSave { get; private set; }
        public TPreviousConclusion PreviousConclusion { get; private set; }
    }
}