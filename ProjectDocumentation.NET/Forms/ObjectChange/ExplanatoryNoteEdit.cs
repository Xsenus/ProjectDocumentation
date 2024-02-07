using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using DevExpress.Xpo;
using DevExpress.XtraEditors;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    public partial class ExplanatoryNoteEdit : XtraForm
    {
        private Session Session => SessionController.GetSession();
        public string FileName { get; private set; }
        public bool IsSave { get; private set; }
        public ExplanatoryNote ExplanatoryNote { get; private set; }

        public ExplanatoryNoteEdit()
        {
            InitializeComponent();
        }

        public ExplanatoryNoteEdit(ExplanatoryNote explanatoryNote) : this()
        {
            ExplanatoryNote = explanatoryNote;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }
    }
}