using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Easylink
{
    internal static class ObjectMapper
    {
        
        
        internal  static List<T> MapTableToList<T>(DataTable table) where T : new()
        {
            var list = new List<T>();

            if (table == null || table.Rows == null || table.Rows.Count <= 0)
            {
                return list;
            }

            foreach (DataRow row in table.Rows)
            {
                var item = MapRowToObject<T>(row);

                if (item is BusinessBase)
                {
                    (item as BusinessBase).OnRetrieved();
                }

                list.Add(item);
            }

            return list;
        }


        internal static List<object> MapTableToList(Type objectType, DataTable table) 
        {
            var list = new List<object>();

            if (table == null || table.Rows == null || table.Rows.Count <= 0)
            {
                return list;
            }

            foreach (DataRow row in table.Rows)
            {
                var item = MapRowToObject(objectType, row);

                if (item is BusinessBase)
                {
                    (item as BusinessBase).OnRetrieved();
                }

                list.Add(item);
            }

            return list;
        }


        internal  static TCollection MapToObjectCollection<TCollection, T>(DataTable table)
            where T : new()
            where TCollection : new()
        {
            var collection = new TCollection();

            if (table == null)
            {
                return collection;
            }


            foreach (DataRow row in table.Rows)
            {
                var obj = MapRowToObject<T>(row);

                AddToCollection(collection, obj);
            }

            return collection;
        }


        private static void AddToCollection<TCollection>(TCollection collection, object obj)
        {
            MethodInfo method = collection.GetType().GetMethod("Add");

            method.Invoke(collection, new[] {obj});
        }


        private static object  MapRowToObject(Type objectType, DataRow row) 
        {
            if (row == null)
            {
                throw new EasylinkException("Error occured when mapping object. Row is null.");
            }


            var obj = Activator.CreateInstance(objectType);


            foreach (DataColumn col in row.Table.Columns)
            {
                string propertyPath = col.ColumnName;

                object propertyParent = Shared.FindPropertyParent(propertyPath, obj);

                var  property = Shared.FindProperty(propertyPath, propertyParent);

                SetPropertyValue(row[propertyPath], propertyParent, property);
            }


            return obj;
        }

        private static T MapRowToObject<T>(DataRow row) where T : new()
        {
            if (row == null)
            {
                throw new EasylinkException("Error occured when mapping object. Row is null.");
            }


            var obj = new T();


            foreach (DataColumn col in row.Table.Columns)
            {
                string propertyPath = col.ColumnName;

                object propertyParent = Shared.FindPropertyParent(propertyPath, obj);

                PropertyInfo property = Shared.FindProperty(propertyPath, propertyParent);

                SetPropertyValue(row[propertyPath], propertyParent, property);
            }


            return obj;
        }


        private static void SetPropertyValue(object value, object obj, PropertyInfo propertyInfo)
        {
            try
            {
                if (value != DBNull.Value)
                {
                    if (propertyInfo.PropertyType == typeof (int) || propertyInfo.PropertyType == typeof (int?))
                    {
                        SetInt32PropertyValue(propertyInfo, obj, value);
                    }

                    else if (propertyInfo.PropertyType == typeof (Int64) || propertyInfo.PropertyType == typeof (Int64?))
                    {
                        SetInt64PropertyValue(propertyInfo, obj, value);
                    }

                    else if (propertyInfo.PropertyType == typeof (decimal) ||
                             propertyInfo.PropertyType == typeof (decimal?))
                    {
                        SetDecimalPropertyValue(propertyInfo, obj, value);
                    }


                    else if (propertyInfo.PropertyType == typeof (bool) ||
                             propertyInfo.PropertyType == typeof (bool?))
                    {
                        SetBooleanPropertyValue(propertyInfo, obj, value);
                    }

                    else if (propertyInfo.PropertyType == typeof (DateTime?))
                    {
                        if ((DateTime?) value == DateTime.MinValue)
                        {
                            value = null;
                        }

                        propertyInfo.SetValue(obj, value, null);
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        SetEnumPropertyValue(propertyInfo, obj, value);
                    }
                    else if (propertyInfo.PropertyType == typeof(Guid))
                    {
                        var temp = Guid.Parse(value.ToString());

                        propertyInfo.SetValue(obj, temp, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, value, null);
                    }
                }
            }
            catch (Exception e)
            {
                throw new EasylinkException(
                    "OR mapping errror: Cannot set property value via reflection. Object: {0}, Property:{1}",
                    obj.GetType().Name, propertyInfo.Name);
            }
        }


        private static void SetInt64PropertyValue<T>(PropertyInfo propertyInfo, T t, object value)
        {
            if (value == null) return;

            propertyInfo.SetValue(t, Int64.Parse(value.ToString()), null);
        }

        private static void SetDecimalPropertyValue<T>(PropertyInfo propertyInfo, T t, object value)
        {
            if (value == null) return;

            propertyInfo.SetValue(t, Decimal.Parse(value.ToString()), null);
        }


        private static void SetInt32PropertyValue<T>(PropertyInfo propertyInfo, T t, object value)
        {
            if (value == null) return;

            propertyInfo.SetValue(t, Int32.Parse(value.ToString()), null);
        }


        private static void SetBooleanPropertyValue<T>(PropertyInfo propertyInfo, T t, object value)
        {
            if (value is bool)
            {
                propertyInfo.SetValue(t, value, null);

                return;
            }

            //oracle 
            string temp = value.ToString();

            if (temp.ToUpper() == "Y")
            {
                propertyInfo.SetValue(t, true, null);
            }
            else if (temp.ToUpper() == "N")
            {
                propertyInfo.SetValue(t, false, null);
            }

            //mysql 
            if (temp == "1")
            {
                propertyInfo.SetValue(t, true, null);
            }
            else if (temp == "0")
            {
                propertyInfo.SetValue(t, false, null);
            }
        }


        private static void SetEnumPropertyValue<T>(PropertyInfo propertyInfo, T t, object value)
        {
            Type enumType = propertyInfo.PropertyType;

            object propertyValue = Enum.Parse(enumType, value.ToString());

            propertyInfo.SetValue(t, propertyValue, null);
        }
    }
}