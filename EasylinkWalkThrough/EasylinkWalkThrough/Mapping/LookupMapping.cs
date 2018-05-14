using Easylink;


namespace EasylinkWalkThrough
{
    public class LookupMapping : Mapping
    {

        public override IMappingConfig Setup()
        {

            var config = Class<Lookup>().ToTable("LOOKUP").NextId(NextIdOption.AutoIncrement);

            config.Property(l => l.Id).ToIdColumn("ID");
            config.Property(l => l.ParentId).ToColumn("PARENT_ID");
            config.Property(l => l.Name).ToColumn("NAME");

            return config;
        }
    }
}
