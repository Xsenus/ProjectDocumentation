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

        public static IEnumerable<PersonObj> GetPersonObjByDeclarator(ICollection<TDeclarant> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByDeclarator();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByDesigner(ICollection<TDesigner> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByDesigner();
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

        public static IEnumerable<PersonObj> GetPersonObjByTechnicalCustomer(ICollection<Core.NET.AutoModels.ConclusionObj.TTechnicalCustomer> collection)
        {
            foreach (var obj in collection)
            {
                var result = new PersonObj(obj);
                result.SetObjByTechnicalCustomer();
                yield return result;
            }
        }

        public static IEnumerable<PersonObj> GetPersonObjByTechnicalCustomer(ICollection<Core.NET.AutoModels.ExplanatoryNoteObj.TTechnicalCustomer> collection)
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

        public PersonObj(object obj)
        {
            Obj = obj;
        }

        public void SetObj()
        {
            if (Obj is TDeclarant declarant)
            {
                BaseObj = declarant;
                var entity = (object)declarant.Organization
                             ?? declarant.ForeignOrganization
                             ?? declarant.Ip
                             ?? (object)declarant.Person;

                if (entity is TForeignOrganization fo)
                    Name = fo.OrgFullName;
                else if (entity is Tip ip)
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                else if (entity is TOrganization org)
                    Name = org.OrgFullName;
                else if (entity is TPerson person)
                    Name = $"ФЛ {person.FamilyName} {person.FirstName} {person.SecondName}";

                if (entity != null)
                    Obj = entity;
            }
            else if (Obj is Core.NET.AutoModels.ConclusionObj.TTechnicalCustomer tc)
            {
                BaseObj = tc;
                var entity = (object)tc.Organization ?? tc.ForeignOrganization;

                if (entity is TForeignOrganization fo)
                    Name = fo.OrgFullName;
                else if (entity is TOrganization org)
                    Name = org.OrgFullName;

                if (entity != null)
                    Obj = entity;
            }
            else if (Obj is Core.NET.AutoModels.ExplanatoryNoteObj.TTechnicalCustomer tce)
            {
                BaseObj = tce;
                Obj = tce.Item;

                if (Obj is TForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (Obj is Tip ip)
                {
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                }
                else if (Obj is TOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
            }
        }

        public void SetObjByDeclarator()
        {
            if (Obj is TDeclarant declarant)
            {
                BaseObj = declarant;
                Obj = (object)declarant.Organization
                      ?? declarant.ForeignOrganization
                      ?? declarant.Ip
                      ?? (object)declarant.Person;
            }

            if (Obj is TForeignOrganization fo)
            {
                Name = fo.OrgFullName;
            }
            else if (Obj is Tip ip)
            {
                Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
            }
            else if (Obj is TOrganization org)
            {
                Name = org.OrgFullName;
            }
            else if (Obj is TPerson person)
            {
                Name = $"ФЛ {person.FamilyName} {person.FirstName} {person.SecondName}";
            }
        }

        public void SetObjByDesigner()
        {
            if (Obj is TDesigner designer)
            {
                BaseObj = designer;
                Obj = (object)designer.Organization ?? (object)designer.ForeignOrganization ?? (object)designer.Ip;

                if (Obj is TForeignOrganization fo)
                    Name = fo.OrgFullName;
                else if (Obj is Tip ip)
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                else if (Obj is TOrganization org)
                    Name = org.OrgFullName;
            }
        }

        public void SetObjByTechnicalCustomer()
        {
            if (Obj is Core.NET.AutoModels.ConclusionObj.TTechnicalCustomer tc)
            {
                BaseObj = tc;
                Obj = (object)tc.Organization ?? tc.ForeignOrganization;

                if (Obj is TForeignOrganization fo)
                    Name = fo.OrgFullName;
                else if (Obj is TOrganization org)
                    Name = org.OrgFullName;
            }
            else if (Obj is Core.NET.AutoModels.ExplanatoryNoteObj.TTechnicalCustomer tce)
            {
                BaseObj = tce;
                Obj = tce.Item;

                if (Obj is TForeignOrganization tForeignOrganization)
                {
                    Name = tForeignOrganization.OrgFullName;
                }
                else if (Obj is Tip ip)
                {
                    Name = $"ИП {ip.FamilyName} {ip.FirstName} {ip.SecondName}";
                }
                else if (Obj is TOrganization tOrganization)
                {
                    Name = tOrganization.OrgFullName;
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public object BaseObj { get; private set; }
        public string Name { get; set; }
        public object Obj { get; private set; }
    }
}
