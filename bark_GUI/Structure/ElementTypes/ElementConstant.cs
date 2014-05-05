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
        public void SetUnit(Unit unit, string defaultUnit) { Unit = unit; DefaultUnit = defaultUnit; Unit.Select(DefaultUnit); }

        public ElementConstant DuplicateStructure()
        {
            var newElement = new ElementConstant(SimpleType, DefaultValue);
            newElement.SetUnit(Unit.DuplicateStructure(), DefaultUnit);

            return newElement;
        }
    }
}
