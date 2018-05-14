using System;
 
using System.Linq;
using System.Management;
using System.Reflection;
 

namespace Easylink
{
    public   static class Shared
    {
       
       internal static bool IsAuditable(object obj)
       {

           var classConfig =  ClassConfigContainer.FindClassConfig2(obj);

           return classConfig.Auditable; 

           
        }


        internal static bool IsAuditable<T>( ) where T: new()
        {

            var classConfig = ClassConfigContainer.FindClassConfig(typeof(T));

            return classConfig.Auditable; 

 
        }


        internal static object FindPropertyParent(string propertyPath, object t)
        {
            string[] temp = propertyPath.Split(new[] { '.' });

            if (temp.Length > 1)
            {
                PropertyInfo property = t.GetType().GetProperty(temp[0]);

                if (property == null)
                {
                    throw new EasylinkException("OR mapping errror: Property {0}  is not found in type {1}",
                                                temp[0], t.GetType().Name);
                }

                object parent = property.GetValue(t, null);

                if (parent == null)
                {
                    parent = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(t, parent, null);
                }

                temp = RemoveAt(temp, 0);

                return FindPropertyParent(string.Join(".", temp), parent);
            }

            return t;
        }

        private static string[] RemoveAt(string[] source, int index)
        {
            var dest = new string[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }


        internal static PropertyInfo FindProperty(string propertyPath, object obj)
        {
            string[] temp = propertyPath.Split(new[] {'.'});

            string propertyName = temp[temp.Length - 1];

            PropertyInfo found = obj.GetType().GetProperty(propertyName);

            if (found == null)
            {
                throw new EasylinkException(
                    "Error occurred when creating parameters..   property {0} not found in type {1}.", propertyName,
                    obj.GetType().Name);
            }

            return found;
        }




        internal static PropertyInfo FindProperty1(string propertyPath, Type objectType)
        {


            if (propertyPath.Contains("."))
            {

                var temp = propertyPath.Split(new[] {'.'}).ToList();

                var  propertyName = temp[0];

                temp.RemoveAt(0);

                propertyPath = string.Join(".", temp);

                var propertyInfo = objectType.GetProperty(propertyName);

                return FindProperty1(propertyPath, propertyInfo.PropertyType);
            }
            
          
            var  found = objectType.GetProperty(propertyPath);

            if (found == null)
            {
                throw new EasylinkException(
                    "Error occurred when creating parameters..   property {0} not found in type {1}.", propertyPath,
                    objectType.Name);
            }

            return found;
        }



        internal static object FindPropertyValue(string propertyPath, object obj)
        {
            if (!propertyPath.Contains("."))
            {
                PropertyInfo propertyInfo1 = obj.GetType().GetProperty(propertyPath);

                return propertyInfo1.GetValue(obj, null);
            }

            var temp = propertyPath.Split(new[] {'.'}).ToList();

            string propertyName = temp[0];

            temp.RemoveAt(0);

            var propertyInfo2 = obj.GetType().GetProperty(propertyName);

            obj = propertyInfo2.GetValue(obj, null);

            if (obj == null)
            {
                throw new EasylinkException("property value  is null. Object: {0},  Property name: {1}.", obj,
                                            propertyName);
            }


            propertyPath = string.Join(".", temp);

            return FindPropertyValue(propertyPath, obj);
        }


       public static string GetHardwareId()
       {

           return GetCPUInfo(); 

       }


       private static string GetCPUInfo()
       {
           var cpuInfo = string.Empty;
           var  mc = new ManagementClass("win32_processor");
           var  moc = mc.GetInstances();

           foreach (ManagementObject mo in moc)
           {
               if (cpuInfo == "")
               {
                   
                   cpuInfo = mo.Properties["processorID"].Value.ToString();
                   break;
               }
           }
           return cpuInfo;

       }


      
    }
}