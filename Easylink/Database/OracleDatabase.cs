using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Easylink
{
    internal class OracleDatabase : Database
    {
        private static string _oracleDateFormat = "yyyy-MM-dd";


        internal override string ParameterPrefix
        {
            get { return ":";  }
        }


        internal override string FindNextIdSql(Type objectType)
        {
            string sequenceName = ClassConfigContainer.FindClassConfig(objectType).SequenceName;

            return string.Format("Select [Schema].{0}.NEXTVAL As NextId From DUAL", sequenceName);

          
        }


             
       internal OracleDatabase( )
        {
            try
            {
 
                Connection = new OracleConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();
            }
            catch (Exception ex)
            {
                throw new EasylinkException("Error occurred  while opening Oracle connection. \nMessage:" + ex.Message,
                                            ex);
            }
        }


        internal override void RenewConnection()
        {
            if (Connection == null)
            {
                Connection = new OracleConnection(DatabaseSetup.Instance.ConnectionString);
                Connection.Open();

                Command = null;
            }
        }


        internal override EasylinkCommand CreateCommand()
        {
            var oracleCommand = new OracleCommand
                {
                    Connection = Connection as OracleConnection,
                    BindByName = true,
                    CommandType = CommandType.Text
                };

            return new EasylinkCommand(oracleCommand, DatabaseSetup.Instance.SchemaName);
        }


        internal override void AddParametersToCommand(Dictionary<string, object> parameters)
        {
            Command.Parameters.Clear();


            foreach (var parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];

                if (parameterValue == null)
                {
                    Command.ToOracleCommand().Parameters.Add(parameterName, DBNull.Value);
                }
                else
                {
                    OracleDbType dbType = GetDbType(parameterValue.GetType());
                    object dbValue = GetDbValue(parameterValue);

                    Command.ToOracleCommand().Parameters.Add(parameterName, dbType, dbValue, ParameterDirection.Input);
                }
            }
        }


        // private methods 
        private static OracleDbType GetDbType(Type type)
        {
            if (type == typeof (int)) return OracleDbType.Int32;

            if (type == typeof (Int64)) return OracleDbType.Int64;

            if (type == typeof (decimal)) return OracleDbType.Decimal;


            if (type == typeof (DateTime)) return OracleDbType.Date;

            if (type == typeof (bool)) return OracleDbType.Varchar2;

            if (type == typeof (string)) return OracleDbType.Varchar2;

            if (type.IsEnum) return OracleDbType.Varchar2;


            //nullable 

            if (type == typeof (int?)) return OracleDbType.Int32;

            if (type == typeof (Int64?)) return OracleDbType.Int64;

            if (type == typeof (decimal?)) return OracleDbType.Decimal;

            if (type == typeof (DateTime?)) return OracleDbType.Date;

            if (type == typeof (bool?)) return OracleDbType.Varchar2;

            if (type == typeof(Guid)) return OracleDbType.Varchar2;

            throw new EasylinkException("No db type is matched with  type  {0} ", type);
        }


        private static object GetDbValue(object value)
        {
            if (value == null) return DBNull.Value;

            if (value is bool)
            {
                if ((bool) value) return 'Y';

                return 'N';
            }

            return value;
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
                                     FormatDate(date), _oracleDateFormat);
            }

            return string.Format("({0}{1}{2})", criteria.ColumnName, criteria.Operator, criteria.Value);
        }


        private string FormatDate(DateTime? date)
        {
            return date.Value.ToString(_oracleDateFormat);
        }


        internal  override string CreateGreaterOrLessCondition(Criteria criteria)
        {
            if (criteria.Value is DateTime)
            {
                var date = (DateTime) criteria.Value;

                return string.Format("({0}{1} to_date('{2}', '{3}'))", criteria.ColumnName, criteria.Operator,
                                     FormatDate(date), _oracleDateFormat);
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