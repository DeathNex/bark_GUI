using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ElementFunction : ElementType
    {
        /* PUBLIC PROPRETIES */
        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public List<string> FunctionNames { get { return functionNames; } }

        /* PRIVATE VARIABLES */
        List<string> functionNames;




        //Constructor
        public ElementFunction(List<string> functionNames)
        {
            this._type = e_type.function;
            this.functionNames = functionNames;
        }







        /* PUBLIC METHODS */






    }
}
