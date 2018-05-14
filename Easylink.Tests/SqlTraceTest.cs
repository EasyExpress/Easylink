using System;

using Easylink.Tests.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public abstract class SqlTraceTest
    {
        protected IDatabase database;

        public void  should_be_able_to_trace_sql_without_transaction()
        {
            
               new LookupBL(database).RetrieveLookupByName("Role");

               Assert.IsTrue(database.Sqls.Count == 1);

               new LookupBL(database).RetrieveLookupByName("Role");

               Assert.IsTrue(database.Sqls.Count == 2);

              
        }

        public void should_be_able_to_trace_sql_in_transaction()
        {

            Action procedure = () =>
            {
                new LookupBL(database).RetrieveLookupByName("Role");

                Assert.IsTrue(database.Sqls.Count == 1);

                new LookupBL(database).RetrieveLookupByName("Role");

                Assert.IsTrue(database.Sqls.Count == 2);
            };


            database.ExecuteInTest(procedure);

        }
    
    }
}