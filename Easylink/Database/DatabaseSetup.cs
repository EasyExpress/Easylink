using System;

namespace Easylink
{
    internal class DatabaseSetup
    {
        private static DatabaseSetup _databaseSetup;

        internal  static DatabaseSetup Instance
        {
            get { return _databaseSetup; }
        }


       internal  string ConnectionString { get;  private set; }

       internal  string SchemaName { get;  private set; }

       internal  DatabaseType DatabaseType { get; private set; }

       internal Type AuditRecordType { get; set;  }

       private static bool  _isTaskComplete_CheckIfAuditTableExists = false; 

     
       
        internal static void Initialize(DbConfig dbConfig)
        {
            _databaseSetup = new DatabaseSetup
                {
                    ConnectionString = dbConfig.ConnectionString,
                    DatabaseType = dbConfig.DatabaseType,
                    SchemaName = dbConfig.SchemaName,
                    AuditRecordType = dbConfig.AuditRecordType
                };

            if (dbConfig.AuditRecordType != null && _isTaskComplete_CheckIfAuditTableExists == false)
             {
                 CheckIfAuditTableExists(dbConfig.AuditRecordType, dbConfig.DatabaseType);

                 _isTaskComplete_CheckIfAuditTableExists = true; 
             }
        }



        private static void CheckIfAuditTableExists(Type auditRecordType, DatabaseType  databaseType)
        {
            var classConfig = ClassConfigContainer.FindClassConfig(auditRecordType);

            var table = classConfig.TableName;

            var sql = string.Empty;

            if (databaseType != DatabaseType.Oracle)
            {
                sql = string.Format("SELECT COUNT(*) " +
                                    "FROM INFORMATION_SCHEMA.TABLES " +
                                    "WHERE TABLE_SCHEMA = '[Schema]'   AND  TABLE_NAME = '{0}'  OR   TABLE_NAME = '{1}' ",
                                    table.ToLower(), table);

            }
            else
            {
                sql = string.Format("SELECT COUNT(*) FROM  ALL_TABLES  WHERE OWNER ='[Schema]'  AND TABLE_NAME ='{0}'",
                                    table);
            }
         
        
            var database = DatabaseCreator.CreateDatabase();

            var result = database.ExecuteScalar(sql);

            if (result.ToString() == "0")
            {
                throw new EasylinkException("Audit table {0} does not exist in the database.", table);
            }

        }



    }
}