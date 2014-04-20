using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementType
{
    class ElementReference : ElementType
    {
        /* PUBLIC PROPRETY */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SimpleType SimpleType { get; private set; }


        /* PRIVATE VARIABLES */


        //Constructor
        public ElementReference(SimpleType simpleType)
        {
            this._type = EType.Reference;
            this.SimpleType = simpleType;
        }
    }
}
