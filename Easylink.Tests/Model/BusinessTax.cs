using System;

namespace Easylink.Tests.Model
{
    
    public class BusinessTax : Tax
    {
        public string CompanyName { get; set;  }
        
        public string  Contact { get; set;  }

        public DateTime? TaxDueDate { get; set;  }

        public decimal?  TaxableIncome { get; set; }

        public double?   TaxRate { get; set; }

        public bool?   Waived  { get; set;  }
    }
}