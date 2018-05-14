using System;
using System.Collections.Generic;
using System.Reflection;

namespace Easylink
{
    internal static class Auditor
    {
        internal static string AuditInsert(object obj, Dictionary<string, object> propertyParameters)
        {
            return CreateInsertAuditText(obj, propertyParameters);
        }


        internal static string AuditDelete(object obj, Dictionary<string, object> propertyParameters)
        {
            return CreateDeleteAuditText(obj, propertyParameters);
        }


        internal static string AuditUpdate(object obj, Dictionary<string, string> changes)
        {
            return CreateUpdateAuditText(obj, changes);
        }
        
        internal static object CreateAuditRecord(object obj, DbOperation operation, string auditText)
        {

            var auditRecordBase = CreateAuditRecordBase(obj, operation, auditText);

            var databaseSetup = DatabaseSetup.Instance;

            if (databaseSetup.AuditRecordType == null)
            {
                throw new EasylinkException("Can not create audit record because audit record type is not set.");

            }

            if (!typeof(AuditBase).IsAssignableFrom(databaseSetup.AuditRecordType))
            {
                throw new EasylinkException("{0} is not  inherited  from AuditRecordBase.", databaseSetup.AuditRecordType.FullName);

            }

            var auditRecord = Activator.CreateInstance(databaseSetup.AuditRecordType);


            var baseProperties = auditRecordBase.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var baseProperty in baseProperties)
            {

                var basePropertyValue = baseProperty.GetValue(auditRecordBase, null);

                var property = databaseSetup.AuditRecordType.GetProperty(baseProperty.Name);

                property.SetValue(auditRecord, basePropertyValue, null);

            }

            (auditRecord as AuditBase).PrepareBeforeInsert();

            return auditRecord;

        }

        private  static  AuditBase CreateAuditRecordBase(object obj, DbOperation operation, string auditText)
        {
            var auditRecord = new AuditBase
            {
                RecordId = GetRecordId(obj),
                TableName = ClassConfigContainer.FindClassConfig2(obj).TableName,
                Description = auditText,
                TimeStamp = DateTime.Now,
                Operation = operation.ToString(),
                ObjectToAudit = obj
            };


            return auditRecord;
        }

        private static string GetRecordId(object obj)
        {

           var isAuditable  =  Shared.IsAuditable(obj);

            if (isAuditable)
            {

                    var classsConfig = ClassConfigContainer.FindClassConfig2(obj);

                    if (classsConfig == null)
                    {
                        throw new EasylinkException("class {0} mapping file is not found!", obj.GetType().Name);
                    }


                    var  key = classsConfig.IdPropertyName;

                    if (string.IsNullOrEmpty(key))
                    {
                        throw new EasylinkException("class {0} mapping Id property is not set.", obj.GetType().Name);
                    }


                    var  property = obj.GetType().GetProperty(key);

                    var  propertyValue = property.GetValue(obj, null);

                    return propertyValue.ToString();
                }
            

            throw new EasylinkException("Record key is not set.");
        }




        private static string CreateInsertAuditText(object obj, Dictionary<string, object> propertyParameters)
        {
            List<string> temp = CreateColumnTexts(obj, propertyParameters);

            return "Inserted " + string.Join(", ", temp) + ".";
        }


        private static string CreateDeleteAuditText(object obj, Dictionary<string, object> propertyParameters)
        {
            List<string> temp = CreateColumnTexts(obj, propertyParameters);

            return "Deleted " + string.Join(", ", temp) + ".";
        }


        private static string CreateUpdateAuditText(object obj, Dictionary<string, string> changes)
        {
            var temp = new List<string>();


            foreach (var  key in changes.Keys)
            {
                var  classConfig = ClassConfigContainer.FindClassConfig2(obj);

                PropertyConfig propertyConfig = classConfig.GetPropertyConfig(key);

                if (propertyConfig == null)
                {
                    throw new EasylinkException(
                            "Error occurred when create update audit text. property name {0}  is not defined in the {1} mapping.",
                            key, obj.GetType().Name);
                }

                string item = string.Format("{0} {1}", propertyConfig.ColumnName, changes[key]);

                temp.Add(item);
            }

            return "Updated " + string.Join(", ", temp) + ".";
        }


        private static List<string> CreateColumnTexts(object obj, Dictionary<string, object> parameters)
        {
            var temp = new List<string>();


            foreach (var key in parameters.Keys)
            {
                var  classConfig = ClassConfigContainer.FindClassConfig2(obj);

                PropertyConfig propertyConfig = classConfig.GetPropertyConfig(key);


                if (propertyConfig == null)
                {
                    throw new EasylinkException(
                            "Error occurred when create create column texts. property name {0}  is not defined in the {1} mapping.",
                            key, obj.GetType().Name);
                }

                string item = string.Format("{0}={1}", propertyConfig.ColumnName, parameters[key]);

                temp.Add(item);
            }
            return temp;
        }
    }
}