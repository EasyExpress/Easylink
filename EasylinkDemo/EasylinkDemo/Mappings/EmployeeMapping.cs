using Easylink;

namespace EasylinkDemo
{

    public class EmployeeMapping : Mapping
    {
        public override IMappingConfig Setup()
        {
            var config = Class<Employee>().ToTable("EMPLOYEE").NextId(NextIdOption.AutoIncrement);
            config.Property(e => e.Id).ToIdColumn("Id");
            config.Property(e => e.FirstName).ToColumn("FIRST_NAME");
            config.Property(e => e.LastName).ToColumn("LAST_NAME");
            config.Property(e => e.Active).ToColumn("ACTIVE");
            config.Property(e => e.LoginId).ToColumn("LOGIN_ID");
            config.Property(e => e.Role.Id).ToColumn("ROLE_ID");
            config.Link<Employee, Lookup>(e => e.Role.Id, l => l.Id);
            return config;
        }

    }

}