using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    [TestClass]
    public class SearchOracleTest : SearchTest
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
                    AuditRecordType = typeof (AuditRecord)
                };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();


            Mapping.SetNextId<Employee>(NextIdOption.Sequence, "EMPLOYEE_ID_SEQ");
            Mapping.SetNextId<Program>(NextIdOption.Sequence, "PROGRAM_ID_SEQ");
            Mapping.SetNextId<EmployeeProgram>(NextIdOption.Sequence, "EMPLOYEE_PROGRAM_ID_SEQ");
            Mapping.SetNextId<Address>(NextIdOption.Sequence, "ADDRESS_ID_SEQ");
            Mapping.SetNextId<FinancialInfo>(NextIdOption.Sequence, "FINANCIAL_INFO_ID_SEQ");
            Mapping.SetNextId<AdditionalInfo>(NextIdOption.Sequence, "ADDITIONAL_INFO_ID_SEQ");
            Mapping.SetNextId<Lookup>(NextIdOption.Sequence, "LOOKUP_ID_SEQ");
            Mapping.SetNextId<AuditRecord>(NextIdOption.Sequence, "AUDIT_ID_SEQ");

        }


        [TestMethod]
        public void oracle_server_employee_search_all()
        {
            employee_search_all();
        }


        [TestMethod]
        public void oracle_employee_search_by_equal_id()
        {
            employee_search_by_equal_id();
        }

        [TestMethod]
        public void oracle_employee_search_by_equal_identifier()
        {
            employee_search_by_equal_identifier();
        }

        [TestMethod]
        public void oracle_employee_search_by_multiple_conditions()
        {
            employee_search_by_multiple_conditions();
        }

        [TestMethod]
        public void oracle_employee_search_by_not_equal_id()
        {
            employee_search_by_not_equal_id();
        }


        [TestMethod]
        public void oracle_employee_search_by_equal_date()
        {
            employee_search_by_equal_date();
        }

        [TestMethod]
        public void oracle_employee_search_by_not_equal_date()
        {
            employee_search_by_not_equal_date();
        }

        [TestMethod]
        public void oracle_employee_search_by_greater_date()
        {
            employee_search_by_greater_date();
        }


        [TestMethod]
        public void oracle_employee_search_by_less_date()
        {
            employee_search_by_less_date();
        }


        [TestMethod]
        public void oracle_employee_search_by_equal_bool()
        {
            employee_search_by_equal_bool();
        }

        [TestMethod]
        public void oracle_employee_search_by_not_equal_bool()
        {
            employee_search_by_not_equal_bool();
        }

        [TestMethod]
        public void oracle_employee_search_by_equal_decimal()
        {
            employee_search_by_equal_decimal();
        }


        [TestMethod]
        public void oracle_employee_search_by_not_equal_decimal()
        {
            employee_search_by_not_equal_decimal();
        }

        [TestMethod]
        public void oracle_employee_search_by_greater_decimal()
        {
            employee_search_by_greater_decimal();
        }

        [TestMethod]
        public void oracle_employee_search_by_less_decimal()
        {
            employee_search_by_less_decimal();
        }


        [TestMethod]
        public void oracle_employee_search_by_equal_text()
        {
            employee_search_by_equal_text();
        }

        [TestMethod]
        public void oracle_employee_search_by_not_equal_text()
        {
            employee_search_by_not_equal_text();
        }

        [TestMethod]
        public void oracle_employee_search_by_startswith_text()
        {
            employee_search_by_starts_with_text();
        }

        [TestMethod]
        public void oracle_employee_search_by_endswith_text()
        {
            employee_search_by_endswith_text();
        }


        [TestMethod]
        public void oracle_employee_search_by_contains_text()
        {
            employee_search_by_contains_text();
        }

        [TestMethod]
        public void oracle_employee_search_by_contains_text_case_sensitive()
        {
            employee_search_by_contains_text_case_sensitive();
        }


        [TestMethod]
        public void oracle_employee_search_by_in_list()
        {
            employee_search_by_in_list();
        }


        [TestMethod]
        public void oracle_employee_search_by_role_name()
        {
            employee_search_by_role_name();
        }

        [TestMethod]
        public void oracle_employee_search_by_country()
        {
            employee_search_by_country();
        }

 

 
    }
}