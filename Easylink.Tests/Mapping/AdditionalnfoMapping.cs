using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public  class AdditionalInfoMapping : Mapping
    {
        public override IMappingConfig Setup()
        {

            var config = Class<AdditionalInfo>().ToTable("ADDITIONAL_INFO");

            config.Property(c => c.Id).ToIdColumn("ADDITIONAL_INFO_ID");
            config.Property(c => c.Notes).ToColumn("NOTES");

            return config;

        }
    }
}