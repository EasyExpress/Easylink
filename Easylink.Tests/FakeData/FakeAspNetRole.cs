using System;

using Easylink.Tests.Model;

namespace Easylink.Tests
{
    internal static class FakeAspNetRole
    {
        public static AspNetRole GetFakeAspNetRole()
        {

            return new AspNetRole
                {
                   ApplicationId  = Guid.NewGuid(),
                   RoleId = Guid.NewGuid(),
                   RoleName ="Administrator"
                   
                };
        }

    }

}