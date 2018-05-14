using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace EasylinkWalkThrough.Tests
{ 
    [TestClass]
    public class ValidationTest :BaseTest
    {     
        [TestMethod]
        public void employee_validation_login_id_is_required()
        {
            var employee = FakeEmployee.GetFakeEmployee();

            employee.LoginId = string.Empty;

            var rules = employee.ValidateRules();

            Assert.IsTrue(rules.Count == 1);

            Assert.IsTrue(rules[0].ErrorMessage == "Employee: Login Id is required.");

        }

        [TestMethod]
        public void employee_validation_login_id_must_be_unique()
        {
            Action proc = () =>
            {         
                var employee = FakeEmployee.GetFakeEmployee();
                database.Insert(employee);

                var employee1 = FakeEmployee.GetFakeEmployee();

                var rules =  new EmployeeBL(database).ValidateEmployee(employee1);

                 Assert.IsTrue(rules.Count == 1);

                 Assert.IsTrue(rules[0].ErrorMessage == "Employee: Login id already exists!");

            };

            database.ExecuteInTest(proc);

        }
    }
}
