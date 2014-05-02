using System.Collections.Generic;
using bark_GUI.Structure.ElementTypes;

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
        public ComplexType(string name, List<ElementType> elements)
        {
            Name = name;

            foreach (var element in elements)
            {
                if(element == null) continue;
                switch (element.CurrentElementType)
                {
                    case EType.Constant:
                        Constant = (ElementConstant)element;
                        break;
                    case EType.Variable:
                        Variable = (ElementVariable)element;
                        break;
                    case EType.Function:
                        Function = (ElementFunction)element;
                        break;
                    case EType.Keyword:
                        Keyword = (ElementKeyword)element;
                        break;
                    case EType.Reference:
                        Reference = (ElementReference)element;
                        break;

                }
            }
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

        public List<string> GetKeywordOptions()
        {
            return Keyword != null ? Keyword.KeywordOptions : null;
        }
    }
}
