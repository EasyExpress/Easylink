using System;
using System.Collections.Generic;
using Easylink;
using EasylinkApp.Business;
 

namespace EasylinkApp.Tests
{
    static class FakeEmployee
    {

     
       public  static Employee  GetFakeEmployee(IDatabase database)
       {
           return new Employee
           {

               FirstName = "test1",
               LastName = "test2",
               Salary = 12045.27M,
               EmployedSince = new DateTime(2010, 3, 12),
          

               Active = true,
               Department = new LookupBL(database).RetrieveLookupByName(Department.IT.ToString()),
               Role = new LookupBL(database).RetrieveLookupByName(Role.Employee.ToString()),

               Programs = new List<EmployeeProgram>
               {
                   new EmployeeProgram { ProgramId= 2, Status =  ProgramStatus.Registered},
                   new EmployeeProgram { ProgramId= 3, Status =  ProgramStatus.Completed},
               }

           };

       }


       public static List<Employee> GetFakeEmployees(IDatabase database)
       {
           return new List<Employee>  {  new Employee
                                                  {

                                                       FirstName = "david",
                                                       LastName = "smith",
                                                       Salary = 12045.27M,
                                                       EmployedSince = new DateTime(2011, 3, 12),
                                                       Active = true,
                                                       Department = new LookupBL(database).RetrieveLookupByName(Department.IT.ToString()),
                                                       Role = new LookupBL(database).RetrieveLookupByName(Role.Employee.ToString()),
                                                       Programs = new List<EmployeeProgram>
                                                                   {
                                                                       new EmployeeProgram { ProgramId= 2, Status =  ProgramStatus.Registered},
                                                                       new EmployeeProgram { ProgramId= 3, Status =  ProgramStatus.Completed},
                                                                   }


                                                 },

                                         
                                           new Employee
                                                  {

                                                       FirstName = "vincient",
                                                       LastName = "john",
                                                       Salary = 11045.39M,
                                                       EmployedSince = new DateTime(2011, 5,6),
                                                       Active = false,
                                                       Department = new LookupBL(database).RetrieveLookupByName(Department.Admin.ToString()),
                                                       Role = new LookupBL(database).RetrieveLookupByName(Role.Manager.ToString()),
                                                       Sex =  new LookupBL(database).RetrieveLookupByName(Sex.Male.ToString()),
                                                       Programs = new List<EmployeeProgram>
                                                                   {
                                                                       new EmployeeProgram { ProgramId= 1, Status =  ProgramStatus.Completed},
                                                                       new EmployeeProgram { ProgramId= 4, Status =  ProgramStatus.InProgress},
                                                                   }


                                                 }

                                         
                                         
           };



       }

    }
}
