using System;


namespace Easylink
{
    [System.Serializable]
    public class ThresholdRule : Rule  
    {
        private readonly string _comparisonOperator;

        private  readonly object _thresholdValue;

        public ThresholdRule(string comparisonOperator, object thresholdValue)
        {
            RuleType = RuleType.Threshold;
            _thresholdValue = thresholdValue;
            _comparisonOperator = comparisonOperator;
        }




        public override string ErrorMessage
        {
            get
            {
                return string.Format("{0}: {1} should {2}{3}", ObjectType.Name, ScreenName, _comparisonOperator,
                                     _thresholdValue);
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
                (_thresholdValue is decimal || _thresholdValue is int || _thresholdValue is double ||
                 _thresholdValue is long))

            {
                string valueString = value.ToString();
                string thresholdValueString = _thresholdValue.ToString();

                switch (_comparisonOperator)
                {
                    case ">":
                        Valid = decimal.Parse(valueString) > decimal.Parse(thresholdValueString);
                        break;

                    case "<":
                        Valid = decimal.Parse(valueString) < decimal.Parse(thresholdValueString);
                        break;

                    case ">=":
                        Valid = decimal.Parse(valueString) >= decimal.Parse(thresholdValueString);
                        break;

                    case "<=":

                        Valid = decimal.Parse(valueString) <= decimal.Parse(thresholdValueString);
                        break;
                }


                return;
            }

            if (value is DateTime && _thresholdValue is DateTime)
            {
                switch (_comparisonOperator)
                {
                    case ">":
                        Valid = (DateTime) value > (DateTime) _thresholdValue;
                        break;

                    case "<":
                        Valid = (DateTime) value < (DateTime) _thresholdValue;
                        break;

                    case ">=":
                        Valid = (DateTime) value >= (DateTime) _thresholdValue;
                        break;

                    case "<=":

                        Valid = (DateTime) value <= (DateTime) _thresholdValue;
                        break;
                }
                return;
            }

            throw new ValidationException("Threshold rule: the type {0} is not supported.", value.GetType().Name);
        }
    }
}