 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Easylink
{
    internal static class ClassConfigLoader
    {
        internal  static List<ClassConfig> LoadClassConfigs()
        {
 
            var mappingTypes = LoadMappingTypes();
          
            var mappingConfigs = new List<IMappingConfig>();

            mappingTypes.ForEach(
                m =>
                {
                    var mapping = Activator.CreateInstance(m) as Mapping;

                    mappingConfigs.Add(mapping.Setup());

                }
                );

           return  SetupClassConfigs(mappingConfigs);

        }

        private static List<ClassConfig> SetupClassConfigs(List<IMappingConfig> mappingConfigs)
        {
            var classConfigs = new List<ClassConfig>();

            foreach (var m in mappingConfigs)
            {
                var classConfig = new ClassConfig(m);

                var parents = FindParentMappingConfigs(classConfig, mappingConfigs);

                classConfig.Inherit(parents);

                classConfigs.Add(classConfig);
            }

            return classConfigs;

        }

        private static List<IMappingConfig> FindParentMappingConfigs(ClassConfig classConfig, List<IMappingConfig> mappingConfigs)
        {
            var paarents = new List<IMappingConfig>();

            foreach (var m in mappingConfigs)
            {
                if (m.Type != classConfig.Type && classConfig.Type.IsSubclassOf(m.Type))
                {
                    paarents.Add(m);
                }
            }

            return paarents;
        }


        private static List<Type> LoadMappingTypes()
        {

            var types = new List<Type>(); 

            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

            if (!Directory.Exists(binPath))
            {
                binPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            foreach (var dllFile in Directory.GetFiles(binPath, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe")))
            {

                var assembly = FindLoadedAssembly(dllFile) ?? Assembly.LoadFrom(dllFile);

                types.AddRange(FindMappingTypes(assembly));

            }


         
            if (types.Count > 0) return types;


            throw new EasylinkException("Mapping assembly can not be found at {0}", binPath);
        }

        private static List<Type> FindMappingTypes(Assembly assembly)
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch
            {
                return new List<Type>();
            }

            Type mappingType = typeof(Mapping);

            return types.Where(t => IsMappingType(mappingType, t)).ToList();

        }


        private static bool IsMappingType(Type mappingType, Type toCheck)
        {

            return toCheck != mappingType && mappingType.IsAssignableFrom(toCheck);
        }


        private static Assembly FindLoadedAssembly(string dllFile)
        {
            string assemblyName = Path.GetFileName(dllFile).Replace(".dll", "");

            return AppDomain.CurrentDomain.GetAssemblies()
                            .ToList()
                            .Find(a => a.GetName().Name == assemblyName);

        }


      
    }
}