using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class EmployeeProgramMapping : Mapping 
    {
        public override  IMappingConfig  Setup()
        {
            var config = Class<EmployeeProgram>().ToTable("EMPLOYEE_PROGRAM").EnableAudit();
                

            config.Property(ep => ep.Id).ToIdColumn("ID");
            config.Property(ep => ep.EmployeeId).ToColumn("EMPLOYEE_ID");

            config.Property(ep => ep.Status).ToColumn("STATUS");
            config.Property(ep => ep.ProgramId).ToColumn("PROGRAM_ID");


            var link1 = config.Link<EmployeeProgram, Program>(ep => ep.ProgramId, p => p.Id,LinkType.Property);
            config.Property(ep => ep.ProgramName).GetFrom<Program>(p => p.Name, link1);

        
            return config;
        }
    }
}