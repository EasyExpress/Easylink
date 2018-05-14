using System;



using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{

    //the reason why adding the test for invoice:
    //a.  test CRUD  with object id property type as int, instead of int64. 
    //b.  test insert invoice with next ID generated from sequence. (sql serveer 2012 introduce sequence)
  
    [TestClass]
    public abstract class InvoiceTest
    {
        protected IDatabase database;

        public void invoice_should_be_able_to_insert()
        {
            Action procedure = () =>
                {
                    var invoice = SharedCode.InstallInvoice(database);

                    var invoiceRetrieved = database.RetrieveObject<Invoice>(i => i.Id == invoice.Id);

                    Assert.IsTrue(invoiceRetrieved!=null);
  

                };


            database.ExecuteInTest(procedure);
        }

     

        public void invoice_should_be_able_to_audit()
        {
            Action procedure = () =>
            {
                //arrange
                var  invoice = SharedCode.InstallInvoice(database);

                var invoiceId = invoice.Id; 

                var  invoiceRetrieved = database.RetrieveObject<Invoice>(i=>i.Id == invoice.Id);

                invoiceRetrieved.InvoiceNumber = "NewInvoiceNumber";
 
                database.Update(invoiceRetrieved);

                database.Delete(invoiceRetrieved);
                 
                //assert
                string recordId = invoiceId.ToString();
                string tableName = "Invoice";

                var  auditRecords =
                    database.RetrieveAll<AuditRecord>(a => a.TableName == tableName && a.RecordId == recordId);

                Assert.IsTrue(auditRecords.Count == 3);

                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Insert") != null);
                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Update") != null);
                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Delete") != null);
            };


            database.ExecuteInTest(procedure);
        }


    }
}

 