using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Forms
{
    internal sealed class NativeSchemaProfile
    {
        public NativeSchemaParticle Root { get; set; }
        public Dictionary<string, NativeSchemaType> Types { get; set; }

        public NativeSchemaType TypeOf(string name)
            => name != null && Types.TryGetValue(name, out var type) ? type : null;

        public NativeSchemaNode RootNode(XElement element) => new NativeSchemaNode(element, Root, TypeOf(Root.Type));

        public NativeSchemaNode ChildNode(NativeSchemaNode parent, XElement element)
        {
            var field = parent.Children.FirstOrDefault(candidate => XName.Get(candidate.Field.Name) == element.Name)?.Field;
            return new NativeSchemaNode(element, field, TypeOf(field?.Type));
        }
    }

    internal sealed class NativeSchemaParticle
    {
        public string Kind { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Documentation { get; set; }
        public string Default { get; set; }
        public string Fixed { get; set; }
        public bool Nillable { get; set; }
        public int Min { get; set; }
        public int? Max { get; set; }
        public List<NativeSchemaParticle> Children { get; set; }

        private bool AllowsOmission => Min == 0 || Max == 0 ||
            (Kind == "choice" ? Members.Any(member => member.AllowsOmission) :
                Kind != "element" && Members.All(member => member.AllowsOmission));

        private IEnumerable<NativeSchemaParticle> Members => Children ?? Enumerable.Empty<NativeSchemaParticle>();

        private IEnumerable<string> ElementNames => Kind == "element" ? new[] { Name } : Members.SelectMany(member => member.ElementNames);

        internal void CollectRequirements(IReadOnlyDictionary<string, int> counts, NativeSchemaRequirements result)
        {
            if (Max == 0) return;
            if (Kind == "element")
            {
                if (Min > 0) result.Elements.Add(new NativeSchemaElementRequirement(this, Count(counts, Name)));
                return;
            }

            var selected = Members.Where(member => member.ElementNames.Any(name => Count(counts, name) > 0)).ToList();
            if (Min == 0 && selected.Count == 0) return;
            if (Kind == "choice")
            {
                if (Min > 0 && !Members.Any(member => member.AllowsOmission))
                {
                    var occurrences = Max == 1 ? Math.Min(selected.Count, 1) :
                        selected.Sum(member => member.Kind == "element" ? Count(counts, member.Name) : 1);
                    result.Choices.Add(new NativeSchemaChoiceRequirement(Min, occurrences,
                        Members.Select(member => member.ElementNames.Distinct().ToArray()).ToArray(),
                        selected.SelectMany(member => member.ElementNames).Where(name => Count(counts, name) > 0).Distinct().ToArray(),
                        Max == 1 && selected.Count > 1));
                }

                // A selected sequence still requires its own members. Other alternatives have no independent minimum.
                if (selected.Count == 1 && selected[0].Kind != "element")
                    selected[0].CollectRequirements(counts, result);
                return;
            }

            foreach (var member in Members)
                member.CollectRequirements(counts, result);
        }

        private static int Count(IReadOnlyDictionary<string, int> counts, string name)
            => counts.TryGetValue(name, out var count) ? count : 0;

        public IEnumerable<NativeSchemaChild> ChildFields(long max = 1, bool required = true, NativeSchemaParticle repeatedGroup = null,
            bool insideChoice = false)
        {
            var capacity = Max.HasValue ? Math.Min((long)int.MaxValue, max * Max.Value) : int.MaxValue;
            if (capacity == 0)
            {
                yield break;
            }

            var requiredHere = required && Min > 0;
            if (Kind == "element")
            {
                yield return new NativeSchemaChild(this, capacity, requiredHere, repeatedGroup, insideChoice);
                yield break;
            }

            foreach (var child in Children ?? Enumerable.Empty<NativeSchemaParticle>())
            {
                var group = repeatedGroup ?? (Max != 1 ? this : null);
                foreach (var field in child.ChildFields(capacity, requiredHere && Kind != "choice", group, insideChoice || Kind == "choice"))
                {
                    yield return field;
                }
            }
        }
    }

    internal sealed class NativeSchemaType
    {
        public string Kind { get; set; }
        public string Base { get; set; }
        public bool? AllowsEmpty { get; set; }
        public string TextType { get; set; }
        public string Documentation { get; set; }
        public NativeSchemaParticle Particle { get; set; }
        public List<NativeSchemaAttribute> Attributes { get; set; }
        public List<NativeSchemaOption> Enumeration { get; set; }
    }

    internal sealed class NativeSchemaRequirements
    {
        internal List<NativeSchemaElementRequirement> Elements { get; } = new List<NativeSchemaElementRequirement>();
        internal List<NativeSchemaChoiceRequirement> Choices { get; } = new List<NativeSchemaChoiceRequirement>();
    }

    internal sealed class NativeSchemaElementRequirement
    {
        internal NativeSchemaElementRequirement(NativeSchemaParticle field, int present)
        {
            Field = field;
            Present = present;
        }

        internal NativeSchemaParticle Field { get; }
        internal int Present { get; }
        internal bool IsMissing => Present < Field.Min;
    }

    internal sealed class NativeSchemaChoiceRequirement
    {
        internal NativeSchemaChoiceRequirement(int minimum, int present, IReadOnlyList<string[]> alternatives, IReadOnlyList<string> selected,
            bool hasConflict)
        {
            Minimum = minimum;
            Present = present;
            Alternatives = alternatives;
            Selected = selected;
            HasConflict = hasConflict;
        }

        internal int Minimum { get; }
        internal int Present { get; }
        internal IReadOnlyList<string[]> Alternatives { get; }
        internal IReadOnlyList<string> Selected { get; }
        internal bool IsMissing => Present < Minimum;
        internal bool HasConflict { get; }
    }

    internal sealed class NativeSchemaAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Documentation { get; set; }
        public string Default { get; set; }
        public string Fixed { get; set; }
        public bool Required { get; set; }
    }

    internal sealed class NativeSchemaOption
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public override string ToString() => string.IsNullOrWhiteSpace(Label) || Label == Value ? Value : $"{Value} — {Label}";
    }

    internal sealed class NativeSchemaChild
    {
        public NativeSchemaChild(NativeSchemaParticle field, long capacity, bool required, NativeSchemaParticle repeatedGroup, bool insideChoice)
        {
            Field = field;
            Capacity = capacity;
            Required = required;
            RepeatedGroup = repeatedGroup;
            InsideChoice = insideChoice;
        }

        public NativeSchemaParticle Field { get; }
        public long Capacity { get; }
        public bool Required { get; }
        public NativeSchemaParticle RepeatedGroup { get; }
        public bool InsideChoice { get; }
        public override string ToString()
            => $"{Field.Name}{(Required ? " *" : "")}{(string.IsNullOrWhiteSpace(Field.Documentation) ? "" : " — " + Field.Documentation)}";
    }

    internal sealed class NativeSchemaNode
    {
        public NativeSchemaNode(XElement element, NativeSchemaParticle field, NativeSchemaType type)
        {
            Element = element;
            Field = field;
            Type = type;
            Children = (type?.Particle?.ChildFields() ?? Enumerable.Empty<NativeSchemaChild>()).ToList();
        }

        public XElement Element { get; }
        public NativeSchemaParticle Field { get; }
        public NativeSchemaType Type { get; }
        public IReadOnlyList<NativeSchemaChild> Children { get; }

        internal NativeSchemaRequirements GetRequirements()
        {
            var result = new NativeSchemaRequirements();
            var counts = Element.Elements().Where(child => child.Name.Namespace == XNamespace.None)
                .GroupBy(child => child.Name.LocalName).ToDictionary(group => group.Key, group => group.Count());
            Type?.Particle?.CollectRequirements(counts, result);
            return result;
        }

        public XElement AddChild(NativeSchemaChild child)
        {
            var field = child.Field;
            var added = new XElement(field.Name, field.Fixed ?? field.Default);
            var order = Children.Select(candidate => candidate.Field.Name).ToList();
            // A new occurrence follows the existing members of its repeated group, before the next schema field.
            var index = child.RepeatedGroup == null ? order.IndexOf(field.Name)
                : Children.Select((candidate, position) => new { candidate, position })
                    .Last(item => ReferenceEquals(item.candidate.RepeatedGroup, child.RepeatedGroup)).position;
            var following = Element.Elements().FirstOrDefault(sibling => order.IndexOf(sibling.Name.LocalName) > index);
            if (following == null)
            {
                Element.Add(added);
            }
            else
            {
                following.AddBeforeSelf(added);
            }

            return added;
        }
    }
}
