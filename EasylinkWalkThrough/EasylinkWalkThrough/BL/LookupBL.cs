 
using Easylink;
 

namespace EasylinkWalkThrough
{
    public class LookupBL :BaseBL
    {

        public LookupBL(IDatabase database)
        {
            Database = database; 
        }

        public Lookup RetrieveLookupByName(string name)
        {
            return Database.RetrieveObject<Lookup>(l=>l.Name== name);

        }

    }
}
