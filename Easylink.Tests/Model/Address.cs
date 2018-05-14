using System;

namespace Easylink.Tests.Model
{
    public class Address : BusinessBase
    {
        public Address()
        {
            Country = new Lookup();

            AddRules<Address>(a => a.HouseNumber, new RequiredRule());
        }

        public Int64? Id { get; set; }

        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

     
        public string PostalCode { get; set; }

        public Lookup Country { get; set; }

        
    }
}