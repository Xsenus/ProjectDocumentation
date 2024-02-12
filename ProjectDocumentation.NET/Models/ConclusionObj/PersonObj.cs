using Core.NET.AutoModels;
using Core.NET.AutoModels.ConclusionObj;
using Core.NET.AutoModels.ExplanatoryNoteObj;
using System.Collections.Generic;

namespace ProjectDocumentation.NET.Models.ConclusionObj
{
    public static class PersonObjController
    {
        public static IEnumerable<PersonObj> GetPersonObj(ICollection<object> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObj();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByDesigner(ICollection<tDesigner> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByDesigner();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByDeclarator(ICollection<tDeclarant> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByDeclarator();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByDeveloper(ICollection<tDeveloper> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByDeclarator();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByTechnicalCustomer(ICollection<ITechnicalCustomer> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByTechnicalCustomer();
                yield return result;
            }
        }
    }

    public class PersonObj
    {
        public object BaseObj { get; private set; }
        public object Obj { get; private set; }

        public PersonObj(object obj)
        {
            Obj = obj;
        }

        public void SetObjByDesigner()
        {
            if (Obj is tDesigner designer)
            {
                BaseObj = designer;
                Obj = designer.Item;

                if (Obj is tForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (Obj is tIP ip)
                {
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                }
                else if (Obj is tOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
            }
        }

        public void SetObjByDeclarator()
        {
            if (Obj is tDeclarant declarant)
            {
                BaseObj = declarant;
                Obj = declarant.Item;                
            }
            else if (Obj is tDeveloper developer)
            {
                BaseObj = developer;
                Obj = developer.Item;                
            }

            if (Obj is tForeignOrganization tForeignOrganization)
            {
                Name = tForeignOrganization.OrgFullName;
            }
            else if (Obj is tIP ip)
            {
                Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
            }
            else if (Obj is tOrganization tOrganization)
            {
                Name = tOrganization.OrgFullName;
            }
            else if (Obj is tPerson person)
            {
                Name = $"ФЛ {person.FamilyName} {person.FirstName} {person.SecondName}";
            }
        }

        public void SetObjByTechnicalCustomer()
        {
            if (Obj is ITechnicalCustomer tTechnicalCustomer)
            {
                BaseObj = tTechnicalCustomer;
                Obj = tTechnicalCustomer.Item;

                if (Obj is tForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (Obj is tIP ip)
                {
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                }
                else if (Obj is tOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
                else if (Obj is tPerson person)
                {
                    Name = $"ФЛ {person.FamilyName} {person.FirstName} {person.SecondName}";
                }
            }
        }

        public void SetObj()
        {
            if (Obj is tDeclarant declarant)
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
            else if (Obj is ITechnicalCustomer tTechnicalCustomer)
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
