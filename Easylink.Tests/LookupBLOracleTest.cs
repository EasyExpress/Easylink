
using Easylink.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easylink.Tests
{
    /// <summary>
    ///     Summary description for EmployeeBLTest
    /// </summary>
    [TestClass]
    public class LookupBLOracleTest : LookupBLTest
    {
        [TestInitialize]
        public void Test_Initialization()
        {

            var dbConfig = new DbConfig()
            {
                ConnectionString =
                    string.Format("Data Source= " +
                                  "(DESCRIPTION =" +
                                  "(ADDRESS = (PROTOCOL = TCP)(HOST = {0} )(PORT = {1}))" +
                                  "(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};",
                                  "localhost", "1521", "xe", "easylink", "1197344"),
                DatabaseType = DatabaseType.Oracle,
                SchemaName = "EASYLINK",
                AuditRecordType = typeof(AuditRecord)
            };

            DatabaseFactory.Initialize(dbConfig);

            database = DatabaseFactory.Create();

            Mapping.SetNextId<Lookup>(NextIdOption.Sequence,"LOOKUP_ID_SEQ");
 
 

        }



        [TestMethod]
        public void oracle_lookup_should_be_able_to_insert()
        {
            lookup_should_be_able_to_insert();
        }
 

        
    }
}