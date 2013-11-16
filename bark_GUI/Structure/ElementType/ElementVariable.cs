using System.Collections.Generic;
using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementType
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
        public Unit X_Unit { get { return _xUnit; } }
        public string DefaultUnit { get { return _defaultUnit; } }

        /* PRIVATE VARIABLES */
        private Unit _unit;
        private string _defaultUnit;
        private SimpleType _simpleType; // xs:decimal
        private Unit _xUnit;
        private string _defaultXUnit;



        //Constructor
        public ElementVariable(SimpleType simpleType)
        {
            this._type = EType.Variable;
            this._simpleType = simpleType;
        }







        /* PUBLIC METHODS */




        public void SetUnit(Unit unit, string defaultUnit) { this._unit = unit; this._defaultUnit = defaultUnit; _unit.Select(_defaultUnit); }
        public void SetX_Unit(Unit xUnit, string defaultXUnit) { this._xUnit = xUnit; this._defaultXUnit = defaultXUnit; xUnit.Select(defaultXUnit); }
        public List<string> GetX_UnitOptions() { if (_xUnit != null) return _xUnit.GetOptions(); return null; }











        /* PRIVATE METHODS */

        private static bool _isABreaker(char c)
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

        private static string _trimVariableTable(string data)
        {
            data = data.Trim(new char[4] { ' ', '\r', '\n', '\t' });

            var i = 1;

            while (i < data.Length)
            {
                var c = data[i];
                var prev = data[i - 1];
                if (_isABreaker(c) && _isABreaker(prev))
                {
                    data = prev != '\n' ? data.Remove(i - 1, 1) : data.Remove(i, 1);
                    i--;
                }
                i++;
            }
            return data;
        }
    }
}
