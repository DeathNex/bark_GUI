using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementConstant : ElementType
    {
        /* PUBLIC PROPRETY */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string DefaultValue { get; set; }

        public Unit Unit { get; private set; }
        public string DefaultUnit { get; private set; }


        /* PRIVATE VARIABLES */
        SimpleType _simpleType; // xs:decimal





        //Constructor
        public ElementConstant(SimpleType simpleType, string defaultValue)
        {
            this._type = EType.Constant;
            this._simpleType = simpleType;
            DefaultValue = defaultValue;
        }







        /* PUBLIC METHODS */
        public void SetUnit(Unit unit, string defaultUnit) { this.Unit = unit; this.DefaultUnit = defaultUnit; Unit.Select(DefaultUnit); }
    }
}
