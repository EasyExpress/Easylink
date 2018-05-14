using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class AspNetRoleMapping : Mapping
    {
        public override IMappingConfig  Setup()
        {
            var config = Class<AspNetRole>().ToTable("ASPNET_ROLE");

            config.Property(r => r.RoleId).ToIdColumn("RoleId");

            config.Property(r => r.ApplicationId).ToIdColumn("ApplicationId");
           
            config.Property(r => r.RoleName).ToColumn("RoleName");
       
            return config;
        }
    }
}