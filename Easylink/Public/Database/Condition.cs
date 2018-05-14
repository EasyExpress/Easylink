
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Easylink
{
    public static class Condition
    {
        public static string CaseSensitive(this string text)
        {
            return text;
        }


        public static bool In<T, TItem>(T value, params TItem[] list)
        {
            return value != null && Flatten(list).Any(obj => obj.ToString() == value.ToString());
        }

        internal   static List<object> Flatten(IEnumerable list)
        {
            var ret = new List<object>();
            if (list == null) return ret;

            foreach (var item in list)
            {
                if (item == null) continue;

                var arr = item as IEnumerable;
                if (arr != null && !(item is string))
                {
                    ret.AddRange(arr.Cast<object>());
                }
                else
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
    }
}
