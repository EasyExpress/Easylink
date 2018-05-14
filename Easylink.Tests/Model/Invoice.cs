using System;

namespace Easylink.Tests.Model
{
    public class Invoice : BusinessBase
    {
        public int  Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime? InvoicedDate { get; set; }

        public string InvoicedBy { get; set; }

        public decimal Total { get; set; }



    }
}