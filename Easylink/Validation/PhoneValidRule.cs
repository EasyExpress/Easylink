using System;
using System.Text.RegularExpressions;

namespace Easylink
{
    [Serializable]
    public class PhoneValidRule : Rule
    {
 
        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}  is invalid.", ObjectType.Name, ScreenName); }
        }


        public PhoneValidRule()
        {
            RuleType = RuleType.Phone;
        }
 
 
        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = true;
            }
            else
            {
                var  temp = value.ToString();


                var  validPhonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

                var regex = new Regex(validPhonePattern, RegexOptions.IgnoreCase);


                Valid = regex.IsMatch(temp);
            }
        }
    }
}