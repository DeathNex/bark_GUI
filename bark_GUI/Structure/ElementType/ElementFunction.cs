using System.Collections.Generic;

namespace bark_GUI.Structure.ElementType
{
    class ElementFunction : ElementType
    {
        /* PUBLIC PROPRETIES */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public List<string> FunctionNames { get { return _functionNames; } }

        /* PRIVATE VARIABLES */
        readonly List<string> _functionNames;




        //Constructor
        public ElementFunction(List<string> functionNames)
        {
            this._type = EType.Function;
            this._functionNames = functionNames;
        }







        /* PUBLIC METHODS */






    }
}
