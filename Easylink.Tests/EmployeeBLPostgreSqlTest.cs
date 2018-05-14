using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class EmployeeBLPostgreSqlTest : EmployeeBLTest
    {
        [TestInitialize]
        public  void Test_Initialization()
        {

            

            var dbConfig = new DbConfig()
            {
                ConnectionString = "Server=localhost;Port=5432;Database=Easylink;Pooling =false; User Id=postgres;Password=1197344;",
                DatabaseType = DatabaseType.PostgreSql,
                SchemaName = "public",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);


            database = DatabaseFactory.Create();

            Mapping.SetNextId<Employee>(NextIdOption.Sequence,"employee_seq");
            Mapping.SetNextId<Program>(NextIdOption.Sequence, "program_seq");
            Mapping.SetNextId<EmployeeProgram>(NextIdOption.Sequence, "employee_program_seq");
            Mapping.SetNextId<Address>(NextIdOption.Sequence, "address_seq");
            Mapping.SetNextId<FinancialInfo>(NextIdOption.Sequence, "financial_info_seq");
            Mapping.SetNextId<AdditionalInfo>(NextIdOption.Sequence, "additional_info_seq");
            Mapping.SetNextId<Lookup>(NextIdOption.Sequence, "lookup_seq");
            Mapping.SetNextId<AuditRecord>(NextIdOption.Sequence, "audit_seq");
 
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_multiple_lookups()
        {
            employee_should_be_able_to_retrieve_multiple_lookups();
        }

        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_program_name()
        {
            employee_should_be_able_to_retrieve_program_name();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_insert()
        {
            employee_should_be_able_to_insert();
        }

 

        [TestMethod]
        public void postgreSql_employee_should_be_able_to_update()
        {
            employee_should_be_able_to_update();
        }

        [TestMethod]
        public void postgreSql_employee_address_should_be_able_to_update()
        {
            employee_address_should_be_able_to_update();
        }

        [TestMethod]
        public void postgreSql_employee_should_retrieve_and_save_nullable()
        {
            employee_should_retrieve_and_save_nullable();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_audit()
        {
            employee_should_be_able_to_audit();
        }


        [TestMethod]
        public void postgresql_delete_all_employee_programs_should_be_able_to_audit()
        {
            delete_all_employee_programs_should_be_able_to_audit();
        }


        [TestMethod]
        public void postgresql_program_non_businessbase_should_be_able_to_audit()
        {
            program_non_businessbase_should_be_able_to_audit();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_delete_programs_using_criteria()
        {
            employee_should_be_able_to_delete_programs_using_criteria();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_financial_info()
        {
            employee_should_be_able_to_retrieve_financial_info();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_addresses()
        {
            employee_should_be_able_to_retrieve_addresses();
        }


        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_lookups()
        {
            employee_should_be_able_to_retrieve_lookups();
        }

        [TestMethod]
        public void postgreSql_employee_should_be_able_to_retrieve_country_name()
        {
            employee_should_be_able_to_retrieve_country_name();
        }


        [TestMethod]
        public void postgreSql_employee_financial_info_bank_column_readonly()
        {
            employee_financial_info_bank_column_readonly();
        }


        [TestMethod]
        public void postgreSql_employee_self_join_should_be_supported()
        {
            employee_self_join_should_be_supported();
        }
    }
}