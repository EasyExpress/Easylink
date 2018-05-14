using System;
using System.Collections.Generic;
 
namespace Easylink
{
    internal  static class ClassConfigContainer
    {

        private static List<ClassConfig> _classConfigs;

        private   static List<ClassConfig> ClassConfigs
        {
            get
            {
                if (_classConfigs == null)
                {
                    _classConfigs = ClassConfigLoader.LoadClassConfigs();
                }

                return _classConfigs;
            }
        }

    

        internal static string FindSelectSql<T>()
        {
            return FindClassConfig1<T>().SelectSql;
        }


        internal static string FindSelectSql(Type objectType)
        {
            return FindClassConfig(objectType).SelectSql;
        }

        internal static string FindInsertSql(Type objectType)
        {

            var classConfig = FindClassConfig(objectType);

            var idPropertyName = classConfig.IdPropertyName;

            var propertyInfo = objectType.GetProperty(idPropertyName);

            if (propertyInfo == null)
            {
                throw new EasylinkException(
                    "Error occurred when finding insert sql. {0}   has invalid id property {1}.", objectType.Name,
                    idPropertyName);

            }

            if (classConfig.NextIdOption == NextIdOption.AutoIncrement)
            {
                return classConfig.InsertSql0;
            }

            return classConfig.InsertSql;
            
        }


        internal static string FindUpdateSql1(Type objectType)
        {
            var  found = FindClassConfig(objectType);
            return String.Format("{0} {1}", found.UpdateSql, found.WhereSql);
        }


        internal static string FindDeleteSql1(Type objectType)
        {
            var  found = FindClassConfig(objectType);
            return String.Format("{0} {1}", found.DeleteSql, found.WhereSql);
        }


        internal static string FindUpdateSql(Type objectType)
        {
            return FindClassConfig(objectType).UpdateSql;
        }


        internal static string FindDeleteSql(Type objectType)
        {
            return FindClassConfig(objectType).DeleteSql;
        }

        internal static string FindWhereSql<T>()
        {
            return FindClassConfig1<T>().WhereSql;
        }

    


        internal static string FindIdPropertyNamer(Type objectType)
        {
            return FindClassConfig(objectType).IdPropertyName;
        }


        internal static  ClassConfig   FindClassConfig2(object obj)
        {
            return FindClassConfig(obj.GetType());

        }


        internal static ClassConfig FindClassConfig1<T>()
        {
            return FindClassConfig(typeof(T));
        }

        internal  static ClassConfig FindClassConfig(Type type)
        {
  
            if (type == null)
            {
                throw new EasylinkException("Error occurred when finding class config: type is null.");

            }
            var found = ClassConfigs.Find(c => c.Type == type);

            if (found == null)
            {
                throw new EasylinkException("Type {0} mapping is not found.", type.Name);
            }

            return found; 

        }

      
    }
}