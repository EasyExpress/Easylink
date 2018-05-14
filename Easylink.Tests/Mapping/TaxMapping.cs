using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class TaxMapping : Mapping
    {
        public override  IMappingConfig Setup()
        {
            var config = Class<Tax>().ToTable("TAX");

            config.Property(t => t.Id).ToIdColumn("TAX_ID");
            config.Property(t => t.TaxName).ToColumn("TAX_NAME");

            return config; 

        }
    }
}