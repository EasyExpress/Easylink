using System;
 
using Easylink;

namespace EasylinkWalkThrough
{
    public class Lookup : BusinessBase
    {

        public Int64 Id { get; set; }

        public Int64? ParentId { get; set; }

        public string Name { get; set; }

    }
}
