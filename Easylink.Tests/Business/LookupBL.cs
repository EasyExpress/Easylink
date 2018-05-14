using System;
using System.Collections.Generic;
using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class LookupBL : BaseBL
    {
        public LookupBL(IDatabase database)
        {
            Database = database;
        }

        public List<Lookup> RetrieveAll()
        {
            return Database.RetrieveAll<Lookup>();
        }


        public Lookup Insert(Lookup lookup)
        {
            Database.Insert(lookup);

            return lookup; 
        }

        public List<Lookup> RetrieveLookupsByGroupName(string group)
        {
            var parentLookup = RetrieveLookupByName(group);

            return Database.RetrieveAll<Lookup>(l => l.ParentId == parentLookup.Id);
        }


        public Lookup RetrieveLookupByName(string name)
        {
            return Database.RetrieveObject<Lookup>(l => l.Name==name);
        }


        public Lookup RetrieveLookupById(Int64 id)
        {
            return Database.RetrieveObject<Lookup>(l => l.Id== id);
        }
    }
}