 

namespace Easylink
{
    [System.Serializable]
    public class MinimumLengthRule : Rule  
    {

    

        public int MinimumLength { get; private set; }


        public override string ErrorMessage
        {
            get
            {
                return string.Format("{0}: {1} should be  equal or more than size {2}.", ObjectType.Name, ScreenName,
                                     MinimumLength);
            }
        }



        public MinimumLengthRule(int minimumLength)
        {
            MinimumLength = minimumLength;

            RuleType = RuleType.MinimumLength;
        }

 

        public override void Validate(object value)
        {

            if (value == null)
            {
                Valid = true;
                return; 
            }

            if (!(value is string))
            {
                throw new ValidationException("Minimumlength rule only applies to string type. Type {0} is not string",
                                              value.GetType().Name);

            }

            string temp = value.ToString();

            Valid = (temp.Length >= MinimumLength);
          
        }
    }
}