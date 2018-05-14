using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
      public class PropertyTaxMapping : Mapping
        {
            public override  IMappingConfig  Setup()
            {
                var config = Class<PropertyTax>().ToTable("TAX");

                config.Property(p => p.City).ToColumn("USER_DEFINED_TEXT1");
                config.Property(p => p.PropertyCode).ToColumn("USER_DEFINED_TEXT2");
                config.Property(p => p.EffectiveDate).ToColumn("USER_DEFINED_DATE1");
                config.Property(p => p.TaxPayable).ToColumn("USER_DEFINED_MONEY1");
                config.Property(p => p.TaxRate).ToColumn("USER_DEFINED_DOUBLE1");
                config.Property(p => p.IsResidential).ToColumn("USER_DEFINED_BOOL1");


                return config;

            }
        }
    
}

 