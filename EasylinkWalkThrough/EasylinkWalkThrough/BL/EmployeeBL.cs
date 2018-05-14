using System.Collections.Generic;

using Easylink;
 

namespace EasylinkWalkThrough
{
    public class EmployeeBL : BaseBL
    {
        
        public EmployeeBL(IDatabase database)
        {
            Database = database; 
        }

        public List<Rule> ValidateEmployee(Employee  employee)
        {

            var rule = new CustomRule<Employee>(e=>e.LoginId, LoginIdShouldBeUnique)
                {
                    PropertyName = "LoginId",
                    ScreenName = "Login Id"
                };


            return employee.ValidateRules(rule);
        }

        private string  LoginIdShouldBeUnique(Employee employee)
        {
        
            var found =
                Database.RetrieveAll<Employee>(e=>e.LoginId==employee.LoginId)
                        .Find(e => e.Id != employee.Id);
            if (found != null)
            {
                return "Login id already exists!";
                
            }

            return string.Empty;
        }
    }
}
