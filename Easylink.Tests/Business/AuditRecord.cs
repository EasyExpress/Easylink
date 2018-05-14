namespace Easylink.Tests
{
    public class AuditRecord: AuditBase
    {

        public override  void PrepareBeforeInsert()
        {
            UserId = "dzhou";
        }
       
    }
}