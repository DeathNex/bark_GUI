namespace bark_GUI.Structure.ElementType
{
    public enum EType { Constant, Variable, Function, Reference, Keyword }

    public class ElementType
    {
        public virtual string Value { get { return _value; } set { _value = value; } }
        public EType Type { get { return _type; } }
        
        protected EType _type;
        protected string _value;
    }
}
