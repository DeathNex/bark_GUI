using System.Collections.Generic;

namespace bark_GUI.Structure
{
    class Unit
    {
        /* PUBLIC VARIABLES */
        public string Name;
        public string Selected;

        /* PRIVATE VARIABLES */
        readonly List<string> _options;

        //Constructor
        public Unit(string name, List<string> options)
        {
            Name = name;
            _options = options;
        }






        /* PUBLIC METHODS */






        public void Select(string option) { Selected = option; }
        public List<string> GetOptions() { return new List<string>(_options); }

        public Unit DuplicateStructure()
        {
            return new Unit(Name, GetOptions());
        }
    }
}
