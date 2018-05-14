using System;
using System.Collections.Generic;

using Easylink.Tests.Business;
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public abstract class EmployeeBLTest
    {
        protected IDatabase database;


      

     
        public void employee_should_be_able_to_delete_programs_using_criteria()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeRetrieved.Programs.Count == 2);

                    database.DeleteAll<EmployeeProgram>(ep => ep.EmployeeId == employeeRetrieved.Id);


                    Employee employeeRetrieved1 = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeRetrieved1.Programs.Count == 0);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_retrieve_additional_info_if_exists()
        {
            Action procedure = () =>
            {
                Employee employee = SharedCode.InstallEmployee(database);

                Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                Assert.IsTrue(employeeRetrieved.AdditionalInfo.Id == null);

                employeeRetrieved.AdditionalInfo= new AdditionalInfo { Notes="test" };

                new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                var employeeUpdated = new EmployeeBL(database).RetrieveById(employeeRetrieved.Id);

                Assert.IsTrue(employeeUpdated.AdditionalInfo.Id != null);

                employeeUpdated.AdditionalInfo.MarkAsDeleted();

                new EmployeeBL(database).UpdateEmployee(employeeUpdated);

                employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                Assert.IsTrue(employeeRetrieved.AdditionalInfo.Id == null);

            };


            database.ExecuteInTest(procedure);
        }

        public void employee_should_be_able_to_retrieve_financial_info()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeRetrieved.FinancialInfo.Id == null);


                    employeeRetrieved.FinancialInfo = new FinancialInfo {Bank = "TD", EmployeeId = employeeRetrieved.Id};

                    database.Insert(employeeRetrieved.FinancialInfo);


                    Employee employeeRetrieved1 = new EmployeeBL(database).RetrieveById(employee.Id);


                    Assert.IsTrue(employeeRetrieved1.FinancialInfo.Bank == "TD");
                    Assert.IsTrue(employeeRetrieved1.FinancialInfo.Id != null);


                    SharedCode.UninstallEmployee(employeeRetrieved1, database);
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_should_be_able_to_retrieve_addresses()
        {
            Action procedure = () =>
                {
                    var  employee = FakeEmployee.GetFakeEmployee1(database);

                    database.Insert(employee.Address);
                    database.Insert(employee);

                    var  employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);


                    Assert.IsTrue(employeeRetrieved.Address.StreetName == "Centre Street");

                 
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_retrieve_lookups()
        {
            Action procedure = () =>
                {
                    //Act
                    Employee employee = SharedCode.InstallEmployee(database);


                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    //Assert 
                    Assert.IsTrue(employeeRetrieved.FirstName == employee.FirstName);
                    Assert.IsTrue(employeeRetrieved.Salary == employee.Salary);

                    Assert.IsTrue(employeeRetrieved.Sex.Id == 0);

                    Assert.IsTrue(employeeRetrieved.Sex.Name == null);

                    employeeRetrieved.Sex = new LookupBL(database).RetrieveLookupByName("Male");

                    new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                    Employee employeeUpdated = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeUpdated.Sex.Id == employeeRetrieved.Sex.Id);

                    Assert.IsTrue(employeeUpdated.Sex.Name == employeeRetrieved.Sex.Name);


                    SharedCode.UninstallEmployee(employeeUpdated, database);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_retrieve_multiple_lookups()
        {
            Action procedure = () =>
                {
                    //Act
                    var  employee = SharedCode.InstallEmployee(database);
                    var  employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    //Assert 
                    Assert.IsTrue(employeeRetrieved.FirstName == employee.FirstName);
                    Assert.IsTrue(employeeRetrieved.Salary == employee.Salary);

                    Assert.IsTrue(employeeRetrieved.Department.Name == Department.IT.ToString());
                    Assert.IsTrue(employeeRetrieved.Role.Name == Role.Employee.ToString());
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_retrieve_program_name()
        {
            Action procedure = () =>
                {
                    //Act
                    Employee employee = SharedCode.InstallEmployee(database);


                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    //Assert 
                    Assert.IsTrue(employeeRetrieved.Programs.Count == 2);
                    Assert.IsTrue(employeeRetrieved.Programs[0].ProgramName != "");
                };


            database.ExecuteInTest(procedure);
        }

        public void employee_should_be_able_to_insert()
        {
            Action procedure = () =>
                {
                    //Act
                    Employee employee = SharedCode.InstallEmployee(database);

                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    //Assert 
                    Assert.IsTrue(employeeRetrieved.FirstName == employee.FirstName);
                    Assert.IsTrue(employeeRetrieved.Salary == employee.Salary);
                    Assert.IsTrue(employeeRetrieved.Identifier == employee.Identifier);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_update()
        {
            Action procedure = () =>
                {
                    //arrange
                    Employee employee = SharedCode.InstallEmployee(database);


                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);


                    employeeRetrieved.FirstName = "New Name";

                    employeeRetrieved.Department =
                        new LookupBL(database).RetrieveLookupByName(Department.Finance.ToString());


                    employeeRetrieved.Programs[0].MarkAsDeleted();
                    Assert.IsTrue(employeeRetrieved.Programs[1].ProgramId == 3);

                    employeeRetrieved.Programs[1].Status = ProgramStatus.Registered;
                    employeeRetrieved.Programs.Add(new EmployeeProgram {ProgramId = 1, Status = ProgramStatus.Completed});


                    new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                    var  employeeUpdated = new EmployeeBL(database).RetrieveById(employee.Id);

                    //assert 
                    Assert.IsTrue(employeeUpdated.FirstName == "New Name");

                    Assert.IsTrue(employeeUpdated.Department.Name == Department.Finance.ToString());

                    Assert.IsTrue(employeeUpdated.Programs.Count == 2);

                    var  found = employeeUpdated.Programs.Find(p => p.ProgramId == 3);
                    Assert.IsTrue(found != null);
                    Assert.IsTrue(found.Status == ProgramStatus.Registered);

                    found = employeeUpdated.Programs.Find(p => p.ProgramId == 1);
                    Assert.IsTrue(found != null);
                    Assert.IsTrue(found.Status == ProgramStatus.Completed);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_address_should_be_able_to_update()
        {
            Action procedure = () =>
            {
                //arrange
                var  employee = SharedCode.InstallEmployee(database);

                var  employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                employeeRetrieved.Address.StreetName = "New Street";
 
                new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                var employeeUpdated = new EmployeeBL(database).RetrieveById(employee.Id);

                //assert 
                Assert.IsTrue(employeeUpdated.Address.StreetName == employeeRetrieved.Address.StreetName);


                var address = employeeUpdated.Address;

                address.PostalCode = address.PostalCode + "LZ";

                database.Update(address);

                var addressUpdated = database.RetrieveObject<Address>(a => a.Id == address.Id);

                //assert 

                Assert.IsTrue(addressUpdated.PostalCode == address.PostalCode);

            };


            database.ExecuteInTest(procedure);
        }

        public void employee_should_retrieve_and_save_nullable()
        {
            Action procedure = () =>
                {
                    //Act
                    var  employee = SharedCode.InstallEmployee(database);

                    var  employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    //Assert 
                    Assert.IsTrue(employeeRetrieved.Age == 41);
                    Assert.IsTrue(employeeRetrieved.Weight == null);
                    Assert.IsTrue(employeeRetrieved.Married == null);
                    Assert.IsTrue(employeeRetrieved.EmployedSince != null);

                    employeeRetrieved.Age = null;
                    employeeRetrieved.Weight = 145.67m;
                    employeeRetrieved.EmployedSince = null;


                    new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                    Employee employeeSaved = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeSaved.Age == null);
                    Assert.IsTrue(employeeSaved.Weight == 145.67m);
                    Assert.IsTrue(employeeSaved.Married == null);
                    Assert.IsTrue(employeeSaved.EmployedSince == null);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_audit()
        {
            Action procedure = () =>
                {
                    //arrange
                    Employee employee = SharedCode.InstallEmployee(database);

                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    employeeRetrieved.FirstName = "New Name";

                    employeeRetrieved.Department =
                        new LookupBL(database).RetrieveLookupByName(Department.Finance.ToString());


                    employeeRetrieved.Programs[0].MarkAsDeleted();
                    Assert.IsTrue(employeeRetrieved.Programs[1].ProgramId == 3);

                    employeeRetrieved.Programs[1].Status = ProgramStatus.Registered;
                    employeeRetrieved.Programs.Add(new EmployeeProgram {ProgramId = 1, Status = ProgramStatus.Completed});


                    new EmployeeBL(database).UpdateEmployee(employeeRetrieved);


                    SharedCode.UninstallEmployee(employee, database);


                    //assert
                    string recordId = employeeRetrieved.Id.ToString();
                    string tableName = "Employee";

                    List<AuditRecord> auditRecords =
                        database.RetrieveAll <AuditRecord>(a => a.TableName == tableName &&  a.RecordId == recordId);


                    Assert.IsTrue(auditRecords.Count == 3);

                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Insert") != null);
                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Update") != null);
                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Delete") != null);
                };


            database.ExecuteInTest(procedure);
        }


        public void program_non_businessbase_should_be_able_to_audit()
        {
            Action procedure = () =>
                {
                    var program = new Program {Name = "Dinner Planning"};

                    database.Insert(program);

                    string recordId = program.Id.ToString();

                    var programRetrieved = database.RetrieveObject<Program>(p => p.Id == program.Id);

                    programRetrieved.Name = "Dinner Planning version 2";

                    database.Update(programRetrieved);

                    programRetrieved.Name = "Dinner Planning version 3";

                    database.Delete(programRetrieved);

                    string tableName = "Program";

                    List<AuditRecord> auditRecords =
                        database.RetrieveAll<AuditRecord>(a => a.TableName == tableName && a.RecordId == recordId);
                                          
                    Assert.IsTrue(auditRecords.Count == 3);

                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Insert") != null);
                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Update") != null);
                    Assert.IsTrue(auditRecords.Find(a => a.Operation == "Delete") != null);
                };


            database.ExecuteInTest(procedure);
        }



        public void delete_all_employee_programs_should_be_able_to_audit()
        {
            Action procedure = () =>
            {
                //arrange
                string tableName = "EMPLOYEE_PROGRAM";

                Employee employee = SharedCode.InstallEmployee(database);

                List<AuditRecord> auditRecords =
                   database.RetrieveAll<AuditRecord>(a => a.TableName == tableName);

                var total = auditRecords.FindAll(a => a.Operation == "Insert").Count;

                Assert.IsTrue(total == employee.Programs.Count);

                database.DeleteAll<AuditRecord>(a => a.TableName == tableName);

                //act
                database.DeleteAll<EmployeeProgram>(ep => ep.EmployeeId == employee.Id);
                
               //assert 
               auditRecords = database.RetrieveAll<AuditRecord>(a => a.TableName == tableName);

               Assert.IsTrue(auditRecords.Count == employee.Programs.Count);

               total = auditRecords.FindAll(a => a.Operation == "Delete").Count;

               Assert.IsTrue(total == employee.Programs.Count);


            };


            database.ExecuteInTest(procedure);
        }


        public void employee_should_be_able_to_retrieve_role_name()
        {
            Action procedure = () =>
            {
                var employee = SharedCode.InstallEmployee(database);

                var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                Assert.IsTrue(employeeRetrieved.Role.Name == "Employee");


                SharedCode.UninstallEmployee(employeeRetrieved, database);
            };


            database.ExecuteInTest(procedure);
        }

        public void employee_should_be_able_to_retrieve_country_name()
        {
            Action procedure = () =>
                {
                    var  employee = SharedCode.InstallEmployee(database);

                    var  employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeRetrieved.Country == Country.China.ToString());


                    SharedCode.UninstallEmployee(employeeRetrieved, database);
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_financial_info_bank_column_readonly()
        {
            Action procedure = () =>
                {
                    Employee employee = SharedCode.InstallEmployee(database);

                    employee.FinancialInfo = new FinancialInfo {EmployeeId = employee.Id};

                    database.Insert(employee.FinancialInfo);


                    Employee employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                    employeeRetrieved.FinancialInfo.Bank = "BMO";


                    new EmployeeBL(database).UpdateEmployee(employeeRetrieved);


                    Employee employeeUpdated = new EmployeeBL(database).RetrieveById(employee.Id);

                    Assert.IsTrue(employeeUpdated.FinancialInfo.Bank == "TD");
                };


            database.ExecuteInTest(procedure);
        }


        public void employee_self_join_should_be_supported()
        {
            Action procedure = () =>
            {
                Employee employee = SharedCode.InstallEmployee(database);

                var employee1 = FakeEmployee.GetFakeEmployee1(database);

                employee1.SupervisorId = employee.Id;

                new EmployeeBL(database).InsertEmployee(employee1);

                var employee1Retrieved = new EmployeeBL(database).RetrieveById(employee1.Id);

                Assert.IsTrue(employee1Retrieved.SupervisorId == employee.Id);
                Assert.IsTrue(employee1Retrieved.SupervisorFistName== employee.FirstName);
                Assert.IsTrue(employee1Retrieved.SupervisorLastName == employee.LastName);


            };


            database.ExecuteInTest(procedure);
        }

        

    }
}