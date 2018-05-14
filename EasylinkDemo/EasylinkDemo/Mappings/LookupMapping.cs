using Easylink;
 

namespace EasylinkDemo
{
    public class LookupMapping :Mapping
    {
        public override IMappingConfig Setup()
        {

            var config = Class<Lookup>().ToTable("LOOKUP").NextId(NextIdOption.AutoIncrement);

            config.Property(c => c.Id).ToIdColumn("ID");
            config.Property(c => c.ParentId).ToColumn("PARENT_ID");
            config.Property(c => c.Name).ToColumn("NAME");

            return config;

        }
    }
}
