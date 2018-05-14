using System;

namespace Easylink
{
    public class AuditBase
    {
        public  Int64  AuditId { get; set; }

        public string TableName { get;  set; }

     
        public string RecordId { get;   set; }

        public string UserId { get; set; }

        public string Description { get;  set; }

        public string Operation { get;   set; }

        public DateTime TimeStamp { get;  set; }

        public object ObjectToAudit { get;  set; }

        public virtual void PrepareBeforeInsert()
        {
            
        }
    }
}