using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ElementReference : ElementType
    {
        /* PUBLIC PROPRETY */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public SimpleType SimpleType { get { return _simpleType; } }



        /* PRIVATE VARIABLES */
        SimpleType _simpleType; // xs:decimal





        //Constructor
        public ElementReference(SimpleType simpleType)
        {
            this._type = e_type.constant;
            this._simpleType = simpleType;
        }
    }
}
