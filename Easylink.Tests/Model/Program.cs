using System;

namespace Easylink.Tests.Model
{

    public class Program :BusinessBase
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public Program()
        {

            AddRules<Program>(p => p.Id, "Id", new RequiredRule(-1));
                                             
        }

    }
            
           
}