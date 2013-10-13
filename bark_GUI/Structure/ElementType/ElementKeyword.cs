using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
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
        SimpleType _simpleType; // xs:decimal
        string _defaultValue;





        //Constructor
        public ElementKeyword(SimpleType simpleType, string defaultValue)
        {
            this._type = e_type.constant;
            this._simpleType = simpleType;
            this._defaultValue = defaultValue;
        }
    }
}
