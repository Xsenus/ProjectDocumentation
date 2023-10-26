using Core.NET.AutoModels;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExpertProjectDocumentsEdit : XtraForm
    {
        private Session _session => SessionController.GetSession();

        public bool IsSave { get; private set; }
        public tExpertProjectDocuments ExpertProjectDocuments { get; private set; }

        public ExpertProjectDocumentsEdit()
        {
            InitializeComponent();

            richEditControlProjectDocumentsReview.ActiveViewType = RichEditViewType.Simple;
            richEditControlProjectDocumentsChanges.ActiveViewType = RichEditViewType.Simple;
        }

        public ExpertProjectDocumentsEdit(tExpertProjectDocuments tExpertProjectDocuments) : this()
        {
            ExpertProjectDocuments = tExpertProjectDocuments;
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(_session).ToListAsync();

            cmbExpertType.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType).ToList());
        }

        private async void ObjPartEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (ExpertProjectDocuments is null)
            {
                ExpertProjectDocuments = new tExpertProjectDocuments();
            }

            cmbExpertType.EditValue = cmbExpertType.Properties.Items
                    ?.Cast<GeneralizedDictionaryXPO>()
                    ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.ExpertType)
                    ?.FirstOrDefault(f => f.Value == ExpertProjectDocuments?.ExpertType.GetEnumXmlAttributeValue().ToString());

            richEditControlProjectDocumentsReview.Text = ExpertProjectDocuments.ProjectDocumentsReview;
            richEditControlProjectDocumentsChanges.Text = ExpertProjectDocuments.ProjectDocumentsChanges;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ExpertProjectDocuments.ExpertType = ConclusionEdit.GetEnumByAttributeValue<tExpertType>(ConclusionEdit.GetValueGeneralizedDictionary(
                    cmbExpertType,
                    message: "Заполните поле направление деятельности в области экспертизы проектной документации.").Value);

                ExpertProjectDocuments.ProjectDocumentsReview = ConclusionEdit.GetStringValue(richEditControlProjectDocumentsReview.Text, richEditControlProjectDocumentsReview, "Заполните поле описание основных решений (мероприятий), принятых в проектной документации.", isNullAble: true);
                ExpertProjectDocuments.ProjectDocumentsChanges = ConclusionEdit.GetStringValue(richEditControlProjectDocumentsChanges.Text, richEditControlProjectDocumentsChanges, "Заполните поле сведения об оперативных изменениях.", isNullAble: true);
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

        private void buttonEdit_ButtonPressed(object sender, ButtonPressedEventArgs e)
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