using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Easylink
{
    public interface IDatabase
    {

        List<string> Sqls { get; }

        void ExecuteInTransaction(Action procedure);

        T ExecuteInTransaction<T>(Func<T> func) where T : new();

        void ExecuteInTest(Action procedure);

        List<T> RetrieveAll<T>() where T : new();


        List<T> RetrieveAll<T>(params Expression<Func<T, bool>>[] expressions) where T : new();

        T RetrieveObject<T>(params Expression<Func<T, bool>>[] expressions) where T : new();

        void DeleteAll<T>(params Expression<Func<T, bool>>[] expressions) where T : new();

        void Insert(object obj);

        void Update(object obj);

        void Delete(object obj);

        void Save(BusinessBase businessBase);

        dynamic RetrieveDynamic(string commandText);

        List<dynamic> RetrieveAll(string commandText);


        List<T> RetrieveAll<T>(string selectSql, Dictionary<string, object> parameters =null) where T : new();

        DataTable RetrieveTable(string commandText, Dictionary<string, object> parameters =null);

        void ExecuteNonQuery(string commandText, Dictionary<string, object> parameters = null);

        object ExecuteScalar(string commandText, Dictionary<string, object> parameters = null);

      

    }
}