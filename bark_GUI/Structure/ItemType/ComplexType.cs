using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ComplexType : ItemType
    {
        /* PUBLIC PROPERTIES */

        public ElementConstant Constant { get { return _constant; } }
        public ElementVariable Variable { get { return _variable; } }
        public ElementFunction Function { get { return _function; } }
        public ElementKeyword Keyword { get { return _keyword; } }
        public ElementReference Reference { get { return _reference; } }

        /* PRIVATE VARIABLES */
        private ElementConstant _constant;
        private ElementVariable _variable;
        private ElementFunction _function;
        private ElementKeyword _keyword;
        private ElementReference _reference;


        //Constructor
        public ComplexType(string name, Unit unit, string defaultUnit,
            ElementConstant constant, ElementVariable variable, ElementFunction function, ElementKeyword keyword,
            ElementReference reference)
        {
            this.name = name;
            _constant = constant;
            _variable = variable;
            _function = function;
            _keyword = keyword;
            _reference = reference;
            if (_constant != null)
                _constant.SetUnit(unit, defaultUnit);
            if (_variable != null)
                _variable.SetUnit(unit, defaultUnit);
        }

        /* PUBLIC METHODS */

        public List<string> GetUnitOptions() { if (_variable != null) return _variable.Unit.GetOptions(); if (_constant != null) return _constant.Unit.GetOptions(); return null; }
        public List<string> GetX_UnitOptions() { if (_variable == null) return null; return _variable.GetX_UnitOptions(); }
        public List<string> GetFunctionNames() { if (_function != null) return _function.FunctionNames; return null; }
        public List<string> GetTypeOptions()
        {
            List<string> types = new List<string>();
            if (_constant != null)
                types.Add("Constant");
            if (_variable != null)
                types.Add("Variable");
            if (_function != null)
                types.Add("Function");
            return types;
        }
        public override bool isComplexType() { return true; }
        public override bool isSimpleType() { return false; }
    }
}
