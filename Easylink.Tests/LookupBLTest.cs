using System;
using System.Collections.Generic;

using Easylink.Tests.Business;
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public abstract class LookupBLTest
    {
        protected IDatabase database;

        public void lookup_should_be_able_to_insert()
        {
            Action procedure = () =>
                {
                    //Act
                    var lookup = new LookupBL(database).RetrieveLookupByName("Role");

                    var newRoleLookup = new Lookup() {ParentId = lookup.Id, Name = "Temporary"};

                    new LookupBL(database).Insert(newRoleLookup);

                    var lookupRetrieved = new LookupBL(database).RetrieveLookupByName("Temporary");

                    //Assert 
                    Assert.IsNotNull(lookupRetrieved);

                    Assert.IsTrue(lookupRetrieved.Name == "Temporary");

                };


            database.ExecuteInTest(procedure);
        }

         
    
    }
}