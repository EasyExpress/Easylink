using System;
using System.Linq.Expressions;

namespace Easylink
{
    [System.Serializable]
    public class CustomRule<T>  : Rule   where T: new()
    {
        private readonly Func<T, string> _validateMethod;

        private string _errorMessage;

        public object ItemToValidate { get; private set;  }


        public CustomRule(Expression<Func<T, object>> expression, Func<T, string> validateMethod)
        {
            _validateMethod = validateMethod;

            RuleType = RuleType.Custom;

            PropertyName = Expressions.GetPropertyName(expression.Body);
            
        }


        public CustomRule(Func<T, string> validateMethod)
        {
            _validateMethod = validateMethod;

            RuleType = RuleType.Custom;

            PropertyName = typeof(T).Name;

        }

    
        public override string ErrorMessage
        {
            get { return string.Format("{0}: {1}", typeof(T).Name, _errorMessage); }
        }

        public override void Validate(object obj)
        {    
            _errorMessage = _validateMethod((T)obj);

            Valid = string.IsNullOrEmpty(_errorMessage);

        }
    }
}