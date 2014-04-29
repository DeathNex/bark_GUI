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
    }
}
