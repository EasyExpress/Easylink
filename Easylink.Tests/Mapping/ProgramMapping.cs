using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class ProgramMapping : Mapping
    {
        public override IMappingConfig  Setup()
        {

            var config = Class<Program>().ToTable("PROGRAM").EnableAudit();
       
            config.Property(p => p.Id).ToIdColumn("ID");
            config.Property(p => p.Name).ToColumn("NAME");

            config.Property(p => p.Cost).ToColumn("COST");

            return config;

        }
    }
}