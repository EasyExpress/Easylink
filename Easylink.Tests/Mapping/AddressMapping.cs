using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class AddressMapping : Mapping
    {
        public override IMappingConfig  Setup()
        {
            var config = Class<Address>().ToTable("ADDRESS");

            config.Property(c => c.Id).ToIdColumn("ID");
            config.Property(c => c.HouseNumber).ToColumn("HOUSE_NUMBER");
            config.Property(c => c.StreetName).ToColumn("STREET");
            config.Property(c => c.PostalCode).ToColumn("POSTAL_CODE");

            config.Property(c => c.Country.Id).ToColumn("COUNTRY_ID");

            return config;
        }
    }
}