using DevExpress.XtraEditors;
using System;

namespace ProjectDocumentation.Forms.ObjectChange
{
    public partial class ConclusionEdit : XtraForm
    {
        public ConclusionEdit()
        {
            InitializeComponent();

            layoutControlGroup7.Text =
                $"Сведения об объекте капитального строительства,{Environment.NewLine}" +
                $"применительно к которому подготовлена {Environment.NewLine}" +
                $"проектная документация";

            layoutControlGroup8.Text =
                $"Сведения об источнике (источниках) и размере{Environment.NewLine}" +
                $"финансирования строительства, реконструкции,{Environment.NewLine}" +
                $"капитального ремонта, сноса объекта{Environment.NewLine}" +
                $"капитального строительства";

            layoutControlGroup9.Text =
                $"Сведения о природных и техногенных{Environment.NewLine}" +
                $"условиях территории";

            layoutControlGroup10.Text =
                $"Сведения об индивидуальных предпринимателях и{Environment.NewLine}" +
                $"(или) юридических лицах, подготовивших{Environment.NewLine}" +
                $"проектную документацию";

            layoutControlGroup11.Text =
                $"Сведения об использовании при подготовке проектной{Environment.NewLine}" +
                $"документации экономически эффективной проектной{Environment.NewLine}" +
                $"документации повторного использования";

            layoutControlGroup12.Text =
                $"Кадастровый номер земельного участка{Environment.NewLine}" +
                $"(земельных участков), в пределах которого{Environment.NewLine}" +
                $"(которых) расположен или планируется расположение {Environment.NewLine}" +
                $"объекта капитального строительства,{Environment.NewLine}" +
                $"не являющегося линейным объектом";

            layoutControlGroup13.Text =
                $"Сведения о застройщике (техническом заказчике),{Environment.NewLine}" +
                $"обеспечившем подготовку проектной документации";
        }
    }
}