 
namespace Easylink
{
    [System.Serializable]
    public class MaximumLengthRule : Rule 
    {

       
        public int MaximumLength { get; private set; }


        public override string ErrorMessage
        {
            get
            {
                return string.Format("{0}: {1} should be no more than size {2}.", ObjectType.Name, ScreenName,
                                     MaximumLength);
            }
        }

        public MaximumLengthRule(int maximumLength)
        {
            MaximumLength = maximumLength;
            RuleType = RuleType.MaximumLength;
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
                throw new ValidationException("Maximumlengh rule only applies to string type. Type {0} is not string",
                                              value.GetType().Name);

            }

            
            string temp = value.ToString();

             Valid = (temp.Length <= MaximumLength);
            
        }
    }
}