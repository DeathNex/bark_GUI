using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementReference : ElementType
    {
        //Constructor
        public ElementReference(SimpleType simpleType)
        {
            CurrentElementType = EType.Reference;
            SimpleType = simpleType;
        }


        public ElementReference DuplicateStructure()
        {
            var newElement = new ElementReference(SimpleType);

            return newElement;
        }
    }
}
