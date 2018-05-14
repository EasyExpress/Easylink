using System;
using System.Globalization;


namespace Easylink
{
    [System.Serializable]
    public class RangeRule : Rule
    {
        private readonly object _from;

        private  readonly object _to;

        private readonly string _dateFormat; 

        public RangeRule(object from, object to)
        {
            RuleType = RuleType.Threshold;
            _from = from;
            _to = to;

            _dateFormat = "yyyy-MM-dd";
        }

        public RangeRule(object from, object to, string dateFormat)
        {
            RuleType = RuleType.Threshold;
            _from = from;
            _to = to;

            _dateFormat = dateFormat;

        }



        public override string ErrorMessage
        {
            get
            {
                if (_from is DateTime && _to is DateTime)
                {
                    return string.Format("{0}: {1} value should between {2} and  {3}", ObjectType.Name, ScreenName,
                                         ((DateTime) _from).ToString(_dateFormat,CultureInfo.InvariantCulture),
                                         ((DateTime) _to).ToString(_dateFormat,CultureInfo.InvariantCulture));

                }

                return string.Format("{0}: {1} value should between {2} and {3}", ObjectType.Name, ScreenName,
                                     _from, _to);
 
            }
        }

        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = true;
                return;
            }

            if ((value is decimal || value is int || value is double || value is long) &&
                (_from is decimal || _from is int || _from is double ||
                 _from is long) &&
                (_to is decimal || _to is int || _to is double || _to is long))

            {
                var valueString = value.ToString();
                var fromString = _from.ToString();
                var toString = _to.ToString();


                Valid = (decimal.Parse(valueString) >= decimal.Parse(fromString) &&
                         decimal.Parse(valueString) <= decimal.Parse(toString));



                return;
            }

            if (value is DateTime && _from is DateTime && _to is DateTime)
            {
                Valid =( (DateTime) value >= (DateTime) _from && (DateTime) value <= (DateTime) _to);

                return;
            }
            
          
            throw new ValidationException("Range rule: invalid value {0}, or from and to value is not supported.", value.GetType().Name);
        }
    }
}