using System.Collections.Generic;
using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementKeyword : ElementType
    {
        /* PUBLIC PROPRETIES */
        public List<string> KeywordOptions { get { return SimpleType.GetOptions(); } }





        //Constructor
        public ElementKeyword(SimpleType simpleType, string defaultValue)
        {
            CurrentElementType = EType.Keyword;
            SimpleType = simpleType;
            DefaultValue = defaultValue;
        }





        public ElementKeyword DuplicateStructure()
        {
            var newElement = new ElementKeyword(SimpleType, DefaultValue);

            return newElement;
        }
    }
}
