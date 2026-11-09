using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using Core.NET.Controllers;
using Core.NET.Extensions;
using Core.NET.Models.XPO;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDocumentation.NET.Forms
{
    public partial class ViewForm : XtraForm
    {
        private string _result;
        private Session Session => SessionController.GetSession();

        public object _obj;
        public ViewForm(object obj)
        {
            InitializeComponent();
            _obj = obj;
        }

        private async void ViewForm_Load(object sender, EventArgs e)
        {
            if (_obj is Conclusion conclusion)
            {
                AddValueToString($"{conclusion.ExpertOrganization?.OrgFullName}");
                AddValueToString($"\"УТВЕРЖДАЮ\"");
                AddValueToString($"{conclusion.Approver?.Position}");
                AddValueToString($"{conclusion.Approver?.FamilyName} {conclusion.Approver?.FirstName} {conclusion.Approver?.SecondName}");
                AddValueToString();

                // Раздел может отсутствовать в импортированном файле: пустой объект вместо NullReferenceException.
                var examinationObject = conclusion.ExaminationObject ?? new TExaminationObject();

                var examinationResult = await GetValueXPOAsync(GeneralizedDictionary.ExaminationResult, examinationObject.ExaminationResult);
                AddValueToString($"{examinationResult?.Replace("ый", "ое")} заключение повторной негосударственной экспертизы");
                AddValueToString();

                AddValueToString($"Наименование объекта экспертизы:");
                AddValueToString($"{examinationObject.Name}");
                AddValueToString();

                AddValueToString($"Вид работ:");
                AddValueToString($"{await GetValueXPOAsync(GeneralizedDictionary.ConstructionType, examinationObject.ConstructionType)}");
                AddValueToString();

                AddValueToString($"Объект экспертизы:");
                AddValueToString($"{await GetValueXPOAsync(GeneralizedDictionary.ExaminationStage, examinationObject.ExaminationStage)}");
                AddValueToString();

                AddValueToString($"Предмет экспертизы:");
                foreach (var item in examinationObject.ExaminationType)
                {
                    AddValueToString($"{await GetValueXPOAsync(GeneralizedDictionary.ExaminationType, item)}");
                }
                AddValueToString();

                AddValueToString($"I. Общие положения и сведения о заключении экспертизы");
                AddValueToString();

                AddValueToString($"1.1. Сведения об организации по проведению повторной экспертизы");
                AddValueToString($"{conclusion.ExpertOrganization?.GetCounterPartyDescription()}");
                AddValueToString();

                AddValueToString($"1.2. Сведения о заявителе");
                //AddValueToString($"{conclusion.Declarant?.Item?.GetCounterPartyDescription()}");
                AddValueToString();

                AddValueToString($"1.3. Основания для проведения повторной экспертизы");
                var documentCount = 0;

                if (conclusion.Documents != null)
                {
                    foreach (var document in conclusion.Documents)
                    {
                        if ((document.DocType.GetEnumXmlAttributeValue()?.ToString() ?? string.Empty).Contains("01."))
                        {
                            documentCount++;
                            var documentValue = $"{documentCount}. {document.DocName} от {document.DocDate.ToShortDateString()} № {document.DocNumber}";
                            //var documentOrganizationName = document.Items?.FirstOrDefault()?.ToString();
                            //if (!string.IsNullOrWhiteSpace(documentOrganizationName))
                            //{
                            //    documentValue += $", {documentOrganizationName}";
                            //}
                            AddValueToString(documentValue);
                        }
                    }
                }
                
                AddValueToString();

                AddValueToString($"1.4. Сведения о положительном заключении государственной экологической экспертизы");

                if (conclusion?.Documents != null)
                {
                    foreach (var document in conclusion.Documents)
                    {
                        if ((document.DocType.GetEnumXmlAttributeValue()?.ToString() ?? string.Empty).Contains("03."))
                        {
                            //documentCount++;
                            //var documentValue = $"{documentCount}. {document.DocName} от {document.DocDate.ToShortDateString()} № {document.DocNumber}";
                            //var documentOrganizationName = document.Items?.FirstOrDefault()?.ToString();
                            //if (!string.IsNullOrWhiteSpace(documentOrganizationName))
                            //{
                            //    documentValue += $", {documentOrganizationName}";
                            //}
                            //AddValueToString(documentValue);
                        }
                    }
                }
                AddValueToString();

                AddValueToString($"1.5. Сведения о составе документов, представленных для проведения повторной экспертизы");
                documentCount = 0;
                if (conclusion?.Documents != null)
                {
                    foreach (var document in conclusion.Documents)
                    {
                        if ((document.DocType.GetEnumXmlAttributeValue()?.ToString() ?? string.Empty).Contains("03."))
                        {
                            documentCount++;
                            var documentValue = $"{documentCount}. {document.DocName} от {document.DocDate.ToShortDateString()} № {document.DocNumber}";
                            //var documentOrganizationName = document.Items?.FirstOrDefault()?.ToString();
                            //if (!string.IsNullOrWhiteSpace(documentOrganizationName))
                            //{
                            //    documentValue += $", {documentOrganizationName}";
                            //}
                            AddValueToString(documentValue);
                        }
                    }
                }
                AddValueToString();

                AddValueToString($"1.6. Сведения о ранее выданных заключениях экспертизы проектной документации и (или) результатов инженерных изысканий в отношении объекта капитального строительства, проектная документация и (или) результаты инженерных изысканий по которому представлены для проведения повторной экспертизы");
                if (conclusion?.Documents != null)
                {
                    foreach (var document in conclusion.Documents)
                    {
                        if ((document.DocType.GetEnumXmlAttributeValue()?.ToString() ?? string.Empty).Contains("03."))
                        {
                            //documentCount++;
                            //var documentValue = $"{documentCount}. {document.DocName} от {document.DocDate.ToShortDateString()} № {document.DocNumber}";
                            //var documentOrganizationName = document.Items?.FirstOrDefault()?.ToString();
                            //if (!string.IsNullOrWhiteSpace(documentOrganizationName))
                            //{
                            //    documentValue += $", {documentOrganizationName}";
                            //}
                            //AddValueToString(documentValue);
                        }
                    }
                }
                AddValueToString();

                AddValueToString($"II. Сведения, содержащиеся в документах, представленных для проведения повторной экспертизы проектной документации");
                AddValueToString();

                AddValueToString($"2.1. Сведения об объекте капитального строительства, применительно к которому подготовлена проектная документация");
                AddValueToString();

                AddValueToString($"2.1.1. Сведения о наименовании объекта капитального строительства, его почтовый (строительный) адрес или местоположение");
                AddValueToString();

                AddValueToString($"Наименование объекта капитального строительства: {conclusion.ExaminationObject?.Name}");
                AddValueToString($"Почтовый (строительный) адрес (местоположение) объекта капитального строительства: {conclusion.Object?.Address}");
                AddValueToString();

                AddValueToString($"2.1.2. Сведения о функциональном назначении объекта капитального строительства");
                AddValueToString();

                //AddValueToString($"Функциональное назначение: {conclusion.Object?.Item ?? "Сведения отсутствуют."}");
                //AddValueToString();

                AddValueToString($"2.1.3. Сведения о технико-экономических показателях объекта капитального строительства");
                AddValueToString();

                //AddValueToString($"Наименование технико-экономического показателя\tЕдиница измерения\tЗначение");
                //AddValueToString($"{GetProcessedList(conclusion.Object?.Tei)}");
                //AddValueToString();

            }

            memoValue.EditValue = _result;
        }


        private string GetProcessedList(IEnumerable<tTEI> collection)
        {
            if (collection != null)
            {
                var maxLenght = collection.Max(m => m.Name?.Length);
                var result = new List<string>();
                foreach (var item in collection)
                {
                    var name = item.Name ?? string.Empty;
                    while (name.Length < maxLenght)
                    {
                        name += " ";
                    }

                    result.Add($"{name}\t{item.Measure}\t{item.Value}");
                }

                return string.Join(Environment.NewLine, result.ToArray());
            }

            return default;
        }

        private void AddValueToString()
        {
            _result += Environment.NewLine;
        }

        private void AddValueToString(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _result += $"{value}{Environment.NewLine}";
            }
        }

        private async Task<string> GetValueXPOAsync<T>(GeneralizedDictionary @enumObj, T value) where T : Enum
        {
            try
            {
                var obj = await new XPQuery<GeneralizedDictionaryXPO>(Session)
                                ?.Where(w => w.GeneralizedDictionary == @enumObj)
                                ?.FirstOrDefaultAsync(f => f.Value == value.GetEnumXmlAttributeValue().ToString());

                return obj?.Name;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}