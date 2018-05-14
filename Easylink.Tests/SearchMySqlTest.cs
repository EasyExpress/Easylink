using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    [TestClass]
    public class SearchMySqlTest : SearchTest
    {
        [TestInitialize]
        public void Test_Initialization()
        {
          
            DatabaseFactory.Initialize(
               new DbConfig()
               {
                   ConnectionString =
                       "Server=127.0.0.1;Port=3306; Uid=root; Pwd=1197344; Database=test;",
                   DatabaseType = DatabaseType.MySql,
                   SchemaName = "test",
                   AuditRecordType = typeof(AuditRecord)
               });
               
           
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
        public void mysql_server_employee_search_all()
        {
            employee_search_all();
        }

        [TestMethod]
        public void mysql_employee_search_by_equal_id()
        {
            employee_search_by_equal_id();
        }

        [TestMethod]
        public void mysql_employee_search_by_multiple_conditions()
        {
            employee_search_by_multiple_conditions();
        }

        [TestMethod]
        public void mysql_employee_search_by_not_equal_id()
        {
            employee_search_by_not_equal_id();
        }

        [TestMethod]
        public void mysql_employee_search_by_equal_identifier()
        {
            employee_search_by_equal_identifier();
        }


        [TestMethod]
        public void mysql_employee_search_by_equal_date()
        {
            employee_search_by_equal_date();
        }

        [TestMethod]
        public void mysql_employee_search_by_not_equal_date()
        {
            employee_search_by_not_equal_date();
        }


        [TestMethod]
        public void mysql_employee_search_by_greater_date()
        {
            employee_search_by_greater_date();
        }


        [TestMethod]
        public void mysql_employee_search_by_less_date()
        {
            employee_search_by_less_date();
        }


        [TestMethod]
        public void mysql_employee_search_by_equal_bool()
        {
            employee_search_by_equal_bool();
        }

        [TestMethod]
        public void mysql_employee_search_by_not_equal_bool()
        {
            employee_search_by_not_equal_bool();
        }

        [TestMethod]
        public void mysql_employee_search_by_equal_decimal()
        {
            employee_search_by_equal_decimal();
        }


        [TestMethod]
        public void mysql_employee_search_by_not_equal_decimal()
        {
            employee_search_by_not_equal_decimal();
        }

        [TestMethod]
        public void mysql_employee_search_by_greater_decimal()
        {
            employee_search_by_greater_decimal();
        }

        [TestMethod]
        public void mysql_employee_search_by_less_decimal()
        {
            employee_search_by_less_decimal();
        }


        [TestMethod]
        public void mysql_employee_search_by_equal_text()
        {
            employee_search_by_equal_text();
        }

        [TestMethod]
        public void mysql_employee_search_by_not_equal_text()
        {
            employee_search_by_not_equal_text();
        }


        [TestMethod]
        public void mysql_employee_search_by_startswith_text()
        {
            employee_search_by_starts_with_text();
        }

        [TestMethod]
        public void mysql_employee_search_by_endswith_text()
        {
            employee_search_by_endswith_text();
        }


        [TestMethod]
        public void mysql_employee_search_by_contains_text()
        {
            employee_search_by_contains_text();
        }


        [TestMethod]
        public void mysql_employee_search_by_contains_text_case_sensitive()
        {
            employee_search_by_contains_text_case_sensitive();
        }


        [TestMethod]
        public void mysql_employee_search_by_in_list()
        {
            employee_search_by_in_list();
        }

        [TestMethod]
        public void mysql_employee_search_by_role_name()
        {
            employee_search_by_role_name();
        }

        [TestMethod]
        public void mysql_employee_search_by_country()
        {
            employee_search_by_country();
        }



 
    }
}