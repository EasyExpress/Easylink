using System;
 
using System.Text.RegularExpressions;

namespace Easylink
{
    [Serializable]
    public class NumericRule : Rule  
    {


 

        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}  should be numeric.", ObjectType.Name, ScreenName); }
        }



        public NumericRule()
        {
            RuleType = RuleType.Numeric;
        }


        public override void Validate(object value)
        {
            Valid = IsNumeric(value);
        }


        private bool IsNumeric(object value)
        {
            if (value == null) return true;

            string temp = value.ToString();

            if (String.IsNullOrWhiteSpace(temp))
                return true;

            var regEx = new Regex(@"^\d+$");

            return regEx.Match(temp).Success;
        }
    }
}