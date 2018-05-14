
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public  class SqlTraceSqlServerTest :SqlTraceTest
    {
        [TestInitialize]
        public void Test_Initialization()
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


            Mapping.SetNextId<Lookup>(NextIdOption.AutoIncrement);
         

        }


        [TestMethod]
        public void sql_server_should_be_able_to_trace_sql_without_transaction()
        {
            should_be_able_to_trace_sql_without_transaction();
        }



        [TestMethod]
        public void sql_server_should_be_able_to_trace_sql_in_transaction()
        {
            should_be_able_to_trace_sql_in_transaction();
        }
 
    }
}