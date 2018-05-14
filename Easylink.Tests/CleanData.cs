
using System;
using Easylink.Tests.Model;


namespace Easylink.Tests
{
    
    //[TestClass]
    public class CleanData  
    {
       
        public void CleanAll()
        {
            
            IDatabase database;

            //my sql 
            var dbConfig = new DbConfig()
            {
                ConnectionString = "Server=127.0.0.1;Port=3306; Uid=root; Pwd=1197344; Database=test;",
                DatabaseType = DatabaseType.MySql,
                SchemaName = "test",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

            Clean(database);

            //sql server 
            dbConfig = new DbConfig()
            {
                ConnectionString = @"Server=meng\sqlexpress;Database=easylink;User Id=easylink; Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

            Clean(database);


            //oracle 
            dbConfig = new DbConfig()
            {
                ConnectionString =
                    string.Format("Data Source= " +
                                  "(DESCRIPTION =" +
                                  "(ADDRESS = (PROTOCOL = TCP)(HOST = {0} )(PORT = {1}))" +
                                  "(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};",
                                  "localhost", "1521", "xe", "easylink", "1197344"),
                DatabaseType = DatabaseType.Oracle,
                SchemaName = "EASYLINK",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

         

            database = DatabaseFactory.Create();

            Clean(database);

            //postgresql 

            dbConfig = new DbConfig()
            {
                ConnectionString = "Server=localhost;Port=5432;Database=Easylink;Pooling =false; User Id=postgres;Password=1197344;",
                DatabaseType = DatabaseType.PostgreSql,
                SchemaName = "public",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

            Clean(database);


        }


 


        private  void Clean(IDatabase database)
        {
            Action procedure = () =>
            {
                database.DeleteAll<Address>();
                database.DeleteAll<EmployeeProgram>();
                database.DeleteAll<Employee>();
                database.DeleteAll<AuditRecord>();

                database.DeleteAll<Lookup>(l => l.Name == "Temporary");
            };


            database.ExecuteInTransaction(procedure);

        }
        
    }
}