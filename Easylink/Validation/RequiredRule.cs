 
namespace Easylink
{
    [System.Serializable]
    public class RequiredRule  : Rule  
    {

        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1} is required.", ObjectType.Name, ScreenName); }
        }

        private object _empty; 

        private RequiredRule()
        {

        }

        public RequiredRule(object empty =null)
        {
            RuleType = RuleType.Required;
            _empty = empty;
        }


        public override void Validate(object value)
        {
            if (value == null)
            {
                Valid = false;
            }
            else
            {
                
               
                string temp = value.ToString();

                if (_empty!=null)
                {

                    if (_empty.ToString() == temp)
                    {
                        Valid = false;
                        return;

                    }
                }

                
                Valid = !string.IsNullOrWhiteSpace(temp);

            }
        }
    }
}