using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ElementVariable : ElementType
    {
        /* PUBLIC PROPRETY */
        public override string Value
        {
            get{ return _value; }
            set{ _value = _trimVariableTable(value); }
        }
        public Unit Unit { get { return _unit; } }
        public Unit X_Unit { get { return x_unit; } }
        public string DefaultUnit { get { return _defaultUnit; } }

        /* PRIVATE VARIABLES */
        private Unit _unit;
        private string _defaultUnit;
        private SimpleType _simpleType; // xs:decimal
        private Unit x_unit;
        private string defaultX_Unit;



        //Constructor
        public ElementVariable(SimpleType _simpleType)
        {
            this._type = e_type.variable;
            this._simpleType = _simpleType;
        }







        /* PUBLIC METHODS */




        public void SetUnit(Unit unit, string defaultUnit) { this._unit = unit; this._defaultUnit = defaultUnit; _unit.Select(_defaultUnit); }
        public void SetX_Unit(Unit x_unit, string defaultX_Unit) { this.x_unit = x_unit; this.defaultX_Unit = defaultX_Unit; x_unit.Select(defaultX_Unit); }
        public List<string> GetX_UnitOptions() { if (x_unit != null) return x_unit.GetOptions(); return null; }











        /* PRIVATE METHODS */

        private bool _isABreaker(char c)
        {
            switch (c)
            {
                case '\n':
                    return true;
                case '\r':
                    return true;
                case ' ':
                    return true;
                default:
                    return false;
            }
        }

        private string _trimVariableTable(string data)
        {
            data = data.Trim(new char[4] { ' ', '\r', '\n', '\t' });

            char prev = data[0];
            int i = 1;

            while (i < data.Length)
            {
                char c = data[i];
                prev = data[i - 1];
                if (_isABreaker(c) && _isABreaker(prev))
                {
                    if (prev != '\n')
                        data = data.Remove(i - 1, 1);
                    else
                        data = data.Remove(i, 1);
                    i--;
                }
                i++;
            }
            return data;
        }
    }
}
