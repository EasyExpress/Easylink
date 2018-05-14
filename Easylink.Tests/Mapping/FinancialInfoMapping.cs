using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class FinancialInfoMapping : Mapping
    {
        public override IMappingConfig  Setup()
        {
            var config = Class<FinancialInfo>().ToTable("FINANCIAL_INFO");

            config.Property(f => f.Id).ToIdColumn("ID");
            config.Property(f => f.EmployeeId).ToColumn("EMPLOYEE_ID");
            config.Property(f => f.Bank).ToColumn("BANK").AsReadOnly();

            return config;
        }
    }
}