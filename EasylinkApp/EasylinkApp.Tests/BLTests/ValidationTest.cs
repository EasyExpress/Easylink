 
using System;
using Easylink;
using EasylinkApp.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EasylinkApp.Tests
{

    [TestClass]
    public class ValidationTest
    {

        protected IDatabase database;
 
        [TestInitialize]
        public void Test_Initialization()
        {

            var dbConfig = new DbConfig()
            {
                ConnectionString = @"Server=meng\sqlexpress;Database=EasylinkApp;User Id=easylink;Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();


        }

       

     


        [TestMethod]
        public void employee_can_not_have_two_identical_programs()
        {

            var employee = FakeEmployee.GetFakeEmployee(database);

            employee.Programs.Add(new EmployeeProgram { ProgramId = employee.Programs[1].ProgramId, Status = ProgramStatus.Completed });

            var rules = employee.ValidateRules();

            Assert.IsTrue(rules.Count == 1);

            Assert.IsTrue(rules[0].ErrorMessage == "Employee: Two or more programs are same.");


        }

    }
}
