using System.Collections.Generic;
using System.Dynamic;

namespace Easylink.Validation
{
    
    public  class ExpandoObject1  : DynamicObject
    {

        private readonly Dictionary<string, object> _dict= new Dictionary<string, object>();

        public ExpandoObject1(IEnumerable<Rule> broken)
        {
            foreach (var temp in broken)
            {
                if (!_dict.ContainsKey(temp.PropertyName))
                {
                    _dict[temp.PropertyName] = temp.ErrorMessage;
                }
            }
        }

     
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            _dict.TryGetValue(binder.Name, out result);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dict[binder.Name] = value;
            return true;
        }
    }
}
