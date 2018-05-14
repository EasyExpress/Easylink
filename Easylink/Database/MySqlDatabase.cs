using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Easylink
{
    internal class MySqlDatabase : Database
    {


       

        internal MySqlDatabase( )
        {
            try
            {

                Connection = new MySqlConnection(DatabaseSetup.Instance.ConnectionString);

                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new EasylinkException("Error occurred  while opening My Sql connection. \nMessage:" + ex.Message,
                                            ex);
            }
        }


        internal override void RenewConnection()
        {
            if (Connection == null)
            {
                Connection = new MySqlConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();

                Command = null;
            }
        }


        internal override EasylinkCommand CreateCommand()
        {
            var mySqlCommand = new MySqlCommand
                {
                    Connection = Connection as MySqlConnection,
                    CommandType = CommandType.Text
                };


            return new EasylinkCommand(mySqlCommand, DatabaseSetup.Instance.SchemaName);
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


            foreach (var parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];

                if (parameterValue == null)
                {
                    Command.ToMySqlCommand().Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    Command.ToMySqlCommand().Parameters.AddWithValue(parameterName, parameterValue);
                }
            }
        }


        internal override string BeforeInsertingRecord(string commandText)
        {
            return string.Format("{0};{1}", commandText, "SELECT last_insert_id()");
        }


        internal  override string CreateEqualOrNonEqualCondition(Criteria criteria)
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
                    return string.Format("(BINARY {0}{1}'{2}')", criteria.ColumnName, criteria.Operator,
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
            string condition;

            switch (criteria.Operator)
            {
                case "StartsWith":
                    condition = string.Format("({0} like '{1}%')", criteria.ColumnName, criteria.Value);
                    break;


                case "EndsWith":
                    condition = string.Format("({0} like '%{1}')", criteria.ColumnName, criteria.Value);
                    break;

                case "Contains":
                    condition = string.Format("({0} like '%{1}%')", criteria.ColumnName, criteria.Value);
                    break;

                default:
                    throw new EasylinkException("invalid criteria is found. {0} {1}", criteria.PropertyName,
                                                criteria.Operator);
            }

            if (criteria.CaseSensitive)
            {
                condition = condition.Replace("(", "(BINARY ");
            }


            return condition;
        }
    }
}