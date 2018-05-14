using System;
 
 
using Easylink;
using Easylink.Tests;
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasylinkApp.Tests
{
    [TestClass]
    public class InheritedMappingTest
    {
        protected IDatabase database;

        [TestInitialize]
        public void Test_Initialization()
        {


            var dbConfig = new DbConfig()
            {
                ConnectionString = @"Server=meng\sqlexpress;Database=easylink;User Id=easylink; Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

            Mapping.SetNextId<BusinessTax>(NextIdOption.AutoIncrement);
            Mapping.SetNextId<PropertyTax>(NextIdOption.AutoIncrement);

        }


        [TestMethod]
        public void tax_should_insert_property_tax()
        {

            Action procedure = () =>
            {
                var propertyTax = FakeTax.GetFakePropertyTax();

                database.Insert(propertyTax);

                var propertyTaxRetrieved = database.RetrieveObject<PropertyTax>(p => p.Id == propertyTax.Id);

                Assert.IsNotNull(propertyTaxRetrieved);

                Assert.IsTrue(propertyTaxRetrieved.TaxName == propertyTax.TaxName);
                Assert.IsTrue(propertyTaxRetrieved.City  == propertyTax.City);
                Assert.IsTrue(propertyTaxRetrieved.PropertyCode == propertyTax.PropertyCode);
                Assert.IsTrue(propertyTaxRetrieved.IsResidential  == propertyTax.IsResidential );
                Assert.IsTrue(propertyTaxRetrieved.TaxPayable  == propertyTax.TaxPayable);
                Assert.IsTrue(propertyTaxRetrieved.TaxRate == propertyTax.TaxRate);
                Assert.IsTrue(propertyTaxRetrieved.EffectiveDate == propertyTax.EffectiveDate);


            };


            database.ExecuteInTest(procedure);
         
            
        }


        [TestMethod]
        public void tax_should_insert_business_tax()
        {

            Action procedure = () =>
            {
                var businessTax = FakeTax.GetFakeBusinessTax();

                database.Insert(businessTax);

                var businessTaxRetrieved = database.RetrieveObject<BusinessTax>(b => b.Id == businessTax.Id);

                Assert.IsNotNull(businessTaxRetrieved);

                Assert.IsTrue(businessTaxRetrieved.TaxName == businessTax.TaxName);
                Assert.IsTrue(businessTaxRetrieved.CompanyName  == businessTax.CompanyName);
                Assert.IsTrue(businessTaxRetrieved.Contact== businessTax.Contact);
                Assert.IsTrue(businessTaxRetrieved.TaxDueDate == businessTax.TaxDueDate);
                Assert.IsTrue(businessTaxRetrieved.TaxableIncome == businessTax.TaxableIncome);
                Assert.IsTrue(businessTaxRetrieved.TaxRate == businessTax.TaxRate);
                Assert.IsTrue(businessTaxRetrieved.Waived  == businessTax.Waived);


            };


            database.ExecuteInTest(procedure);


        }


       
    }
}