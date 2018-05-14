using System;
using System.Collections.Generic;
using Easylink.Tests.Business;
using Easylink.Tests.Model;

namespace Easylink.Tests
{
    internal static class FakeTax
    {
        public static PropertyTax GetFakePropertyTax()
        {
         
            return new PropertyTax()
                {
                     TaxName="PropertyTax",
                     City="Edmonton",
                     EffectiveDate= new DateTime(2012,10,12),
                     IsResidential = true,
                     PropertyCode ="2wdf12",
                     TaxPayable=2078.05m,
                     TaxRate = 0.034131
                };
        }


        public static BusinessTax GetFakeBusinessTax()
        {

            return new BusinessTax()
            {
                  TaxName = "BusinessTax",
                  CompanyName="Easy Express Solutions Inc.",
                  Contact= "Bryan",
                  TaxDueDate = new DateTime(2014, 10, 12),
                  TaxRate = 0.15,
                  TaxableIncome =200001,
                  Waived =false

            };
        }
    }
}