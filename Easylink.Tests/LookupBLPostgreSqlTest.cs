﻿
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class LookupBLPostgreSqlTest : LookupBLTest
    {
        [TestInitialize]
        public void Test_Initialization()
        {
 

            var dbConfig = new DbConfig()
            {
                ConnectionString = "Server=localhost;Port=5432;Database=Easylink;Pooling =false; User Id=postgres;Password=1197344;",
                DatabaseType = DatabaseType.PostgreSql,
                SchemaName = "public",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);
			

            database = DatabaseFactory.Create();

            Mapping.SetNextId<Lookup>(NextIdOption.Sequence, "lookup_seq");
  
        }


        [TestMethod]
        public void postgre_sql_lookup_should_be_able_to_insert()
        {
            lookup_should_be_able_to_insert();
        }
 

        
    }
}