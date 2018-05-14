using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace Easylink
{
    [System.Serializable]
    public class Rule
    {
     
        public bool Valid { get; set; }

        public string ScreenName { get; set; }
       
        public string PropertyName { get; set; }

        public Guid RuleKey { get; set; }

        public RuleType RuleType { get; set; }

       
        public Type ObjectType { get; set; }

        public dynamic ControlToValidate { get; set; }

        public virtual string ErrorMessage
        {
            get { return string.Empty; }
        }


 

        public Rule()
        {
            RuleKey = Guid.NewGuid();
        }

   
    
        internal  void Setup<T>(Expression<Func<T, object>> expression)
        {
            PropertyName = Expressions.GetPropertyName(expression.Body);

            ObjectType = typeof(T);
        }
         

        public virtual void Validate(object value)
        {
        }
    }
}