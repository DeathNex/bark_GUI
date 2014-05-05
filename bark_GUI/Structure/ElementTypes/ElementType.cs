using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    public enum EType { Constant, Variable, Function, Reference, Keyword }

    public class ElementType
    {
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public EType CurrentElementType { get; protected set; }

        // Inheriting variables.
        public SimpleType SimpleType { get; protected set; }

        public bool ValueIsValid(string value) { return SimpleType.IsValid(value); }
    }
}
