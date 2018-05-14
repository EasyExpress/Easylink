
using System;
namespace Easylink
{
    [System.Serializable]
    public class WordCountRule  : Rule  
    {

        public override string ErrorMessage
        {
            get { return string.Format("{0}: too many words in {1}.", ObjectType.Name, ScreenName); }
        }

        private int _max; 


        public WordCountRule(int max)
        {
            _max = max;
            RuleType = RuleType.WordCount;
        }


        public override void Validate(object value)
        {
            if (value != null)
            {

                var wordCount = CountWords(value);

                if (wordCount > _max)
                {
                    Valid = false;
                }
            }
            else
            {
                Valid = true; 
 
            }
        }

        private int CountWords(object value)
        {
            var  text = value.ToString().Trim();
            int wordCount = 0, index = 0;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;
            }

            return wordCount; 
        }

     }

        
}