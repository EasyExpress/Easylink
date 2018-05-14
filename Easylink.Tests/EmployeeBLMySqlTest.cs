using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    
    [TestClass]
    public class EmployeeBLMySqlTest : EmployeeBLTest
    {
        [TestInitialize]
        public  void Test_Initialization()
        {

 
            var dbConfig = new DbConfig()
            {
                ConnectionString = "Server=127.0.0.1;Port=3306; Uid=root; Pwd=1197344; Database=test;",
                DatabaseType =  DatabaseType.MySql, 
                SchemaName = "test",
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
        public void my_sql_employee_should_be_able_to_retrieve_multiple_lookups()
        {
            employee_should_be_able_to_retrieve_multiple_lookups();
        }

        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_program_name()
        {
            employee_should_be_able_to_retrieve_program_name();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_insert()
        {
            employee_should_be_able_to_insert();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_update()
        {
            employee_should_be_able_to_update();
        }

        [TestMethod]
        public void my_sql_employee_address_should_be_able_to_update()
        {
            employee_address_should_be_able_to_update();
        }


        [TestMethod]
        public void my_sql_employee_should_retrieve_and_save_nullable()
        {
            base.employee_should_retrieve_and_save_nullable();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_audit()
        {
            employee_should_be_able_to_audit();
        }



        [TestMethod]
        public void mysql_delete_all_employee_programs_should_be_able_to_audit()
        {
            delete_all_employee_programs_should_be_able_to_audit();
        }

        [TestMethod]
        public void mysql_program_non_businessbase_should_be_able_to_audit()
        {
            program_non_businessbase_should_be_able_to_audit();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_delete_programs_using_criteria()
        {
            employee_should_be_able_to_delete_programs_using_criteria();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_additional_info_if_exists()
        {
            employee_should_be_able_to_retrieve_additional_info_if_exists();


        }

        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_financial_info()
        {
            employee_should_be_able_to_retrieve_financial_info();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_addresses()
        {
            employee_should_be_able_to_retrieve_addresses();
        }


        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_lookups()
        {
            employee_should_be_able_to_retrieve_lookups();
        }

        [TestMethod]
        public void my_sql_employee_should_be_able_to_retrieve_country_name()
        {
            employee_should_be_able_to_retrieve_country_name();
        }


        [TestMethod]
        public void my_sql_employee_financial_info_bank_column_readonly()
        {
            employee_financial_info_bank_column_readonly();
        }


        [TestMethod]
        public void my_sql_employee_self_join_should_be_supported()
        {
            employee_self_join_should_be_supported();
        }
    }
}