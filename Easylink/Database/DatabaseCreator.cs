using System;
using System.ComponentModel;

namespace Easylink
{
   
    internal static class DatabaseCreator
    {
        internal static IDatabase CreateDatabase()
        {
           

            if (string.IsNullOrEmpty(DatabaseSetup.Instance.ConnectionString) || string.IsNullOrEmpty(DatabaseSetup.Instance.SchemaName))
            {
                throw new EasylinkException(
                    "Please set up database connection string or schema name using DatabaseSetup.");
            }


            if (DatabaseSetup.Instance.DatabaseType == DatabaseType.Oracle)
            {
                return new OracleDatabase();
            }

            if (DatabaseSetup.Instance.DatabaseType == DatabaseType.SqlServer)
            {
                return new SqlServerDatabase();
            }


            if (DatabaseSetup.Instance.DatabaseType == DatabaseType.MySql)
            {
                return new MySqlDatabase();
            }

            if (DatabaseSetup.Instance.DatabaseType == DatabaseType.PostgreSql)
            {
                return new PostgreSqlDatabase();
            }

          

            throw new EasylinkException("Database {0} can not be created.",DatabaseSetup.Instance.DatabaseType);
        }
    }
}