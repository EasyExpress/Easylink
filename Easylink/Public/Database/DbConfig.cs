using System;
 
namespace Easylink
{
    public class DbConfig
    {
        public string ConnectionString { get; set; }

        public string SchemaName { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public Type AuditRecordType { get; set; }

 
    }
}
