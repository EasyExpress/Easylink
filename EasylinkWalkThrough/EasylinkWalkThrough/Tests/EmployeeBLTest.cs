using System;
 
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
 
namespace EasylinkWalkThrough.Tests
{ 
    [TestClass]
    public class EmployeeBLTest :BaseTest 
    {     
        [TestMethod]
        public void employee_should_insert_update_delete()
        {
            Action proc = () =>
                {
                    //insert employee
                    var employee = FakeEmployee.GetFakeEmployee();
                    database.Insert(employee);

                    //retrieve
                    var employeeInserted = database.RetrieveObject<Employee>(e=>e.Id== employee.Id);
                    Assert.IsTrue(employeeInserted.LoginId == employee.LoginId);

                    //update
                    employeeInserted.Active = false;
                    database.Update(employeeInserted);

                    var employeeUpdated = database.RetrieveObject<Employee>(e => e.Id== employee.Id);
                    Assert.IsTrue(employeeUpdated.Active == false);

                    //delete
                    database.Delete(employeeUpdated);
                    var employeeDeleted = database.RetrieveObject<Employee>(e => e.Id== employee.Id);
                    Assert.IsTrue(employeeDeleted == null);
                };

            database.ExecuteInTest(proc);
        }


        [TestMethod]
        public void should_able_to_get_role_name_using_link()
        {
            Action proc = () =>
            {
                //insert employee
                var employee = FakeEmployee.GetFakeEmployee();
                database.Insert(employee);

                //retrieve
                var employeeInserted = database.RetrieveObject<Employee>(e=>e.Id== employee.Id);
                Assert.IsTrue(employeeInserted.Role.Name == "Employee");

            };

            database.ExecuteInTest(proc);
        }
    }
}
