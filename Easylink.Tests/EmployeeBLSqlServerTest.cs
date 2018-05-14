using System;

using Easylink.Tests.Business;
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class EmployeeBLSqlServerTest : EmployeeBLTest
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


            Mapping.SetNextId<Employee>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<Program>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<EmployeeProgram>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<Address>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<FinancialInfo>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<AdditionalInfo>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<Lookup>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<AuditRecord>(NextIdOption.AutoIncrement);

 
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_multiple_lookups()
        {
            employee_should_be_able_to_retrieve_multiple_lookups();
        }

        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_program_name()
        {
            employee_should_be_able_to_retrieve_program_name();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_insert()
        {
            employee_should_be_able_to_insert();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_update()
        {
            employee_should_be_able_to_update();
        }

        [TestMethod]
        public void sql_server_employee_address_should_be_able_to_update()
        {
            employee_address_should_be_able_to_update();
        }

        [TestMethod]
        public void sql_server_employee_should_retrieve_and_save_nullable()
        {
            employee_should_retrieve_and_save_nullable();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_audit()
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
                employeeRetrieved.Programs.Add(new EmployeeProgram { ProgramId = 1, Status = ProgramStatus.Completed });


                new EmployeeBL(database).UpdateEmployee(employeeRetrieved);


                SharedCode.UninstallEmployee(employee, database);


                //assert
                string recordId = employeeRetrieved.Id.ToString();
                string tableName = "Employee";

                var  auditRecords =
                    database.RetrieveAll<AuditRecord>(a => a.TableName == tableName &&  a.RecordId == recordId);
                                        


                Assert.IsTrue(auditRecords.Count == 3);

                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Insert") != null);
                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Update") != null);
                Assert.IsTrue(auditRecords.Find(a => a.Operation == "Delete") != null);


            };


            database.ExecuteInTest(procedure);
        }

        [TestMethod]
        public void sql_server_program_non_businessbase_should_be_able_to_audit()
        {
            program_non_businessbase_should_be_able_to_audit();
        }


        [TestMethod]
        public void sql_server_delete_all_employee_programs_should_be_able_to_audit()
        {
            delete_all_employee_programs_should_be_able_to_audit();
        }

        [TestMethod]
        public void sql_server_employee_should_be_able_to_delete_programs_using_criteria()
        {
            employee_should_be_able_to_delete_programs_using_criteria();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_additional_info_if_exists()
        {
            employee_should_be_able_to_retrieve_additional_info_if_exists();


        }



        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_financial_info()
        {
            employee_should_be_able_to_retrieve_financial_info();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_addresse()
        {
            employee_should_be_able_to_retrieve_addresses();
        }


        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_lookups()
        {
            employee_should_be_able_to_retrieve_lookups();
        }

          [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_role_name()
        {
              employee_should_be_able_to_retrieve_role_name();
        }

       

        [TestMethod]
        public void sql_server_employee_should_be_able_to_retrieve_country_name()
        {
            employee_should_be_able_to_retrieve_country_name();
        }

        [TestMethod]
        public void sql_server_employee_financial_info_bank_column_readonly()
        {
            employee_financial_info_bank_column_readonly();
        }

        [TestMethod]
        public void sql_server_employee_self_join_should_be_supported()
        {
              employee_self_join_should_be_supported();
        }


      
        
    }
}