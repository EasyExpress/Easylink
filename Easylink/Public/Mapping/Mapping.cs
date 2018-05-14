 

namespace Easylink
{
    public  abstract class Mapping
    {
      

        public MappingConfig<T> Class<T>()  where T: new()
        {
            return  new MappingConfig<T>();
        }

   
        public abstract IMappingConfig  Setup();

        
        public static void SetNextId<T>(NextIdOption nextIdOption, string sequenceName=null)
        {
            var classConfig = ClassConfigContainer.FindClassConfig1<T>();

            classConfig.SetNextId(nextIdOption, sequenceName);
        }

        
      
    }
}