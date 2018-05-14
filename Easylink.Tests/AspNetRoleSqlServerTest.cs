using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class AspNetRoleSqlServerTest : AspNetRoleTest 
    {
        [TestInitialize]
        public  void Test_Initialization()
        {
            var dbConfig = new DbConfig()
            {
                ConnectionString = @"Server=meng\sqlexpress;Database=easylink;User Id=easylink; Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);


            database = DatabaseFactory.Create();


            Mapping.SetNextId<AspNetRole>(NextIdOption.None);
          
 
        }


        [TestMethod]
        public void sql_server_aspnetRole_should_be_able_to_insert()
        {
            aspnetRole_should_be_able_to_insert();
        }


        [TestMethod]
        public void sql_server_aspnetRole_should_be_able_to_delete()
        {
            aspnetRole_should_be_able_to_delete();
        }


        [TestMethod]
        public void sql_server_aspnetRole_should_be_able_to_update()
        {
            aspnetRole_should_be_able_to_update();
        } 
 
        
        
    }
}