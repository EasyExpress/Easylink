 

using System;

namespace Easylink.Tests.Model
{
    
    public class PropertyTax : Tax
    {
        public string City { get; set;  }
        
        public string PropertyCode { get; set;  }

        public DateTime? EffectiveDate { get; set;  }

        public decimal?  TaxPayable { get; set; }

        public double?   TaxRate { get; set; }

        public bool? IsResidential { get; set;  }
    }
}