

namespace Easylink
{
    [System.Serializable]
    public class FixedLengthRule: Rule  
    {


      
      
        public int FixedLength { get; private set; }


        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}  should be size of {2}.", ObjectType.Name, ScreenName, FixedLength); }
        }


        public FixedLengthRule(int fixedLength)
        {

            FixedLength = fixedLength;
            RuleType = RuleType.FixedLength;
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

                if (string.IsNullOrWhiteSpace(temp))
                {
                    Valid = true;
                }
                else
                {
                    Valid = (temp.Length == FixedLength);
                }
            }
        }
    }
}