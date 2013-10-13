using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ElementConstant : ElementType
    {
        /* PUBLIC PROPRETY */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public Unit Unit { get { return _unit; } }
        public string DefaultUnit { get { return _defaultUnit; } }



        /* PRIVATE VARIABLES */
        private Unit _unit;
        private string _defaultUnit;
        SimpleType _simpleType; // xs:decimal
        string _defaultValue;





        //Constructor
        public ElementConstant(SimpleType simpleType, string defaultValue)
        {
            this._type = e_type.constant;
            this._simpleType = simpleType;
            this._defaultValue = defaultValue;
        }







        /* PUBLIC METHODS */
        public void SetUnit(Unit unit, string defaultUnit) { this._unit = unit; this._defaultUnit = defaultUnit; _unit.Select(_defaultUnit); }
    }
}
