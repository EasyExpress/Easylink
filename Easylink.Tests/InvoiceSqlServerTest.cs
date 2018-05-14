using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class InvoiceSqlServerTest : InvoiceTest
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

            Mapping.SetNextId<Invoice>(NextIdOption.Sequence, "Invoice_Id_Seq");
            Mapping.SetNextId<AuditRecord>(NextIdOption.AutoIncrement);


        }


    

        [TestMethod]
        public void sql_server_invoice_should_be_able_to_insert()
        {
            invoice_should_be_able_to_insert();
        } 

        [TestMethod]
        public void sql_server_invoice_should_be_able_to_audit()
        {
            invoice_should_be_able_to_audit();
        } 

        
        
    }
}