using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementType
{
    class ElementKeyword : ElementType
    {
        /* PUBLIC PROPRETIES */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public SimpleType SimpleType { get { return _simpleType; } }



        /* PRIVATE VARIABLES */
        readonly SimpleType _simpleType; // xs:decimal
        string _defaultValue;





        //Constructor
        public ElementKeyword(SimpleType simpleType, string defaultValue)
        {
            this._type = EType.Constant;
            this._simpleType = simpleType;
            this._defaultValue = defaultValue;
        }
    }
}
