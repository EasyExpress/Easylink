using System;
 
using Easylink;
 
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EasylinkWalkThrough.Tests
{ 
    [TestClass]
    public class NestedTransactionTest :BaseTest 
    {
        private void InsertEmployee(Employee employee, IDatabase database)
        {

            Action proc = () =>
            {
                database.Insert(employee);

            };

            database.ExecuteInTransaction(proc);
        }
        [TestMethod]
        public void transaction_roll_back_at_root_level()
        {  
            var employee = FakeEmployee.GetFakeEmployee();
            try
            {
                Action proc = () =>
                    {
                        InsertEmployee(employee, database);
                        var employeeInserted =
                            database.RetrieveObject<Employee>(e=>e.LoginId== employee.LoginId);
                        Assert.IsTrue(employeeInserted != null);
                        InsertEmployee(null, database);   
                    };

                database.ExecuteInTransaction(proc);
            }
            catch (Exception ex)
            {
            }
            var employeeRetrieved = database.RetrieveObject<Employee>(e => e.LoginId==employee.LoginId);
            Assert.IsTrue(employeeRetrieved == null);
        }
    }
}
