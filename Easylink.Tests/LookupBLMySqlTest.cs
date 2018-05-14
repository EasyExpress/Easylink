
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class LookupBLMySqlTest : LookupBLTest
    {
        [TestInitialize]
        public void Test_Initialization()
        {

		 
			
            var dbConfig = new DbConfig()
            {
                ConnectionString = "Server=127.0.0.1;Port=3306; Uid=root; Pwd=1197344; Database=test;",
                DatabaseType =  DatabaseType.MySql, 
                SchemaName = "test",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

 
            Mapping.SetNextId<Lookup>(NextIdOption.AutoIncrement);
 


        }



        [TestMethod]
        public void my_sql_lookup_should_be_able_to_insert()
        {
            lookup_should_be_able_to_insert();
        }
 

        
    }
}