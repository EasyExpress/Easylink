using System;
using Easylink;


namespace EasylinkWalkThrough
{
   
    public class Employee : BusinessBase
    {
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool   Active { get; set; }

        
        public string LoginId { get; set; }

        public Lookup Role { get; set; }

        public Employee()
        {
            AddRules<Employee>(e=>e.LoginId, "Login Id", new RequiredRule());

              
        }

    }
    
}
