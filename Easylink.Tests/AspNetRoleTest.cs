using System;
using System.Collections.Generic;


using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
  
    [TestClass]
    public abstract class AspNetRoleTest
    {
        protected IDatabase database;

        public void aspnetRole_should_be_able_to_insert()
        {
            Action procedure = () =>
                {
                    var aspnetRole = SharedCode.InstallAspNetRole(database);


                    var aspnetRoleRetrieved = database.RetrieveObject<AspNetRole>(r => r.RoleId == aspnetRole.RoleId && r.ApplicationId == aspnetRole.ApplicationId);

        
                    Assert.IsTrue(aspnetRoleRetrieved != null);

                    Assert.IsTrue(aspnetRoleRetrieved.ApplicationId == aspnetRole.ApplicationId);

                    Assert.IsTrue(aspnetRoleRetrieved.RoleName == aspnetRole.RoleName);
                };


            database.ExecuteInTest(procedure);
        }



        public void aspnetRole_should_be_able_to_delete()
        {
            Action procedure = () =>
            {
                var aspnetRole = SharedCode.InstallAspNetRole(database);

                var aspnetRoleRetrieved = database.RetrieveObject<AspNetRole>(r => r.RoleId == aspnetRole.RoleId && r.ApplicationId == aspnetRole.ApplicationId);

                Assert.IsTrue(aspnetRoleRetrieved != null);

                database.Delete(aspnetRoleRetrieved);

                aspnetRoleRetrieved =
                    database.RetrieveObject<AspNetRole>(
                        r => r.RoleId == aspnetRole.RoleId && r.ApplicationId == aspnetRole.ApplicationId);

                Assert.IsTrue(aspnetRoleRetrieved == null);


            };


            database.ExecuteInTest(procedure);
        }

        public void aspnetRole_should_be_able_to_update()
        {
            Action procedure = () =>
            {
                var aspnetRole = SharedCode.InstallAspNetRole(database);


                var aspnetRoleRetrieved = database.RetrieveObject<AspNetRole>(r => r.RoleId == aspnetRole.RoleId && r.ApplicationId == aspnetRole.ApplicationId);

        
                Assert.IsTrue(aspnetRoleRetrieved != null);

                aspnetRoleRetrieved.RoleName = "New Role";

                database.Update(aspnetRoleRetrieved);

                var aspnetRoleUpdated =
                    database.RetrieveObject<AspNetRole>(
                        r => r.RoleId == aspnetRole.RoleId && r.ApplicationId == aspnetRole.ApplicationId);

                Assert.IsTrue(aspnetRoleUpdated.RoleName == "New Role");


            };


            database.ExecuteInTest(procedure);
        }

    }
}

 