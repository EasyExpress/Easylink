
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Easylink
{
    public class DatabaseFactory
    {

  
        public static IDatabase Create()
        {

            if (DatabaseSetup.Instance == null)
            {
                InitializeDatabaseFactory(); 
            }

            return DatabaseCreator.CreateDatabase();
        }


        public static  void Initialize(DbConfig dbConfig)
        {
            DatabaseSetup.Initialize(dbConfig);

        }



        private static void InitializeDatabaseFactory()
        {
             var dbConfig  = CreateDbConfig();

            DatabaseSetup.Initialize(dbConfig);

        }


        private static DbConfig CreateDbConfig()
        {
            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

            if (!Directory.Exists(binPath))
            {
                binPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            foreach (var dllFile in Directory.GetFiles(binPath, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe")))
            {

                var assembly = FindLoadedAssembly(dllFile) ?? Assembly.LoadFrom(dllFile);
      
                try
                {
                   Type[] types = assembly.GetTypes();

                    var found = types.Where(IsDbConfigCreatorType);

                    if (found.Count() != 0)
                    {
                        var dbConfigCreator = Activator.CreateInstance(found.First()) as IDbConfigCreator;

                        return dbConfigCreator.Create();

                    }
                }
                catch
                {
                   
                }
            }

            throw new EasylinkException("Please implement IDbConfigCreator class.");


        }

        private static bool IsDbConfigCreatorType(Type toCheck)
        {
            var dbConfigCreator = typeof(IDbConfigCreator);

            return toCheck != dbConfigCreator && dbConfigCreator.IsAssignableFrom(toCheck);
        }

    

        private static Assembly FindLoadedAssembly(string dllFile)
        {
            var  assemblyName = Path.GetFileName(dllFile).Replace(".dll", "");

            return AppDomain.CurrentDomain.GetAssemblies()
                            .ToList()
                            .Find(a => a.GetName().Name == assemblyName);

        }

       
     
    }
}