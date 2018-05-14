using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class EmployeeMapping : Mapping
    {
        public override IMappingConfig  Setup()
        {

            var config = Class<Employee>().ToTable("EMPLOYEE").EnableAudit();

            config.Property(e => e.Id).ToIdColumn("ID");
            config.Property(e => e.Identifier).ToColumn("IDENTIFIER");
            config.Property(e => e.FirstName).ToColumn("FIRST_NAME");
            config.Property(e => e.LastName).ToColumn("LAST_NAME");
            config.Property(e => e.Salary).ToColumn("SALARY");
            config.Property(e => e.EmployedSince).ToColumn("EMPLOYED_SINCE");
            config.Property(e => e.Active).ToColumn("ACTIVE");
            config.Property(e => e.Age).ToColumn("AGE");
            config.Property(e => e.Weight).ToColumn("WEIGHT");
            config.Property(e => e.Married).ToColumn("MARRIED");

            config.Property(e => e.Department.Id).ToColumn("DEPARTMENT_ID");
            config.Property(e => e.Role.Id).ToColumn("ROLE_ID");
            config.Property(e => e.Sex.Id).ToColumn("SEX_ID");
            config.Property(e => e.Address.Id).ToColumn("ADDRESS_ID");
            config.Property(e => e.AdditionalInfo.Id).ToColumn("ADDITIONAL_INFO_ID");
            config.Property(e => e.SupervisorId).ToColumn("SUPERVISOR_ID");

            config.Link<Employee, Lookup>(e => e.Department.Id, l => l.Id);

            config.Link<Employee, Lookup>(e => e.Role.Id, l => l.Id);
            config.Link<Employee, Lookup>(e => e.Sex.Id, l => l.Id);
            config.Link<Employee, AdditionalInfo>(e => e.AdditionalInfo.Id, a => a.Id);
            config.Link<Employee, FinancialInfo>(e => e.Id, f => f.EmployeeId);
 
             //address
             var link5 = config.Link<Employee, Address>(e => e.Address.Id, a => a.Id);
             var link6 = config.Link<Address, Lookup>(a => a.Country.Id, l => l.Id).Extend(link5);

             config.Property(e => e.Country).GetFrom<Lookup>(l => l.Name, link6);

            var link7 = config.Link<Employee, Employee>(e => e.SupervisorId, e => e.Id, LinkType.Property);

             config.Property(e => e.SupervisorFistName).GetFrom<Employee>(e => e.FirstName, link7);
             config.Property(e => e.SupervisorLastName).GetFrom<Employee>(e => e.LastName, link7);

            return config;

        }
    }
}