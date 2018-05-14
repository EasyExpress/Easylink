namespace Easylink
{
    public enum DatabaseType
    {
        Oracle,
        SqlServer,
        MySql,
        PostgreSql
    };

    public  enum Aggregate
    {
         Sum,
         Avg,
         Min,
         Max,
         Count,
         None
         
    }

    public  enum  NextIdOption
    {
       None,
       AutoIncrement,
       Sequence
    }

    internal  enum   CriteriaNodeType 
    {
       AND,
       OR

    }
}