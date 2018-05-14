using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Easylink.Validation;

namespace Easylink
{
    [Serializable]
    public class BusinessBase
    {
        [NonSerialized] private Validator _validator;


        private List<Rule> _rules;


        public  List<Rule> Rules
        {
            get
            {
                if (_rules == null)
                {
                    _rules = new List<Rule>();
                }
                return _rules;
            }
        }



        public BusinessBase()
        {
            LifeCycleStatus = BusinessBaseLifeCycle.New;
            _rules = new List<Rule>();

        }

        private Validator Validator
        {
            get
            {
                if (_validator == null)
                {
                    _validator = new Validator();

             
                }
                return _validator;
            }
        }


        public BusinessBaseLifeCycle LifeCycleStatus { get;   set; }

      

     
        public List<Rule> ValidateRules(params Rule[] rules)
        {
           return  Validator.Validate(this, rules);
 
        }



        public dynamic Validation
        {
            get
            {
                return  new ExpandoObject1(Validator.Broken);

            }
        }



        public void Validate(params Rule[] rules)
        {
            var broken  = ValidateRules(rules);

            if (broken.Count > 0)
            {
                throw new EasylinkException(broken[0].ErrorMessage);
            }

        }


        public void AddRules<T>(Expression<Func<T, object>> expression, string screenName, params Rule[] rules)
        {
            AddRules(expression, rules);

            foreach (var rule in rules)
            {
                rule.ScreenName = screenName;

            }
        }

        public void AddRules<T>(Expression<Func<T, object>> expression, params Rule[] rules)
        {
            foreach (var rule in rules)
            {
                rule.Setup(expression);
 
                Rules.Add(rule);

                rule.ScreenName = rule.PropertyName;

            }
        }

        public void AddCustomRules<T>(params CustomRule<T>[] rules) where T : new()
        {
            foreach (var rule in rules)
            {
                Rules.Add(rule);

            }
        }


        internal void OnRetrieved()
        {
            LifeCycleStatus = BusinessBaseLifeCycle.Old;


            PropertyInfo[] properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                if (typeof (BusinessBase).IsAssignableFrom(property.PropertyType))
                {
                    var obj = property.GetValue(this, null) as BusinessBase;

                    if (obj != null)
                    {
                        obj.OnRetrieved();
                    }
                }
            }
        }

    


        public void MarkAsDeleted()
        {
            LifeCycleStatus = BusinessBaseLifeCycle.Deleted;
        }

 
    }
}