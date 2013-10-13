using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class ItemType
    {
        /* INHERiTING VARIABLES */
        public string name;

        /* INHERiTING METHODS */
        public virtual bool isComplexType() { return false; }
        public virtual bool isSimpleType() { return false; }
    }
}
