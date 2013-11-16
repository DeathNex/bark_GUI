using System.Collections.Generic;

namespace bark_GUI.Structure
{
    class Unit
    {
        /* PUBLIC VARIABLES */
        public string Name;

        /* PRIVATE VARIABLES */
        readonly List<string> _options;
        string _selected;

        //Constructor
        public Unit(string name, List<string> options)
        {
            Name = name;
            _options = options;
        }






        /* PUBLIC METHODS */






        public void Select(string option) { _selected = option; }
        public List<string> GetOptions() { return _options; }
    }
}
