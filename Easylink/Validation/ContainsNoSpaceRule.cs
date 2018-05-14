namespace Easylink
{
    [System.Serializable]
    public class ContainsNoSpaceRule : Rule 
    {

        public  ContainsNoSpaceRule()
        {
        
            RuleType = RuleType.ContainsNoSpace;
        }
 

        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1} cannot contain spaces.", ObjectType.Name, ScreenName); }
        }


        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = true;
            }
            else
            {
                bool containsSpace = value.ToString().Contains(" ");

                Valid = !containsSpace;
            }
        }
    }
}