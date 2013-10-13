using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    class Unit
    {
        /* PUBLIC VARIABLES */
        public string name;

        /* PRIVATE VARIABLES */
        List<string> _options;
        string _selected;

        //Constructor
        public Unit(string _name, List<string> _options)
        {
            name = _name;
            this._options = _options;
        }






        /* PUBLIC METHODS */






        public void Select(string option) { _selected = option; }
        public List<string> GetOptions() { return _options; }
    }
}
