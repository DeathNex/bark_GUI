using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementConstant : ElementType
    {
        /* PUBLIC PROPRETIES */
        public Unit Unit { get; private set; }
        public string DefaultUnit { get; private set; }



        //Constructor
        public ElementConstant(SimpleType simpleType, string defaultValue)
        {
            CurrentElementType = EType.Constant;
            SimpleType = simpleType;
            DefaultValue = defaultValue;
        }







        /* PUBLIC METHODS */
        public void SetUnit(Unit unit, string defaultUnit) { this.Unit = unit; this.DefaultUnit = defaultUnit; Unit.Select(DefaultUnit); }
    }
}
