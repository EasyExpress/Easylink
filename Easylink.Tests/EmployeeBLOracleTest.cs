using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class EmployeeBLOracleTest : EmployeeBLTest
    {
        [TestInitialize]
        public   void Test_Initialization()
        {


            var dbConfig = new DbConfig()
            {
                ConnectionString =
                    string.Format("Data Source= " +
                                  "(DESCRIPTION =" +
                                  "(ADDRESS = (PROTOCOL = TCP)(HOST = {0} )(PORT = {1}))" +
                                  "(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};",
                                  "localhost", "1521", "xe", "easylink", "1197344"),
                DatabaseType = DatabaseType.Oracle,
                SchemaName = "EASYLINK",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);
 

            database = DatabaseFactory.Create();


            Mapping.SetNextId<Employee>(NextIdOption.Sequence,"EMPLOYEE_ID_SEQ");
            Mapping.SetNextId<Program>(NextIdOption.Sequence, "PROGRAM_ID_SEQ");
            Mapping.SetNextId<EmployeeProgram>(NextIdOption.Sequence, "EMPLOYEE_PROGRAM_ID_SEQ");
            Mapping.SetNextId<Address>(NextIdOption.Sequence, "ADDRESS_ID_SEQ");
            Mapping.SetNextId<FinancialInfo>(NextIdOption.Sequence, "FINANCIAL_INFO_ID_SEQ");
            Mapping.SetNextId<AdditionalInfo>(NextIdOption.Sequence, "ADDITIONAL_INFO_ID_SEQ");
            Mapping.SetNextId<Lookup>(NextIdOption.Sequence, "LOOKUP_ID_SEQ");
            Mapping.SetNextId<AuditRecord>(NextIdOption.Sequence, "AUDIT_ID_SEQ");
 
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_multiple_lookups()
        {
            employee_should_be_able_to_retrieve_multiple_lookups();
        }

        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_program_name()
        {
            employee_should_be_able_to_retrieve_program_name();
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_insert()
        {
            employee_should_be_able_to_insert();
        }

        [TestMethod]
        public void oracle_employee_should_be_able_to_update()
        {
            employee_should_be_able_to_update();
        }


        [TestMethod]
        public void oracle_employee_address_should_be_able_to_update()
        {
            employee_address_should_be_able_to_update();
        }

        [TestMethod]
        public void oracle_employee_should_retrieve_and_save_nullable()
        {
            base.employee_should_retrieve_and_save_nullable();
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_audit()
        {
            employee_should_be_able_to_audit();
        }


        [TestMethod]
        public void oracle_delete_all_employee_programs_should_be_able_to_audit()
        {
            delete_all_employee_programs_should_be_able_to_audit();
        }

        [TestMethod]
        public void oracle_program_non_businessbase_should_be_able_to_audit()
        {
            program_non_businessbase_should_be_able_to_audit();
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_delete_programs_using_criteria()
        {
            employee_should_be_able_to_delete_programs_using_criteria();
        }

        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_additional_info_if_exists()
        {
            employee_should_be_able_to_retrieve_additional_info_if_exists();


        }

        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_financial_info()
        {
            employee_should_be_able_to_retrieve_financial_info();
        }

        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_addresses()
        {
            employee_should_be_able_to_retrieve_addresses();
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_lookups()
        {
            employee_should_be_able_to_retrieve_lookups();
        }


        [TestMethod]
        public void oracle_employee_should_be_able_to_retrieve_country_name()
        {
            employee_should_be_able_to_retrieve_country_name();
        }


        [TestMethod]
        public void oracle_employee_financial_info_bank_column_readonly()
        {

            employee_financial_info_bank_column_readonly();

        }


        [TestMethod]
        public void oracle_employee_self_join_should_be_supported()
        {
            employee_self_join_should_be_supported();
        }
    }
}