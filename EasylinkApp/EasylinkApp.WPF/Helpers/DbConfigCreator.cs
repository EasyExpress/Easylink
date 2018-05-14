
using Easylink;

namespace EasylinkApp.WPF
{
    class DbConfigCreator: IDbConfigCreator
    {
        public DbConfig Create()
        {
            return  new DbConfig()
            {
                ConnectionString = @"Server=meng;Database=EasylinkApp;User Id=demo;Password=password;",
                DatabaseType = DatabaseType.SqlServer,
                SchemaName = "dbo"

            };

 
        }
    }
}
