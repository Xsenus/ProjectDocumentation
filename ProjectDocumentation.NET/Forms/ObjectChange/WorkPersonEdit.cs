using Core.NET.AutoModels;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
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
    public partial class WorkPersonEdit : XtraForm
    {
        public WorkPersonEdit()
        {
            InitializeComponent();
            ClassicRequiredFields.HighlightExisting(this);
            ClassicRequiredFields.Unmark(txtFamilyName, txtFirstName, txtPosition);
        }

        public WorkPersonEdit(tWorkPerson person) : this()
        {
            WorkPerson = person;
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
                WorkPerson.FamilyName = ConclusionEdit.GetStringValue(txtFamilyName.EditValue, txtFamilyName, "Заполните фамилию подписанта");
                WorkPerson.FirstName = ConclusionEdit.GetStringValue(txtFirstName.EditValue, txtFirstName, "Заполните имя подписанта");
                WorkPerson.SecondName = ConclusionEdit.GetStringValue(txtSecondName.EditValue, txtSecondName, "Заполните отчество подписанта");
                WorkPerson.Position = ConclusionEdit.GetStringValue(txtPosition.EditValue, txtPosition, "Заполните должность подписанта");

                if (cmbEngineeringRegistry.EditValue is GeneralizedDictionaryXPO valueEngineeringRegistry)
                {
                    WorkPerson.engineeringRegistrySpecified = true;
                    WorkPerson.engineeringRegistry = ConclusionEdit.GetEnumByAttributeValue<TYesNo>(valueEngineeringRegistry.Value);
                }
                else
                {
                    WorkPerson.engineeringRegistrySpecified = false;
                }

                if (cmbConstructionRegistry.EditValue is GeneralizedDictionaryXPO valueConstructionRegistry)
                {
                    WorkPerson.constructionRegistrySpecified = true;
                    WorkPerson.constructionRegistry = ConclusionEdit.GetEnumByAttributeValue<TYesNo>(valueConstructionRegistry.Value);
                }
                else
                {
                    WorkPerson.constructionRegistrySpecified = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message,
                    "Информационное сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            IsSave = true;
            Close();
        }

        private void ClearEditValue(object sender, ButtonPressedEventArgs e)
        {
            if (sender is CheckedComboBoxEdit checkedComboBoxEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    foreach (CheckedListBoxItem item in checkedComboBoxEdit.Properties.Items.Cast<CheckedListBoxItem>())
                    {
                        item.CheckState = CheckState.Unchecked;
                    }
                }
            }

            if (sender is ButtonEdit buttonEdit)
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    buttonEdit.EditValue = null;
                }
            }
        }

        private void cmbEngineeringRegistry_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {

        }

        private async void ConclusionBaseSettingEdit_Load(object sender, EventArgs e)
        {
            await FillFormControllersAsync();

            if (WorkPerson is null)
            {
                WorkPerson = new tWorkPerson();
            }

            txtFamilyName.EditValue = WorkPerson.FamilyName;
            txtFirstName.EditValue = WorkPerson.FirstName;
            txtSecondName.EditValue = WorkPerson.SecondName;
            txtPosition.EditValue = WorkPerson.Position;

            if (WorkPerson?.engineeringRegistrySpecified is true)
            {
                cmbEngineeringRegistry.EditValue = cmbEngineeringRegistry.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
                  ?.FirstOrDefault(f => f.Value == WorkPerson?.engineeringRegistry.GetEnumXmlAttributeValue().ToString());
            }

            if (WorkPerson?.constructionRegistrySpecified is true)
            {
                cmbConstructionRegistry.EditValue = cmbConstructionRegistry.Properties.Items
                  ?.Cast<GeneralizedDictionaryXPO>()
                  ?.Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer)
                  ?.FirstOrDefault(f => f.Value == WorkPerson?.constructionRegistry.GetEnumXmlAttributeValue().ToString());
            }
        }

        private async Task FillFormControllersAsync()
        {
            var collection = await new XPQuery<GeneralizedDictionaryXPO>(Session).ToListAsync();

            cmbEngineeringRegistry.Properties.Items.AddRange(collection
                            .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());

            cmbConstructionRegistry.Properties.Items.AddRange(collection
                .Where(w => w.GeneralizedDictionary == GeneralizedDictionary.LogicalAnswer).ToArray());
        }

        private Session Session => SessionController.GetSession();
        public bool IsSave { get; private set; }
        public tWorkPerson WorkPerson { get; private set; }
    }
}
