using System.Collections.Generic;
using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementVariable : ElementType
    {
        public Unit Unit { get; private set; }
        public Unit XUnit { get; private set; }
        public string DefaultUnit { get; private set; }
        public string DefaultXUnit { get; private set; }



        //Constructor
        public ElementVariable(SimpleType simpleType, string defaultValue)
        {
            CurrentElementType = EType.Variable;
            SimpleType = simpleType;
            DefaultValue = defaultValue;
        }







        /* PUBLIC METHODS */
        
        public void SetUnit(Unit unit, string defaultUnit) { this.Unit = unit; this.DefaultUnit = defaultUnit; Unit.Select(defaultUnit); }
        public void SetX_Unit(Unit xUnit, string defaultXUnit) { this.XUnit = xUnit; DefaultXUnit = defaultXUnit; xUnit.Select(defaultXUnit); }
        public List<string> GetX_UnitOptions() { return XUnit != null ? XUnit.GetOptions() : null; }

        public ElementVariable DuplicateStructure()
        {
            var newElement = new ElementVariable(SimpleType, DefaultValue);
            newElement.SetUnit(Unit.DuplicateStructure(), DefaultUnit);
            if(XUnit != null)
                newElement.SetX_Unit(XUnit.DuplicateStructure(), DefaultXUnit);

            return newElement;
        }

        //public override bool ValueIsValid(string value)
        //{
        //    var values = value.Split(new[] { '\n', '\t', ' ' });


        //    return base.ValueIsValid(value);
        //}

        /* PRIVATE METHODS */

        //private static bool _isABreaker(char c)
        //{
        //    switch (c)
        //    {
        //        case '\n':
        //            return true;
        //        case '\r':
        //            return true;
        //        case ' ':
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        //private static string _trimVariableTable(string data)
        //{
        //    data = data.Trim(new char[4] { ' ', '\r', '\n', '\t' });

        //    var i = 1;

        //    while (i < data.Length)
        //    {
        //        var c = data[i];
        //        var prev = data[i - 1];
        //        if (_isABreaker(c) && _isABreaker(prev))
        //        {
        //            data = prev != '\n' ? data.Remove(i - 1, 1) : data.Remove(i, 1);
        //            i--;
        //        }
        //        i++;
        //    }
        //    return data;
        //}
    }
}
