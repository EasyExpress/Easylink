using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Easylink
{
    internal static class ParametersCreator
    {
        internal static Dictionary<string, object> CreatePropertyParameters(string parameterPrefix, object obj,
                                                                            ref string sql)
        {
            var parameters = new Dictionary<string, object>();

            List<string> propertyPathList = FindPropertyPathList(sql);

            foreach (var propertyPath in propertyPathList)
            {
                object propertyParent = Shared.FindPropertyParent(propertyPath, obj);

                if (propertyParent == null)
                {
                    throw new EasylinkException("Error occurred when creating parameters.. {0} property {1} is null.",
                                                obj, propertyPath);
                }


                PropertyInfo property = Shared.FindProperty(propertyPath, propertyParent);

                object propertyValue = property.GetValue(propertyParent, null);


                parameters[propertyPath] = propertyValue;

                string parameterName = propertyPath.Replace(".", "");

                sql = sql.Replace("@" + propertyPath, parameterPrefix + parameterName);
            }

            return parameters;
        }


        private static List<string> FindPropertyPathList(string sql)
        {
            var list = new List<string>();

            foreach (Match match in Regex.Matches(sql, @"@[^) ,\r\n]*"))
            {
                list.Add(match.Groups[0].ToString().TrimStart('@'));
            }

            return list;
        }
    }
}