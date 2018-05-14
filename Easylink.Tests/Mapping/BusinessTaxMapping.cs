using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class BusinessTaxMapping : Mapping
    {
        public override IMappingConfig   Setup()
        {
            var config = Class<BusinessTax>().ToTable("TAX");

            config.Property(b => b.CompanyName).ToColumn("USER_DEFINED_TEXT1");
            config.Property(b => b.Contact).ToColumn("USER_DEFINED_TEXT2");
            config.Property(b => b.TaxDueDate).ToColumn("USER_DEFINED_DATE1");
            config.Property(b => b.TaxableIncome).ToColumn("USER_DEFINED_MONEY1");
            config.Property(b => b.TaxRate).ToColumn("USER_DEFINED_DOUBLE1");
            config.Property(b => b.Waived).ToColumn("USER_DEFINED_BOOL1");

            return config;
        }
    }
}


 