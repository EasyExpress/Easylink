using System;
using System.Collections.Generic;
using System.Linq;
 
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    [TestClass]
    public abstract class SearchTest
    {
        protected IDatabase database;



        public void employee_search_by_equal_id()
        {
            Action procedure = () =>
                {
                   var employee = SharedCode.InstallEmployee(database);

                   var  employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Id == employee.Id);

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }



        public void employee_search_all()
        {
         
            Action procedure = () =>
            {
                var employee = SharedCode.InstallEmployee(database);

                var all = database.RetrieveAll<Employee>();

                 Assert.IsTrue(all.Count == 1);

                 Assert.IsTrue(all[0].FirstName == employee.FirstName);

                 Assert.IsTrue(all[0].LastName == employee.LastName);
            };


            database.ExecuteInTest(procedure);
        }
        public void employee_search_by_multiple_conditions()
        {
            Action procedure = () =>
            {
                var employee = SharedCode.InstallEmployee(database);

                var employeeRetrieved1 =
                    database.RetrieveObject<Employee>(e => e.FirstName == employee.FirstName && e.Salary == employee.Salary);

                Assert.IsNotNull(employeeRetrieved1);

                var employeeRetrieved2 =
                    database.RetrieveObject<Employee>(e => e.EmployedSince == employee.EmployedSince,
                                                      e => e.Age == employee.Age);

                Assert.IsNotNull(employeeRetrieved2);


                var employeeRetrieved3 =
                    database.RetrieveObject<Employee>(
                        e => e.Department.Name == "IT" || e.FirstName == employee.FirstName + "aa");

                Assert.IsNotNull(employeeRetrieved3);



                var employeeRetrieved4 =
               database.RetrieveObject<Employee>(e => e.EmployedSince == employee.EmployedSince && e.Active == employee.Active,
                                                 e => e.Age == employee.Age);

                Assert.IsNotNull(employeeRetrieved4);


            };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_equal_identifier()
        {
            Action procedure = () =>
            {
                Employee employee = SharedCode.InstallEmployee(database);

                var employeeRetrieved =
                    database.RetrieveObject<Employee>(e => e.Identifier == employee.Identifier);

                Assert.IsNotNull(employeeRetrieved);
            };

            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_not_equal_id()
        {
            Action procedure = () =>
                {
                    SharedCode.InstallEmployee(database);

                    var employeeRetrieved = database.RetrieveObject<Employee>(e => e.Id != 0);

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_equal_date()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince == employee.EmployedSince);


                    Assert.IsNotNull(employeeRetrieved);

                    var employedSince = employee.EmployedSince;

                    var employeeRetrieved1 =
                      database.RetrieveObject<Employee>(e => e.EmployedSince == employedSince);
 
                    Assert.IsNotNull(employeeRetrieved1);


                  
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_not_equal_date()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince != employee.EmployedSince.Value.AddDays(1));


                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_greater_date()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince >= employee.EmployedSince);

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince >  employee.EmployedSince);

                    Assert.IsNull(employeeRetrieved);


                    employeeRetrieved= database.RetrieveObject<Employee>(e => e.EmployedSince > new DateTime(1928,12,19));

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_less_date()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince <= employee.EmployedSince);

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.EmployedSince < employee.EmployedSince);

                    Assert.IsNull(employeeRetrieved);


                    var employeeRetrieved2 = database.RetrieveObject<Employee>(e => e.EmployedSince < DateTime.Now);

                    Assert.IsNotNull(employeeRetrieved2);


                 
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_equal_bool()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Active ==  employee.Active);


                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_equal_decimal()
        {
            Action procedure = () =>
            {
                Employee employee = SharedCode.InstallEmployee(database);

                var employeeRetrieved =
                    database.RetrieveObject<Employee>(e => e.Salary == employee.Salary);


                Assert.IsNotNull(employeeRetrieved);
            };


            database.ExecuteInTest(procedure);
        }



        public void employee_search_by_not_equal_bool()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Active != !employee.Active);


                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


      


        public void employee_search_by_not_equal_int()
        {
            Action procedure = () =>
            {
                Employee employee = SharedCode.InstallEmployee(database);

                var employeeRetrieved =
                    database.RetrieveObject<Employee>(e => e.Age  != employee.Age +5);


                Assert.IsNotNull(employeeRetrieved);
            };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_not_equal_decimal()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Salary != employee.Salary + 0.001M);


                    Assert.IsNotNull(employeeRetrieved);


                    var temp = employee.Salary + 0.001M;

                    employeeRetrieved =
                     database.RetrieveObject<Employee>(e => e.Salary != temp);


                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_greater_decimal()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Salary >= employee.Salary);


                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Salary > employee.Salary);

                    Assert.IsNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_less_decimal()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Salary <= employee.Salary);


                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Salary < employee.Salary);

                    Assert.IsNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_equal_text()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName == employee.FirstName);

                    Assert.IsNotNull(employeeRetrieved);

                    //case sensitive
                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName == "DAvid".CaseSensitive());

              
                    Assert.IsNull(employeeRetrieved);

                    //case is not sensitive
                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName == "DAvid");


                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_not_equal_text()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName != employee.FirstName + "a");

                    Assert.IsNotNull(employeeRetrieved);

                    var text = "a";
                    var employeeRetrieved1 =  database.RetrieveObject<Employee>(e => e.FirstName != employee.FirstName + text);

                    Assert.IsNotNull(employeeRetrieved1);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_starts_with_text()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.StartsWith(employee.FirstName.Substring(0, 1)));

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_endswith_text()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.EndsWith(
                                                                       employee.FirstName.Substring(
                                                                           employee.FirstName.Length - 1, 1)));

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_contains_text()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.Contains(employee.FirstName));

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.Contains(employee.FirstName.Substring(1,
                                                                                                    employee.FirstName
                                                                                                            .Length - 2)));

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_contains_text_case_sensitive()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.Contains(employee.FirstName));

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.FirstName.Contains(employee.FirstName.ToUpper().CaseSensitive()));

                    Assert.IsNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_in_list()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => Condition.In(e.Id, new List<Int64> {employee.Id}));

                    Assert.IsNotNull(employeeRetrieved);


                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => Condition.In( e.Salary, new List<decimal> {employee.Salary}));
                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e =>Condition.In( e.FirstName,  new List<string> {employee.FirstName}));
                    Assert.IsNotNull(employeeRetrieved);


                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => Condition.In(e.EmployedSince, new List<DateTime> {employee.EmployedSince.Value}));

                    Assert.IsNotNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_search_by_role_name()
        {
            Action procedure = () =>
                {
                    SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Role.Name == "Employee");

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Role.Name == "Manager");

                    Assert.IsNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_search_by_country()
        {
            Action procedure = () =>
                {
                    SharedCode.InstallEmployee(database);

                    var employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Address.Country.Name == "China");

                    Assert.IsNotNull(employeeRetrieved);

                    employeeRetrieved =
                        database.RetrieveObject<Employee>(e => e.Address.Country.Name == "Canada");

                    Assert.IsNull(employeeRetrieved);
                };


            database.ExecuteInTest(procedure);
        }


 
    }
}