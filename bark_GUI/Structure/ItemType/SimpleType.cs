using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bark_GUI
{
    enum Restriction { basic, enumeration, maxMin }
    enum BasicType { _string, _integer, _decimal }

    class SimpleType : ItemType
    {
        //MaxMin InclusiveExclusive
        double max = double.MaxValue;
        double min = double.MinValue;

        //Enumeration
        List<string> options;

        //BasicType
        BasicType basicType;

        //XSD Restriction
        Restriction restriction;





        //Constructors

        //Constructor for General
        public SimpleType(string name, BasicType basicType)
        {
            this.name = name;
            restriction = Restriction.basic;
            this.basicType = basicType;
        }
        //Constructor for Max/Min
        public SimpleType(string name, BasicType basicType, double min, double max)
        {
            this.name = name;
            restriction = Restriction.maxMin;
            this.basicType = basicType;
            this.max = max;
            this.min = min;
        }
        //Constructor for Enumeration
        public SimpleType(string name, List<string> options)
        {
            this.name = name;
            restriction = Restriction.enumeration;
            basicType = BasicType._string;
            this.options = options;
        }






        /* PUBLIC METHODS */
        public override bool isComplexType() { return false; }
        public override bool isSimpleType() { return true; }
        public List<string> GetOptions() { return options; }
    }
}
