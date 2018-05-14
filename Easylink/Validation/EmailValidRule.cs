 
using System.Text.RegularExpressions;

namespace Easylink
{
    [System.Serializable]
    public class EmailValidRule : Rule  
    {


        public EmailValidRule()
        {
        
              RuleType = RuleType.Email;
        }
  

        
        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}  is invalid.", ObjectType.Name, ScreenName); }
        }


        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = true;
            }
            else
            {
                var temp = value.ToString();


                var  validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);


                Valid = regex.IsMatch(temp);
            }
        }
    }
}