using System;




namespace EasylinkApp.Business
{

   public  class  AuditRecord 
    {

        public string RecordType { get; set; }

        public string RecordId { get; set; }
  
        public string UserId { get; set; }

        public string Description { get; set; }

        public string Operation { get; set; }

        public DateTime TimeStamp { get; set; }
 

    }
}
