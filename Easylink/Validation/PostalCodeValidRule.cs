using System.Text.RegularExpressions;

namespace Easylink
{
    [System.Serializable]
    public class PostalCodeValidRule: Rule  
    {


        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}  is invalid.", ObjectType.Name, ScreenName); }
        }


        public PostalCodeValidRule()
        {

            RuleType = RuleType.PostalCode;
        }

        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = true;
            }
            else
            {
                string temp = value.ToString();


                string validPostalCodePattern =
                    @"[ABCEFGHJKLMNPRSTVXY][0-9][ABCEFGHJKLMNPRSTVWXYZ] ?[0-9][ABCEFGHJKLMNPRSTVWXYZ][0-9]";

                var regex = new Regex(validPostalCodePattern, RegexOptions.IgnoreCase);


                Valid = regex.IsMatch(temp);
            }
        }
    }
}