using System.Collections.Generic;

namespace bark_GUI.Structure.ItemTypes
{
    enum Restriction { Basic, Enumeration, MaxMin }
    enum BasicType { String, Integer, Decimal }

    class SimpleType : ItemType
    {
        //MaxMin InclusiveExclusive
        double _max = double.MaxValue;
        double _min = double.MinValue;

        //Enumeration
        readonly List<string> _options;

        //BasicType
        BasicType _basicType;

        //XSD Restriction
        Restriction _restriction;





        //Constructors

        //Constructor for General
        public SimpleType(string name, BasicType basicType)
        {
            Name = name;
            _restriction = Restriction.Basic;
            _basicType = basicType;
        }
        //Constructor for Max/Min
        public SimpleType(string name, BasicType basicType, double min, double max)
        {
            Name = name;
            _restriction = Restriction.MaxMin;
            _basicType = basicType;
            _max = max;
            _min = min;
        }
        //Constructor for Enumeration
        public SimpleType(string name, List<string> options)
        {
            Name = name;
            _restriction = Restriction.Enumeration;
            _basicType = BasicType.String;
            _options = options;
        }






        /* PUBLIC METHODS */
        public List<string> GetOptions() { return _options; }
    }
}
