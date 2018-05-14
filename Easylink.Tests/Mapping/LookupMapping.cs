using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class LookupMapping : Mapping
    {
        public override IMappingConfig Setup()
        {

            var config = Class<Lookup>().ToTable("LOOKUP");
                
 
            config.Property(l => l.Id).ToIdColumn("ID");
            config.Property(l => l.ParentId).ToColumn("PARENT_ID");
            config.Property(l => l.Name).ToColumn("NAME");

            return config;

        }
    }
}