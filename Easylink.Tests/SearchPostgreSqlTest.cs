using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    [TestClass]
    public class SearchPostgreSqlTest : SearchTest
    {
        [TestInitialize]
        public   void Test_Initialization()
        {

            DatabaseFactory.Initialize(
                new DbConfig()
                    {
                        ConnectionString =
                            "Server=localhost;Port=5432;Database=Easylink;Pooling =false; User Id=postgres;Password=1197344;",
                        DatabaseType = DatabaseType.PostgreSql,
                        SchemaName = "public",
                        AuditRecordType =typeof(AuditRecord)
                    });
               
              

            database = DatabaseFactory.Create();


            Mapping.SetNextId<Employee>(NextIdOption.Sequence, "employee_seq");
            Mapping.SetNextId<Program>(NextIdOption.Sequence, "program_seq");
            Mapping.SetNextId<EmployeeProgram>(NextIdOption.Sequence, "employee_program_seq");
            Mapping.SetNextId<Address>(NextIdOption.Sequence, "address_seq");
            Mapping.SetNextId<FinancialInfo>(NextIdOption.Sequence, "financial_info_seq");
            Mapping.SetNextId<AdditionalInfo>(NextIdOption.Sequence, "additional_info_seq");
            Mapping.SetNextId<Lookup>(NextIdOption.Sequence, "lookup_seq");
            Mapping.SetNextId<AuditRecord>(NextIdOption.Sequence, "audit_seq");
        }


        [TestMethod]
        public void postgreSql_server_employee_search_all()
        {
            employee_search_all();
        }

        
        [TestMethod]
        public void postgreSql_employee_search_by_equal_id()
        {
            employee_search_by_equal_id();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_equal_identifier()
        {
            employee_search_by_equal_identifier();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_multiple_conditions()
        {
            employee_search_by_multiple_conditions();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_not_equal_id()
        {
            employee_search_by_not_equal_id();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_equal_date()
        {
            employee_search_by_equal_date();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_not_equal_date()
        {
            employee_search_by_not_equal_date();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_greater_date()
        {
            employee_search_by_greater_date();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_less_date()
        {
            employee_search_by_less_date();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_equal_bool()
        {
            employee_search_by_equal_bool();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_not_equal_bool()
        {
            employee_search_by_not_equal_bool();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_equal_decimal()
        {
            employee_search_by_equal_decimal();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_not_equal_decimal()
        {
            employee_search_by_not_equal_decimal();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_greater_decimal()
        {
            employee_search_by_greater_decimal();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_less_decimal()
        {
            employee_search_by_less_decimal();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_equal_text()
        {
            employee_search_by_equal_text();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_not_equal_text()
        {
            employee_search_by_not_equal_text();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_startswith_text()
        {
            employee_search_by_starts_with_text();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_endswith_text()
        {
            employee_search_by_endswith_text();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_contains_text()
        {
            employee_search_by_contains_text();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_contains_text_case_sensitive()
        {
            employee_search_by_contains_text_case_sensitive();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_in_list()
        {
            employee_search_by_in_list();
        }


        [TestMethod]
        public void postgreSql_employee_search_by_role_name()
        {
            employee_search_by_role_name();
        }

        [TestMethod]
        public void postgreSql_employee_search_by_country()
        {
            employee_search_by_country();
        }


       
 
    }
}