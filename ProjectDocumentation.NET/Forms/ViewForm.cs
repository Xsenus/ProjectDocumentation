using Core.NET.AutoModels;
using DevExpress.XtraEditors;
using System;

namespace ProjectDocumentation.NET.Forms
{
    public partial class ViewForm : XtraForm
    {
        public object _obj;
        public ViewForm(object obj)
        {
            InitializeComponent();
            _obj = obj;
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            var result = default(string);
            if (_obj is Conclusion conclusion)
            {
                result += $"{conclusion.ExpertOrganization?.OrgFullName}{Environment.NewLine}";
                result += $"{conclusion.Approver?.Position}{Environment.NewLine}";
                result += $"{conclusion.Approver?.FamilyName} {conclusion.Approver?.FirstName} {conclusion.Approver?.SecondName}{Environment.NewLine}";
            }

            memoValue.EditValue = result;
        }
    }
}