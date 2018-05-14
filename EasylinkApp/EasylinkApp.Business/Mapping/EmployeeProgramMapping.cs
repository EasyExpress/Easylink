

using Easylink;
 

namespace EasylinkApp.Business
{
    public   class  EmployeeProgrammMapping : Mapping 
    {

  
        public override IMappingConfig Setup()
        {

             var config = Class<EmployeeProgram>().ToTable("EMPLOYEE_PROGRAM").NextId(NextIdOption.AutoIncrement);

             config.Property(c => c.Id).ToIdColumn("ID");
             config.Property(c => c.EmployeeId).ToColumn("EMPLOYEE_ID");
             config.Property(c => c.ProgramId).ToColumn("PROGRAM_ID");

             config.Property(c => c.Status).ToColumn("STATUS");
           
             var link1 = config.Link<EmployeeProgram, Program>(ep=>ep.ProgramId, p => p.Id,LinkType.Property);

             config.Property(c => c.ProgramName).GetFrom<Program>(p => p.Name,link1);
     
             return config; 
        }

        
    }

         
 }

 



     
  
