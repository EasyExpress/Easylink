using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace EasylinkApp.WPF 
{
    public static class SharedCode
    {

        public static string PropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            var temp = body.ToString();

            return temp.Substring(temp.IndexOf(".") + 1);
        }

        public static ObservableCollection<T> ToObservable<T>(this List<T> list)  
        {
            var result = new ObservableCollection<T>();

            list.ForEach(t => result.Add(t));

            return result; 
        }

        public static void  Try(Action proc)
        {
            try
            {
                proc(); 
            }
            catch (Exception ex)
            {
                ErrorMessageBox.ShowException(ex);
            }
        }

    }
}
