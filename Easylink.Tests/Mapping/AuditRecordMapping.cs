namespace Easylink.Tests.Business
{
    public class AuditRecorMappping : Mapping
    {
        public override IMappingConfig Setup()
        {

            var config = Class<AuditRecord>().ToTable("SYS_AUDIT");

            config.Property(a => a.AuditId).ToIdColumn("AUDIT_ID");
            config.Property(a => a.UserId).ToColumn("USER_ID");
            config.Property(a => a.Description).ToColumn("DESCRIPTION");
            config.Property(a => a.TableName).ToColumn("TABLE_NAME");
            config.Property(a => a.RecordId).ToColumn("RECORD_ID");
            config.Property(a => a.Operation).ToColumn("OPERATION");
            config.Property(a => a.TimeStamp).ToColumn("TIMESTAMP");

            return config;
        }
    }
}


 