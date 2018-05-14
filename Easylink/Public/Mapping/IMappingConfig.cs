 


using System;
using System.Collections.Generic;

namespace Easylink 
{
   public interface IMappingConfig
    {
          List<PropertyConfig> Properties { get;   }

          List<LinkConfig> Links { get;   }
 
          string TableName { get;   }

          NextIdOption NextIdOption { get; }

          string SequenceName { get;  }

          Type  Type { get; }

          bool Auditable { get; }
 
    }
}
