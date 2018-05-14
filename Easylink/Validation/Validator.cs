
using System.Collections.Generic;

namespace Easylink
{
    internal  class Validator
    {
     
        private List<Rule> _broken;

        internal  Validator()
        {
          
            _broken = new List<Rule>(); 
        }


        public  List<Rule> Broken
        {
            get
            {
                return _broken;
            }
        }
 
        internal  List<Rule> Validate(BusinessBase businessBase, params Rule[] rules)
        {
            businessBase.Rules.ForEach(r => r.Valid = true);

            businessBase.Rules.AddRange(rules);

            foreach (var rule in businessBase.Rules)
            {
               

                ValidateRule(rule, businessBase);
            }


           _broken= businessBase.Rules.FindAll(r => r.Valid == false);

           return _broken;
 
        }
 
 

        private void ValidateRule(Rule rule, BusinessBase businessBase)
        {
            if (rule.RuleType == RuleType.Custom)
            {
                rule.Validate(businessBase);
            }

            else if (!string.IsNullOrEmpty(rule.PropertyName))
            {
                object value = Shared.FindPropertyValue(rule.PropertyName, businessBase);

                rule.Validate(value);
            }
        }
 
 
    }
}