using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Easylink
{
    internal class SqlServerDatabase : Database
    {

 
        internal SqlServerDatabase( )
        {
            try
            {
                
                Connection = new SqlConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new EasylinkException(
                    "Error occurred  while opening Sql Server connection. \nMessage:" + ex.Message, ex);
            }
        }


        internal override void RenewConnection()
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();

                Command = null;
            }
        }

        internal override EasylinkCommand CreateCommand()
        {
            var sqlCommand = new SqlCommand {Connection = Connection as SqlConnection, CommandType = CommandType.Text};


            return new EasylinkCommand(sqlCommand,DatabaseSetup.Instance.SchemaName);
        }


        internal override string FindNextIdSql(Type objectType)
        {
            string sequenceName = ClassConfigContainer.FindClassConfig(objectType).SequenceName;

            return string.Format("SELECT Next VALUE FOR [Schema].{0} as NextId", sequenceName);

        }

        internal override string ParameterPrefix
        {
            get { return "@"; }
        }


        internal override void AddParametersToCommand(Dictionary<string, object> parameters)
        {
            Command.Parameters.Clear();

            if (parameters == null) return;

            foreach (var parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];

                if (parameterValue == null)
                {
                    Command.ToSqlCommand().Parameters.Add(parameterName, DBNull.Value);
                }
                else
                {
                    Command.Parameters.Add(new SqlParameter(parameterName, parameterValue));
                }
            }
        }


        internal override string BeforeInsertingRecord(string commandText)
        {
            return string.Format("{0};{1}", commandText, "SELECT CAST(scope_identity() AS int)");
        }


        internal override string CreateEqualOrNonEqualCondition(Criteria criteria)
        {
            if (criteria.Value == null)
            {
                if (criteria.Operator == "!=")
                {
                    return string.Format("({0} is not null)", criteria.ColumnName);
                }

                return string.Format("({0} is null)", criteria.ColumnName);
            }

            if (criteria.Value is string)
            {
                if (criteria.CaseSensitive)
                {
                    return string.Format("(convert(varbinary, {0}) {1} convert(varbinary, '{2}') )",
                                         criteria.ColumnName, criteria.Operator,
                                         criteria.Value);
                }

                return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator, criteria.Value);
            }


            if (criteria.Value is Guid)
            { 
                return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator, criteria.Value);
            }

            if (criteria.Value is bool)
            {
                if ((bool) criteria.Value)
                {
                    return string.Format("({0}{1} 1)", criteria.ColumnName, criteria.Operator);
                }

                return string.Format("({0}{1} 0)", criteria.ColumnName, criteria.Operator);
            }


            if (criteria.Value is DateTime)
            {
                var date = (DateTime) criteria.Value;

                return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator,
                                     date.ToString("yyyy-MM-dd"));
            }

            return string.Format("({0}{1}{2})", criteria.ColumnName, criteria.Operator, criteria.Value);
        }


        internal  override string CreateGreaterOrLessCondition(Criteria criteria)
        {
            if (criteria.Value is DateTime)
            {
                var date = (DateTime) criteria.Value;

                return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator,
                                     date.ToString("yyyy-MM-dd"));
            }

            return string.Format("({0}{1}{2})", criteria.ColumnName, criteria.Operator, criteria.Value);
        }


        internal  override string CreateLikeCondition(Criteria criteria)
        {
            if (criteria.CaseSensitive)
            {
                throw new EasylinkException("Sql server does not support  case senstive like search at this time.");
            }
            switch (criteria.Operator)
            {
                case "StartsWith":
                    return string.Format("({0} like '{1}%')", criteria.ColumnName, criteria.Value);


                case "EndsWith":
                    return string.Format("({0} like '%{1}')", criteria.ColumnName, criteria.Value);

                case "Contains":
                    return string.Format("({0} like '%{1}%')", criteria.ColumnName, criteria.Value);
            }

            throw new EasylinkException("invalid criteria is found. {0} {1}", criteria.PropertyName, criteria.Operator);
        }
    }
}