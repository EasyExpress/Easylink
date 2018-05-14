
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Easylink;


namespace EasylinkApp.Tests
{

    [TestClass]
    public class BaseTest
    {

        protected IDatabase database;

        [TestInitialize]
        public void Test_Initialization()
        {


            var dbConfig = new DbConfig()
            {
                ConnectionString = @"Server=meng\sqlexpress;Database=EasylinkApp;User Id=easylink;Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo",
            };

            DatabaseFactory.Initialize(dbConfig);
           
            database = DatabaseFactory.Create();


        }

        





    }
}
