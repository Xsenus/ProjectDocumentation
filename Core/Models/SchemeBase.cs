namespace Core.Models
{
    public class SchemeBase
    {
        public SchemeBase(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" не может быть пустым или содержать только пробел.", nameof(name));
            }

            Name = name;
        }

        public string Name { get; private set; }
        public Scheme? Scheme { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Scheme
    {        
        public Scheme()
        {
            SchemeObjS = new List<SchemeObj>();
        }

        public List<SchemeObj> SchemeObjS { get; private set; }
    }

    public class SchemeObj
    {
        public SchemeObj(string name) : this(name, name) { }
        public SchemeObj(string name, string attributeName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" не может быть пустым или содержать только пробел.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentException($"\"{nameof(attributeName)}\" не может быть пустым или содержать только пробел.", nameof(attributeName));
            }

            Guid = Guid.NewGuid();
            Name = name;
            AttributeName = attributeName;
        }

        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string AttributeName { get; private set; }
        public SchemeObj? Parent { get; set; }
        public List<SchemeObj>? ChildrenS { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}