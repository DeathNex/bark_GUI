using System.Collections.Generic;

namespace bark_GUI.Structure.ElementTypes
{
    class ElementFunction : ElementType
    {
        public List<string> FunctionNames { get; private set; }


        // Constructor
        public ElementFunction(List<string> functionNames)
        {
            CurrentElementType = EType.Function;
            FunctionNames = functionNames;
        }
    }
}
