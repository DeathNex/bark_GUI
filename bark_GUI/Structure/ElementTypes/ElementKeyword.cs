using System.Collections.Generic;
using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementKeyword : ElementType
    {
        /* PUBLIC PROPRETIES */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public List<string> KeywordOptions { get { return _simpleType.GetOptions(); } }

        public string DefaultValue { get; set; }
        public SimpleType SimpleType { get { return _simpleType; } }



        /* PRIVATE VARIABLES */
        readonly SimpleType _simpleType; // xs:decimal





        //Constructor
        public ElementKeyword(SimpleType simpleType, string defaultValue)
        {
            _type = EType.Keyword;
            _simpleType = simpleType;
            DefaultValue = defaultValue;
        }
    }
}
