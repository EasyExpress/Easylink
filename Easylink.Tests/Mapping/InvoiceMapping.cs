using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class InvoiceMapping : Mapping
    {
        public override IMappingConfig Setup()
        {

            var config = Class<Invoice>().ToTable("INVOICE").EnableAudit();
                
 
            config.Property(i => i.Id).ToIdColumn("Id");
          
            config.Property(i => i.InvoiceNumber).ToColumn("INVOICE_NUMBER");
            config.Property(i => i.InvoicedDate).ToColumn("INVOICED_DATE");
            config.Property(i => i.InvoicedBy).ToColumn("INVOICED_BY");
            config.Property(i => i.Total).ToColumn("Total");
       
            return config;

        }
    }
}