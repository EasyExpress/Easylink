using System;
using System.Collections.Generic;

namespace Easylink.Tests.Model
{
    [Serializable]
    public class Employee : BusinessBase
    {
        public List<EmployeeProgram> Programs;

        public Int64 Id { get; set; }

        public Guid Identifier { get; set; }

 
        public string FirstName { get; set; }

     
        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public bool Active { get; set; }

        public DateTime? EmployedSince { get; set; }

        public int? Age { get; set; }

        public decimal? Weight { get; set; }

        public bool? Married { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

 
        public string SocialInsuranceNumber { get; set; }


        public Lookup Department { get; set; }

        public Lookup Role { get; set; }

        public Lookup Sex { get; set; }

        public Address Address { get; set; }

        public FinancialInfo FinancialInfo { get; set; }

        public AdditionalInfo AdditionalInfo { get; set;  }

        public Int64? SupervisorId { get; set; }

        public string SupervisorFistName { get; set; }

        public string SupervisorLastName { get; set; }
 

        public Employee()
        {
            Programs = new List<EmployeeProgram>();


           
          
            AddRules<Employee>(e => e.FirstName, "First Name", new RequiredRule(), 
                                                 new MaximumLengthRule(20), 
                                                 new ContainsNoSpaceRule(),
                                                 new WordCountRule(2));

            AddRules<Employee>(e => e.LastName,  new ContainsNoSpaceRule());

            AddRules<Employee>(e => e.Salary, new ThresholdRule(">", 10000));

            AddRules<Employee>(e => e.Salary, "Salary", new RangeRule(10000.00, 90000.00));

            AddRules<Employee>(e => e.EmployedSince, "Employeed Since", new RangeRule(new DateTime(1915, 12, 31), new DateTime(2015, 12, 31), "MM/dd/yyyy"));


            AddRules<Employee>(e => e.SocialInsuranceNumber, "Social Insurance Number", new FixedLengthRule(9), 
                                                             new NumericRule());

            AddRules<Employee>(e => e.Email, new EmailValidRule());

            AddRules<Employee>(e => e.Phone, new PhoneValidRule());

            AddRules<Employee>(e => e.Address.PostalCode,"Postal Code", new PostalCodeValidRule());

            AddCustomRules(new CustomRule<Employee>(ProgramsCanNotBeSame));
       
        }

 

        private string ProgramsCanNotBeSame(Employee employee)
        {
            var  errorMessage = "";

            employee.Programs.ForEach(p =>
                {
                    var  found = Programs.FindAll(program => program.ProgramId == p.ProgramId);

                    if (found.Count >= 2)
                    {
                        errorMessage = "Two or more programs are same.";
                    }
                });

            return errorMessage;
        }
    }
}