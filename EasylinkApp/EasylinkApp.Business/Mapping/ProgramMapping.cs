
using Easylink;
 


namespace EasylinkApp.Business
{
    public   class  ProgramMapping :  Mapping 
    {

  
        public override IMappingConfig Setup()
        {

            var config = Class<Program>().ToTable("PROGRAM").NextId(NextIdOption.AutoIncrement);

            config.Property(c => c.Id).ToIdColumn("ID");
             config.Property(c => c.Name).ToColumn("NAME");

            return config; 
        }

       

    }

         
 }


  
