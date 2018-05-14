using System;
using System.Collections.Generic;
using Easylink;


namespace EasylinkApp.Business
{
 
    public class Employee : BusinessBase
    {

        public Int64 Id { get; set; }
 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public bool Active { get; set; }

        public DateTime? EmployedSince { get; set; }

        public int? Age { get; set; }

        public decimal? Weight { get; set; }

        public bool?  Married { get; set; }

        public Lookup Department { get; set; }

        public Lookup Role { get; set; }

        public Lookup Sex { get; set; }

     
        public List<EmployeeProgram> Programs; 


        public Employee()
        {
            Programs = new List<EmployeeProgram>();

   

            AddRules<Employee>(e=>e.FirstName,"First Name", new RequiredRule());

           
            AddCustomRules(new CustomRule<Employee>(ProgramIdShouldBeUnique));
 

        }
 

        private static string ProgramIdShouldBeUnique(Employee employee)
        {
            string result = string.Empty;

            employee.Programs.ForEach(p =>
                {

                    var found = employee.Programs.FindAll(program => program.ProgramId == p.ProgramId);

                    if (found.Count >= 2)
                    {
                        result = "Two or more programs are same.";

                    }

                });
 

            return result;

        }

      

 
    }

     
}
