using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Easylink;
using Easylink.Tests;
using Easylink.Tests.Model;
using System.Xml.Serialization;
using System.IO;


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
                ConnectionString = @"Server=meng\sqlexpress;Database=easylink;User Id=easylink; Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();
 
        }


        [TestMethod]
        public void employee_salary_must_be_more_than_10000()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.Salary = 9999.99M;

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 2);
        }


        [TestMethod]
        public void employee_first_name_containsNoSpace()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.FirstName = "MA RK";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken[0].ScreenName == "First Name");

            Assert.IsTrue(broken[0].ErrorMessage == "Employee: First Name cannot contain spaces.");

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_last_name_containsNoSpace()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.LastName = "MA RK";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken[0].ScreenName == "LastName");

            Assert.IsTrue(broken[0].ErrorMessage == "Employee: LastName cannot contain spaces.");

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_firstname_can_not_be_more_than_20_letters()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.FirstName = "davidscheulferzebufsseess";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_email_should_be_valid()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.Email = "dag@ttsv";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_social_insurance_number_must_be_9()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.SocialInsuranceNumber = "12345678";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken[0].ScreenName == "Social Insurance Number");

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_social_insurance_number_should_be_numeric()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.SocialInsuranceNumber = "ASDEfgHJW";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_phone_should_be_valid()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.Phone = "(403)123-2a12";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 1);
        }


        [TestMethod]
        public void employee_address_postalcode_should_be_valid()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.Address.PostalCode = "RRR 234";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken[0].ScreenName == "Postal Code");


            Assert.IsTrue(broken.Count == 1);
        }

      

       
        [TestMethod]
        public void employee_should_not_have_two_identical_programs()
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            List<Rule> broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.Programs.Add(new EmployeeProgram
                {
                    ProgramId = employee.Programs[1].ProgramId,
                    Status = ProgramStatus.Completed
                });

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 1);

            Assert.IsTrue(broken[0].ErrorMessage == "Employee: Two or more programs are same.");
        }



        [TestMethod]
        public void employee_salary_should_be_within_a_range()
        {

            var employee = FakeEmployee.GetFakeEmployee(database);

            employee.Salary = 120000.00m;

            var rules = employee.ValidateRules();

            Assert.IsTrue(rules.Count == 1);

            Assert.IsTrue(rules[0].ErrorMessage == "Employee: Salary value should between 10000 and 90000");


        }


        [TestMethod]
        public void employee_birth_date_should_be_within_a_range()
        {

            var employee = FakeEmployee.GetFakeEmployee(database);

            employee.EmployedSince  = new DateTime(1905, 12, 3);

            var rules = employee.ValidateRules();

            Assert.IsTrue(rules.Count == 1);

            Assert.IsTrue(rules[0].ErrorMessage == "Employee: Employeed Since value should between 12/31/1915 and  12/31/2015");


        }


        [TestMethod]
        public void employee_first_name_word_count_no_more_than_two()
        {

            var employee = FakeEmployee.GetFakeEmployee(database);

            var broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

            employee.FirstName = "1 2 3";

            broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count ==2);

            Assert.IsTrue(broken[1].ErrorMessage == "Employee: too many words in First Name.");


        }



        [TestMethod]
        public void employee_validation_dynamic_should_be_empty()
        {
            var employee = FakeEmployee.GetFakeEmployee(database);

            Assert.IsTrue(employee.Validation != null);

        }

        [TestMethod]
        public void employee_validation_dynamic()
        {
            var  employee = FakeEmployee.GetFakeEmployee(database);

            var  broken = employee.ValidateRules();

            Assert.IsTrue(broken.Count == 0);

             employee.LastName = "MA RK";

             employee.ValidateRules();

             Assert.IsTrue(employee.Validation.LastName == "Employee: LastName cannot contain spaces.");

             Assert.IsTrue(employee.Validation.FirstName == null);
        }

 

        [TestMethod]
        public void program_id_is_required()
        {
            Program program = new Program
            {
                Id = -1,
                Cost = 200,
                Name = "Math"
            };

            List<Rule> broken = program.ValidateRules();

            Assert.IsTrue(broken.Count ==1);

            program.Id= 0;

            broken = program.ValidateRules();

            Assert.IsTrue(broken.Count ==0);

           
        }

    }
}