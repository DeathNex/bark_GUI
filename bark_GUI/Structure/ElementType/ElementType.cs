using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    public enum e_type { constant, variable, function, reference, keyword }

    public class ElementType
    {
        public virtual string Value { get { return _value; } set { _value = value; } }
        public e_type Type { get { return _type; } }
        
        protected e_type _type;
        protected string _value;
    }
}
