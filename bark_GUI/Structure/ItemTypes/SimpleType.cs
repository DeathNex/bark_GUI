using System.Collections.Generic;

namespace bark_GUI.Structure.ItemTypes
{
    // XSD Restriction types
    // Basic :       basic type restriction (e.g. string or int)
    // Enumeration : one of the available (preset) options
    // MaxMin :      basic type number with maximum and/or minimum values. (inclusive)
    public enum Restriction { Basic, Enumeration, MaxMin }
    public enum BasicType { String, Integer, Decimal }

    public class SimpleType : ItemType
    {
        //Enumeration
        readonly List<string> _options;

        // Validation Fields
        // Basic/Enumeration/MaxMin
        readonly Restriction _restriction;
        // String/Integer/Decimal
        readonly BasicType _basicType;
        // MaxMin Inclusive
        readonly decimal _max = decimal.MaxValue;
        readonly decimal _min = decimal.MinValue;





        #region Constructors

        //Constructor for General
        public SimpleType(string name, BasicType basicType)
        {
            Name = name;
            _restriction = Restriction.Basic;
            _basicType = basicType;
        }
        //Constructor for Max/Min
        public SimpleType(string name, BasicType basicType, decimal min, decimal max)
        {
            Name = name;
            _restriction = Restriction.MaxMin;
            _basicType = basicType;
            _max = max;
            _min = min;

            // Handle decimal not able to check number equality. (precision is not required)
            if (basicType == BasicType.Decimal)
            {
                _min = _min - (1 / 999999999999);
                _max = _max + (1 / 999999999999);
            }
        }
        //Constructor for Enumeration
        public SimpleType(string name, List<string> options)
        {
            Name = name;
            _restriction = Restriction.Enumeration;
            _basicType = BasicType.String;
            _options = options;
        }

        #endregion




        /* PUBLIC METHODS */
        public List<string> GetOptions() { return _options; }

        public bool IsValid(string inputData)
        {
            switch (_restriction)
            {
                case Restriction.Enumeration:
                    return GetOptions().Contains(inputData);
                case Restriction.Basic:
                    switch (_basicType)
                    {
                        case BasicType.String:
                            return true;
                        case BasicType.Integer:
                            int resultInt;
                            return int.TryParse(inputData, out resultInt);
                        case BasicType.Decimal:
                            decimal resultDecimal;
                            return decimal.TryParse(inputData, out resultDecimal);
                    }
                    break;
                case Restriction.MaxMin:
                    switch (_basicType)
                    {
                        case BasicType.Integer:
                            int resultInt;

                            if (!int.TryParse(inputData, out resultInt))
                                return false;

                            return ((resultInt >= _min) && (resultInt <= _max));
                        case BasicType.Decimal:
                            decimal resultDecimal;

                            if (!decimal.TryParse(inputData, out resultDecimal))
                                return false;

                            return ((resultDecimal > _min) && (resultDecimal < _max));
                    }
                    break;
            }

            return false;
        }
    }
}
