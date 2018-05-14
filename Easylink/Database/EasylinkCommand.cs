using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace Easylink
{
    internal enum CommandStatus
    {
        Before,
        Executing,
        Executed
    };

    internal class EasylinkCommand
    {
        private readonly DbCommand _command;

        private readonly string _schemaName;

        internal List<string> Sqls;
       

        internal EasylinkCommand(DbCommand command, string schemaName)
        {
            _command = command;

            _schemaName = schemaName;

            Sqls = new List<string>();

        }

        internal string CommandText
        {
            get { return _command.CommandText; }

            set { _command.CommandText = value; }
        }

        internal DbParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }


        internal CommandStatus Status { get; private set; }

        public void SetTransaciton(DbTransaction transaction)
        {
            _command.Transaction = transaction;
        }


        internal int ExecuteNonQuery()
        {
            BeforeExecutingCommand();

            
            int result = _command.ExecuteNonQuery();

            Sqls.Add(CommandText);

            AfterCommandExecuted();

            return result;
        }


        internal DbDataReader ExecuteReader()
        {
            BeforeExecutingCommand();

            DbDataReader result = _command.ExecuteReader();

            Sqls.Add(CommandText);

            AfterCommandExecuted();

            return result;
        }


        internal object ExecuteScalar()
        {
            BeforeExecutingCommand();

            object result = _command.ExecuteScalar();

            Sqls.Add(CommandText);

            AfterCommandExecuted();

            return result;
        }

        internal void Start()
        {
            Status = CommandStatus.Before;
        }

        internal OracleCommand ToOracleCommand()
        {
            return _command as OracleCommand;
        }


        internal MySqlCommand ToMySqlCommand()
        {
            return _command as MySqlCommand;
        }


        internal SqlCommand ToSqlCommand()
        {
            return _command as SqlCommand;
        }


        private void BeforeExecutingCommand()
        {
            _command.CommandText = _command.CommandText.Replace("[Schema]", _schemaName);

            Status = CommandStatus.Executing;
        }

        private void AfterCommandExecuted()
        {
            Status = CommandStatus.Executed;
        }
    }
}