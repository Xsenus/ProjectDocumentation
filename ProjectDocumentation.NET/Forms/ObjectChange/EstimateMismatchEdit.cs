using Core.NET.AutoModels.ConclusionObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class EstimateMismatchEdit : XtraForm
    {
        private static readonly string[] KindNames =
        {
            "Общее замечание",
            "Сводный сметный расчёт",
            "Объектный или локальный сметный расчёт",
            "Соответствие сметы проектным решениям и объёмам работ",
            "Пересчёт сметной стоимости в текущий уровень цен"
        };

        public EstimateMismatchEdit()
        {
            InitializeComponent();
            cmbEstimateMismatches.Properties.Items.Clear();
            cmbEstimateMismatches.Properties.Items.AddRange(KindNames);
            cmbEstimateMismatches.Properties.Buttons.RemoveAt(2);
            cmbEstimateMismatches.Properties.Buttons.RemoveAt(1);
            cmbExpertType.Properties.Buttons.RemoveAt(2);
            cmbExpertType.Properties.Buttons.RemoveAt(1);
            layoutControlItemExpertType.Text = "Направление деятельности *";
            cmbEstimateMismatches.SelectedIndexChanged += (_, __) => UpdateExpertTypeVisibility();
            btnSave.AllowFocus = true;
            btnCancel.AllowFocus = true;
            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        internal EstimateMismatchEdit(Row mismatch) : this()
        {
            Mismatch = mismatch;
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbEstimateMismatches.SelectedIndex < 0)
                {
                    cmbEstimateMismatches.Focus();
                    throw new InvalidOperationException("Выберите вид несоответствия.");
                }

                var kind = (Kind)cmbEstimateMismatches.SelectedIndex;
                var expertType = kind == Kind.ProjectDocuments
                    ? ConclusionEdit.GetEnumByAttributeValue<TExpertType>(
                        ConclusionEdit.GetValueGeneralizedDictionary(cmbExpertType, "Выберите направление деятельности.").Value,
                        isNullAble: false)
                    : (TExpertType?)null;
                Mismatch = new Row
                {
                    Category = kind,
                    ExpertType = expertType,
                    Summary = ConclusionEdit.GetStringValue(txtSummary.Text, txtSummary, "Заполните вывод о несоответствии.", isNullAble: false),
                    Part = ConclusionEdit.GetStringValue(txtPart.Text, txtPart, "Укажите ссылку на материалы.", isNullAble: false),
                    Link = ConclusionEdit.GetStringValue(txtLink.Text, txtLink, "Укажите ссылку на конкретное требование.", isNullAble: false)
                };
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(this, error.Message, "Несоответствие", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IsSave = true;
            DialogResult = DialogResult.OK;
        }

        private void ExpretEdit_Load(object sender, EventArgs e)
        {
            cmbExpertType.Properties.Items.AddRange(new XPQuery<GeneralizedDictionaryXPO>(SessionController.GetSession())
                .Where(item => item.GeneralizedDictionary == GeneralizedDictionary.ExpertType)
                .ToList());

            if (Mismatch != null)
            {
                cmbEstimateMismatches.SelectedIndex = (int)Mismatch.Category;
                cmbExpertType.EditValue = cmbExpertType.Properties.Items.OfType<GeneralizedDictionaryXPO>()
                    .FirstOrDefault(item => item.Value == Mismatch.ExpertTypeCode);
                txtSummary.Text = Mismatch.Summary;
                txtPart.Text = Mismatch.Part;
                txtLink.Text = Mismatch.Link;
            }

            UpdateExpertTypeVisibility();
        }

        private void UpdateExpertTypeVisibility()
        {
            layoutControlItemExpertType.Visibility = cmbEstimateMismatches.SelectedIndex == (int)Kind.ProjectDocuments
                ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        internal Row Mismatch { get; private set; }
        public bool IsSave { get; private set; }

        internal enum Kind
        {
            Common,
            FullCalculation,
            LocalCalculation,
            ProjectDocuments,
            Basic
        }

        internal sealed class Row
        {
            [Browsable(false)]
            public Kind Category { get; set; }
            [Browsable(false)]
            public TExpertType? ExpertType { get; set; }
            public string CategoryName => KindNames[(int)Category];
            public string Summary { get; set; }
            public string Part { get; set; }
            public string Link { get; set; }
            public string ExpertTypeCode => ExpertType?.GetEnumXmlAttributeValue()?.ToString();
        }
    }
}
