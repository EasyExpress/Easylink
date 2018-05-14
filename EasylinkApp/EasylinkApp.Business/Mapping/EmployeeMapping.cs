using Easylink;
 

namespace EasylinkApp.Business
{
    public   class  EmployeeMapping : Mapping 
    { 
 
        public override IMappingConfig Setup()
        {

             var config = Class<Employee>().ToTable("EMPLOYEE").NextId(NextIdOption.AutoIncrement);;

             config.Property(e => e.Id).ToIdColumn("ID");

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

             config.Link<Employee,Lookup>(e=>e.Department.Id,l => l.Id);
             config.Link<Employee,Lookup>(e=>e.Role.Id, l => l.Id);
             config.Link<Employee,Lookup>(e=>e.Sex.Id, l => l.Id);
     
             return config; 
        }


    }

         
 }


  
