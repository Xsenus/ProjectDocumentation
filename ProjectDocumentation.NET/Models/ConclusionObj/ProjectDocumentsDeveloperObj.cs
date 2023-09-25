using Core.NET.AutoModels;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class ProjectDocumentsDeveloperObjController
    {
        public static IEnumerable<ProjectDocumentsDeveloperObj> GetProjectDocumentsDeveloperObj(ICollection<object> collection)
        {
            foreach (var obj in collection)
            {
                var result = new ProjectDocumentsDeveloperObj(obj);
                yield return result;
            }
        }
    }

    public class ProjectDocumentsDeveloperObj
    {
        public object ProjectDocumentsDeveloper { get; private set; }

        public ProjectDocumentsDeveloperObj(object projectDocumentsDeveloper)
        {
            ProjectDocumentsDeveloper = projectDocumentsDeveloper;

            if (ProjectDocumentsDeveloper is tDeclarant declarant)
            {
                if (declarant.Item is tForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (declarant.Item is tIP ip)
                {
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                }
                else if (declarant.Item is tOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
                else if (declarant.Item is tPerson tPerson)
                {
                    Name = $"ФЛ {tPerson.FamilyName} {tPerson.FirstName} {tPerson.SecondName}";
                }
            }
            else if (ProjectDocumentsDeveloper is tTechnicalCustomer tTechnicalCustomer)
            {
                if (tTechnicalCustomer.Item is tForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (tTechnicalCustomer.Item is tOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
            }
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
