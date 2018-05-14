using System;

using Easylink.Tests.Model;

namespace Easylink.Tests
{
    internal static class FakeInvoice
    {
        public static Invoice GetFakeInvoice()
        {

            return new Invoice
                {
                  
                   InvoiceNumber = "AA-DRS-12A",
                   InvoicedDate = DateTime.Now,
                   InvoicedBy ="Daniel Zhou",
                   Total = 34.61m
                   
                };
        }

    }

}