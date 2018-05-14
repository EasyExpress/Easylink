using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Easylink
{
    internal class PostgreSqlDatabase : Database
    {
        private static string _postgreSqlDateFormat = "yyyy-MM-dd";

        
       

        
    


        internal PostgreSqlDatabase( )
        {
            try
            {
     
                Connection = new NpgsqlConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new EasylinkException(
                    "Error occurred  while opening PostgreSql connection. \nMessage:" + ex.Message, ex);
            }
        }

        internal override void RenewConnection()
        {
            if (Connection == null)
            {
                Connection = new NpgsqlConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();

                Command = null;
            }
        }


        internal override EasylinkCommand CreateCommand()
        {
            var npgsqlCommand = new NpgsqlCommand
                {
                    Connection = Connection as NpgsqlConnection,
                    CommandType = CommandType.Text
                };

            return new EasylinkCommand(npgsqlCommand, DatabaseSetup.Instance.SchemaName);
        }


        internal override string FindNextIdSql(Type objectType)
        {
            string sequenceName = ClassConfigContainer.FindClassConfig(objectType).SequenceName;

            return string.Format("SELECT nextval('{0}') as NextId", sequenceName);

        }

        internal override string ParameterPrefix
        {
            get { return ":"; }
        }

        internal override void AddParametersToCommand(Dictionary<string, object> parameters)
        {
            Command.Parameters.Clear();


            foreach (var parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];

                if (parameterValue == null)
                {
                    Command.Parameters.Add(new NpgsqlParameter(parameterName, DBNull.Value));
                }
                else
                {
                    if (parameterValue.GetType().IsEnum)
                    {
                        parameterValue = parameterValue.ToString();
                    }
                    Command.Parameters.Add(new NpgsqlParameter(parameterName, parameterValue));
                }
            }
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
                    return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator, criteria.Value);
                }


                return string.Format("(UPPER({0}) {1} UPPER('{2}'))", criteria.ColumnName, criteria.Operator,
                                     criteria.Value);
            }


            if (criteria.Value is Guid)
            {
                return string.Format("({0}{1}'{2}')", criteria.ColumnName, criteria.Operator, criteria.Value);
            }


            if (criteria.Value is bool)
            {
                if ((bool) criteria.Value)
                {
                    return string.Format("({0}{1} 'Y')", criteria.ColumnName, criteria.Operator);
                }

                return string.Format("({0}{1} 'N')", criteria.ColumnName, criteria.Operator);
            }


            if (criteria.Value is DateTime)
            {
                var date = (DateTime) criteria.Value;


                return string.Format("({0}{1} to_date('{2}', '{3}'))", criteria.ColumnName, criteria.Operator,
                                     FormatDate(date), _postgreSqlDateFormat);
            }

            return string.Format("({0}{1}{2})", criteria.ColumnName, criteria.Operator, criteria.Value);
        }


        private string FormatDate(DateTime? date)
        {
            return date.Value.ToString(_postgreSqlDateFormat);
        }


        internal  override string CreateGreaterOrLessCondition(Criteria criteria)
        {
            if (criteria.Value is DateTime)
            {
                var date = (DateTime) criteria.Value;

                return string.Format("({0}{1} to_date('{2}', '{3}'))", criteria.ColumnName, criteria.Operator,
                                     FormatDate(date), _postgreSqlDateFormat);
            }

            return string.Format("({0}{1}{2})", criteria.ColumnName, criteria.Operator, criteria.Value);
        }


        internal  override string CreateLikeCondition(Criteria criteria)
        {
            switch (criteria.Operator)
            {
                case "StartsWith":

                    if (criteria.CaseSensitive)
                    {
                        return string.Format("({0} like '{1}%')", criteria.ColumnName, criteria.Value);
                    }

                    return string.Format("(UPPER({0}) like UPPER('{1}%'))", criteria.ColumnName, criteria.Value);


                case "EndsWith":

                    if (criteria.CaseSensitive)
                    {
                        return string.Format("({0} like '%{1}')", criteria.ColumnName, criteria.Value);
                    }

                    return string.Format("(UPPER({0}) like UPPER('%{1}'))", criteria.ColumnName, criteria.Value);


                case "Contains":

                    if (criteria.CaseSensitive)
                    {
                        return string.Format("({0} like '%{1}%')", criteria.ColumnName, criteria.Value);
                    }

                    return string.Format("(UPPER({0}) like UPPER('%{1}%'))", criteria.ColumnName, criteria.Value);
            }

            throw new EasylinkException("invalid criteria is found. {0} {1}", criteria.PropertyName, criteria.Operator);
        }
    }
}