using System;
using System.Collections.Generic;
 
using Easylink;
 


namespace EasylinkApp.Business
{
    public  class LookupBL  :BaseBL 
    {


        public LookupBL(IDatabase database)
        {
            Database = database; 
        }

       
        public List<Lookup> RetrieveAll()
        {

            return   Database.RetrieveAll<Lookup>();
 
        }


        public List<Lookup> RetrieveLookupsByGroupName(string group)
        {
            var groupLookup =  Database.RetrieveObject<Lookup>(l=>l.Name==group,l=>l.ParentId == null);

            return Database.RetrieveAll<Lookup>(l=>l.ParentId== groupLookup.Id);
        }


        public Lookup RetrieveLookupByName(string name)
        {

            return Database.RetrieveObject<Lookup>( l=>l.Name== name);

        }



        public Lookup RetrieveLookupById(Int64 id)
        {

            return Database.RetrieveObject<Lookup>(l=>l.Id==id);

        }
    }

         
 }
 
