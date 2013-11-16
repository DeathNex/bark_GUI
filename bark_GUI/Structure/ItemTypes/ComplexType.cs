using System.Collections.Generic;
using bark_GUI.Structure.ElementType;

namespace bark_GUI.Structure.ItemTypes
{
    class ComplexType : ItemType
    {
        /* PUBLIC PROPERTIES */

        public ElementConstant Constant { get; private set; }
        public ElementVariable Variable { get; private set; }
        public ElementFunction Function { get; private set; }
        public ElementKeyword Keyword { get; private set; }
        public ElementReference Reference { get; private set; }


        //Constructor
        public ComplexType(string name, Unit unit, string defaultUnit,
            ElementConstant constant, ElementVariable variable, ElementFunction function, ElementKeyword keyword,
            ElementReference reference)
        {
            Name = name;
            Constant = constant;
            Variable = variable;
            Function = function;
            Keyword = keyword;
            Reference = reference;
            if (Constant != null)
                Constant.SetUnit(unit, defaultUnit);
            if (Variable != null)
                Variable.SetUnit(unit, defaultUnit);
        }

        /* PUBLIC METHODS */

        public List<string> GetUnitOptions()
        {
            if (Variable != null)
                return Variable.Unit.GetOptions();
            return Constant != null ? Constant.Unit.GetOptions() : null;
        }

        public List<string> GetX_UnitOptions()
        {
            return Variable == null ? null : Variable.GetX_UnitOptions();
        }

        public List<string> GetFunctionNames()
        {
            return Function != null ? Function.FunctionNames : null;
        }

        public List<string> GetTypeOptions()
        {
            List<string> types = new List<string>();
            if (Constant != null)
                types.Add("Constant");
            if (Variable != null)
                types.Add("Variable");
            if (Function != null)
                types.Add("Function");
            return types;
        }
    }
}
