using System;

namespace Easylink.Tests.Model
{
    public class AspNetRole : BusinessBase
    {
       
        public Guid ApplicationId { get; set; }

        public Guid RoleId { get; set; }

        public string  RoleName { get; set; }
        
    }
}