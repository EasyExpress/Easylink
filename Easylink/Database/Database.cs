using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

using System.Collections.Generic;
using System.Runtime.Remoting;


namespace Easylink
{
    internal abstract class Database : IDatabase
    {
        private EasylinkCommand _command;

        internal abstract string ParameterPrefix { get;  }
       

        internal EasylinkCommand Command
        {
            get
            {
                RenewConnection();

                if (_command == null)
                {
                    _command = CreateCommand();
                }

                _command.SetTransaciton(_transaction);

                return _command;
            }
            set { _command = value; }
        }


        public List<string> Sqls
        {
            get
            {
                if (_command == null)
                {
                    return new List<string>();
                }

                return _command.Sqls;
            }
        }

     
        #region Transaction 

        internal DbConnection Connection;

        private int _level;
        private DbTransaction _transaction;

      

        public void ExecuteInTransaction(Action procedure)
        {
            try
            {
                BeginTransaction();

                procedure();

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();

                throw;
            }
            finally
            {
                Close();
            }
        }

        public T ExecuteInTransaction<T>(Func<T> func) where T : new()
        {
            try
            {
                BeginTransaction();

                T result = func();

                CommitTransaction();

                return result;
            }
            catch
            {
                RollbackTransaction();

                throw;
            }

            finally
            {
                Close();
            }
        }


        public void ExecuteInTest(Action procedure)
        {
            try
            {
                BeginTransaction();

                procedure();
            }

            finally
            {
                RollbackTransaction();
                Close();
            }
        }

        internal void BeginTransaction()
        {
            if (_level == 0)
            {
                RenewConnection();

                if (_command != null)
                {
                    _command.Sqls = new List<string>();
                }

                _transaction = Connection.BeginTransaction();
            }

            _level++;
        }

        internal void CommitTransaction()
        {
            if (_level == 1)
            {
                _transaction.Commit();
            }


            _level--;
        }

        internal void RollbackTransaction()
        {
            if (_level == 1)
            {
                _transaction.Rollback();
            }
            _level--;
        }

        internal void Close()
        {
            if (_level == 0)
            {
                _transaction = null;

                if (Connection != null)
                {
                    Connection.Close();
                }

                Connection = null;
            }
        }

        #endregion

        public List<dynamic> RetrieveAll(string commandText)
        {
            try
            {
                var result = new List<dynamic>();

                var  table = RetrieveTable(commandText);

                foreach (DataRow row in table.Rows)
                {
                    var obj = new ExpandoObject();

                    var objDictionary = (IDictionary<string, object>) obj;

                    foreach (DataColumn column in table.Columns)
                    {
                        if (row[column.ColumnName] == DBNull.Value)
                        {
                            objDictionary[column.ColumnName] = null;
                        }
                        else
                        {
                            objDictionary[column.ColumnName] = row[column.ColumnName];
                        }
                    }

                    result.Add(obj);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }


        public dynamic RetrieveDynamic(string commandText)
        {
            try
            {
                var  all = RetrieveAll(commandText);

                return all[0];
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }


        public DataTable RetrieveTable(string commandText, Dictionary<string, object> parameters =null)
        {
            try
            {
                Command.Start();

                if (parameters != null)
                {
                    commandText = CreateSqlTextByParameters(commandText, parameters);

                }

                Command.CommandText = EnclosePropertyInQuotation(commandText);

                var  reader = Command.ExecuteReader();

                var table = new DataTable();

                table.Load(reader);

                return table;
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }

        public void ExecuteNonQuery(string commandText, Dictionary<string, object> sqlParameters)
        {
            try
            {
                Command.Start();

                Command.CommandText = commandText;

                if (sqlParameters != null)
                {               
                    AddParametersToCommand(sqlParameters);
                }

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }

        public object ExecuteScalar(string commandText, Dictionary<string, object> parameters = null)
        {
            try
            {
                Command.Start();

                Command.CommandText = commandText;

                if (parameters != null)
                {
                    AddParametersToCommand(parameters);
                }

                object result = Command.ExecuteScalar();

                if (result == null) return 0;

                return result;
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }


        public T RetrieveObject<T>(params Expression<Func<T, bool>>[] expressions) where T : new()
        {
            var criteriaNode = Expressions.CreateCriteriaNode(expressions);

            return RetrieveObject<T>(criteriaNode);
        }


        public List<T> RetrieveAll<T>(params Expression<Func<T, bool>>[] expressions) where T : new()
        {
            var criteriaNode = Expressions.CreateCriteriaNode(expressions);

            return RetrieveAll<T>(criteriaNode);
        }


        public List<T> RetrieveAll<T>() where T : new()
        {
            return RetrieveAll<T>(new CriteriaNode(null));
        }



    

       

        public void DeleteAll<T>(params Expression<Func<T, bool>>[] expressions) where T : new()
        {
            var criteriaNode = Expressions.CreateCriteriaNode(expressions);

            DeleteAll<T>(criteriaNode);
        }


       

        private List<T> RetrieveAll<T>(CriteriaNode criteriaNode) where T : new()
        {
            var selectSql = ClassConfigContainer.FindSelectSql<T>();

            var criterias = criteriaNode.SearchAll();
 
            if (criterias.Count > 0)
            {
                 UpdateCriteria<T>(criterias);
            }

            var table = RetrieveTable(selectSql, criteriaNode);

            return ObjectMapper.MapTableToList<T>(table);
        }



        private T RetrieveObject<T>(CriteriaNode criteriaNode) where T : new()
        {
            var list = RetrieveAll<T>(criteriaNode);

            if (list.Count == 0)
            {
                return default(T);
            }

            if (list.Count > 1)
            {
                throw new ServerException("More than one objects matched with criteria are retrieved.");
            }

            return list[0];
        }

        private void DeleteAll<T>(CriteriaNode criteriaNode) where T : new()
        {
             
            var deleteSql = ClassConfigContainer.FindDeleteSql(typeof(T));
            try
            {
 
                Command.Start();

                var criterias = criteriaNode.SearchAll();

                if (criterias.Count > 0)
                {
                    UpdateCriteria<T>(criterias);
                }

                if (Shared.IsAuditable<T>())
                {
                    var originalObjects = RetrieveAll<T>(criteriaNode);

                    var dict = new Dictionary<T, string>();

                    foreach (var original in originalObjects)
                    {

                        string updateSql = ClassConfigContainer.FindUpdateSql1(typeof(T));

                        var propertyParameters =
                            ParametersCreator.CreatePropertyParameters(ParameterPrefix,
                                                                       original, ref updateSql);

                        string auditText = Auditor.AuditDelete(original, propertyParameters);

                        dict[original] = auditText;


                    }

                    ExecuteNonQuery(deleteSql, criteriaNode);

                    foreach (var original in originalObjects)
                    {
                        InsertAuditRecord(original, DbOperation.Delete, dict[original]);
                    }

                }
                else
                {

                    ExecuteNonQuery(deleteSql, criteriaNode);
                }


            }

            catch (Exception ex)
            {
                throw CreateEasylinkException(deleteSql, ex);
            }
        }

     
        public List<T> RetrieveAll<T>(string selectSql, Dictionary<string, object> parameters =null) where T : new()
        {
            var table = RetrieveTable(selectSql, parameters);

            return ObjectMapper.MapTableToList<T>(table);
        }



        public void Insert(object obj)
        {
            string insertSql = ClassConfigContainer.FindInsertSql(obj.GetType());

            try
            {
                var classConfig = ClassConfigContainer.FindClassConfig2(obj);

                if (classConfig.NextIdOption == NextIdOption.Sequence)
                {
                    if (string.IsNullOrEmpty(classConfig.SequenceName))
                    {
                        throw new EasylinkException(
                            "Class {0} mapping NextId option is Sequence, but no sequence name is found.",
                            obj.GetType().Name);

                    }

                    SetObjectIdBeforeInsert(obj);
                }

             
                Command.Start();

                var propertyParameters =
                    ParametersCreator.CreatePropertyParameters(ParameterPrefix, obj, ref insertSql);

                var sqlParameters = ConvertPropertyParametersToSqlParameters(propertyParameters);


                insertSql = BeforeInsertingRecord(insertSql);


                var result = ExecuteScalar(insertSql, sqlParameters);

                if (classConfig.NextIdOption == NextIdOption.AutoIncrement)
                {
                    string idPropertyName;

                    var id = SetObjectIdAfterInsert(obj, result, out idPropertyName);

                    propertyParameters.Add(idPropertyName, id);

                }
 

                if (Shared.IsAuditable(obj))
                {

                    string auditText = Auditor.AuditInsert(obj, propertyParameters);


                    InsertAuditRecord(obj, DbOperation.Insert, auditText);
                }
            }

            catch (Exception ex)
            {
                throw CreateEasylinkException(insertSql, ex);
            }
        }


        public void Update(object obj)
        {
         
            var id = GetObjectId(obj);

            if (id == null || id.ToString() == "0")
            {
                return; 
            }
             
             
            var updateSql = ClassConfigContainer.FindUpdateSql1(obj.GetType());
           
            try
            {
                Command.Start();
               

                var  propertyParameters =
                    ParametersCreator.CreatePropertyParameters(ParameterPrefix, obj, ref updateSql);

               
                var  sqlParameters = ConvertPropertyParametersToSqlParameters(propertyParameters);


 

                var  changes = CompareToOriginal(obj, propertyParameters);

            
                if (changes.Keys.Count > 0)
                {
                    ExecuteNonQuery(updateSql, sqlParameters);

                    if (Shared.IsAuditable(obj))
                    {
                       var  auditText = Auditor.AuditUpdate(obj, changes);

                        InsertAuditRecord(obj, DbOperation.Update, auditText);
                    }
                }
            }

            
            
            catch (Exception ex)
            {
                throw CreateEasylinkException(updateSql, ex);
            }
        }


        public void Delete(object obj)
        {
       

            if (obj == null) return;

            var  deleteSql = ClassConfigContainer.FindDeleteSql1(obj.GetType());

            try
            {
                Command.Start();

                var  propertyParameters =
                    ParametersCreator.CreatePropertyParameters(ParameterPrefix, obj, ref deleteSql);

                var  sqlParameters = ConvertPropertyParametersToSqlParameters(propertyParameters);

                var original = GetOriginal(obj);

                ExecuteNonQuery(deleteSql, sqlParameters);

                var businessBase = obj as BusinessBase;

                if (businessBase != null)
                {
                    businessBase.MarkAsDeleted();
                }

                if (Shared.IsAuditable(obj))
                {
                    string updateSql = ClassConfigContainer.FindUpdateSql1(obj.GetType());

                    propertyParameters = ParametersCreator.CreatePropertyParameters(ParameterPrefix,
                                                                                    original, ref updateSql);


                    string auditText =  Auditor.AuditDelete(original, propertyParameters);

                    InsertAuditRecord(original, DbOperation.Delete, auditText);
                }

                NullifyIdProperty(obj);


            }

            catch (Exception ex)
            {
                throw CreateEasylinkException(deleteSql, ex);
            }
        }


        public void Save(BusinessBase businessBase)
        {
             businessBase.ValidateRules();

            if (businessBase.LifeCycleStatus == BusinessBaseLifeCycle.New)
            { 
                 Insert(businessBase);
            }

            else if (businessBase.LifeCycleStatus == BusinessBaseLifeCycle.Deleted)
            {
                  Delete(businessBase);
            }

            else if (businessBase.LifeCycleStatus == BusinessBaseLifeCycle.Old)
            {

                Update(businessBase);
              
                   
            }
       }


        private  void ExecuteNonQuery(string commandText, CriteriaNode criteriaNode)
        {
            try
            {
                Command.Start();

                Command.CommandText = CreateSqlTextByCriteriaNode(commandText, criteriaNode);

                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }

     
        internal abstract EasylinkCommand CreateCommand();

        internal abstract void RenewConnection();

        internal abstract void AddParametersToCommand(Dictionary<string, object> parameters);

        internal  abstract string CreateEqualOrNonEqualCondition(Criteria criteria);

        internal  abstract string CreateGreaterOrLessCondition(Criteria criteria);

        internal abstract string CreateLikeCondition(Criteria criteria);


        internal virtual string BeforeInsertingRecord(string commandText)
        {
            return commandText;
        }



     
        private DataTable RetrieveTable(string commandText, CriteriaNode criteriaNode)
        {
            try
            {
                Command.Start();


                commandText = CreateSqlTextByCriteriaNode(commandText, criteriaNode);

                Command.CommandText = EnclosePropertyInQuotation(commandText);

                using (var  reader = Command.ExecuteReader())
                {
                    var table = new DataTable();

                    table.Load(reader);

                    return table;
                }
            }
            catch (Exception ex)
            {
                throw CreateEasylinkException(Command.CommandText, ex);
            }
        }

     

        private  void SetObjectIdBeforeInsert(object obj)
        {
            var objectType = obj.GetType();

            var id = RetrieveNextId(objectType);

            var idPropertyName = ClassConfigContainer.FindIdPropertyNamer(objectType);

            SetObjectId(obj,idPropertyName, id);
          
        }


        private  static   long SetObjectIdAfterInsert(object obj, object result, out string idPropertyName)
        {
            long id = 0;

            var succeeded = Int64.TryParse(result.ToString(), out id);

            if (succeeded == false)
            {
                throw new EasylinkException(
                    "Error occured when setting object id for {0},  database does not return number!", obj.GetType().Name);
            }


            idPropertyName = ClassConfigContainer.FindIdPropertyNamer(obj.GetType());

            SetObjectId(obj, idPropertyName, id);
            return id;
        }



        private  static  void SetObjectId(object obj, string idPropertyName, Int64 id)
        {

            var objectType = obj.GetType();

            var propertyInfo = objectType.GetProperty(idPropertyName);


            if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
            {
                propertyInfo.SetValue(obj, (int)id, null);
            }
            else if (propertyInfo.PropertyType == typeof(Int64) || propertyInfo.PropertyType == typeof(Int64?))
            {
                propertyInfo.SetValue(obj, id, null);
            }
            else
            {
                throw new EasylinkException(
                    "Object {0}  Id property {1} is type {2}, and can not set with a number value.", obj.GetType().Name,
                    idPropertyName, propertyInfo.PropertyType);

            }

        }

        private void NullifyIdProperty(object obj)
        {
            var classConfig = ClassConfigContainer.FindClassConfig2(obj);

            var idPropertyName = classConfig.IdPropertyName;

            var property = obj.GetType().GetProperty(idPropertyName);

            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(int?) ||
                property.PropertyType == typeof(Int64?) || property.PropertyType == typeof(DateTime?))
            {
                property.SetValue(obj, null, null);
            }

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
            {
                property.SetValue(obj, -1, null);
            }

            if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(obj, DateTime.MinValue, null);
            }

        }

        private  object GetObjectId(object obj)
        {
            var classConfig = ClassConfigContainer.FindClassConfig2(obj);

            var idPropertyName = classConfig.IdPropertyName;

            var property = obj.GetType().GetProperty(idPropertyName);

            return property.GetValue(obj, null);
        }

        internal abstract string FindNextIdSql(Type objectType);

        private Int64 RetrieveNextId(Type objectType)
        {
          
            var nextIdSql = FindNextIdSql(objectType);

            var result = ExecuteScalar(nextIdSql);

            return Int64.Parse(result.ToString());
        }


        private void InsertAuditRecord(object obj, DbOperation operation, string auditText)
        {
           
            var auditRecord = Auditor.CreateAuditRecord(obj, operation, auditText);

            Insert(auditRecord);
        }


        private static string EnclosePropertyInQuotation(string sql)
        {
            var matches = Regex.Matches(sql, @"\s+as\s+(?<propertyName>[\w.]+)[,\s\n]+",
                                                    RegexOptions.IgnoreCase | RegexOptions.Singleline);


            foreach (Match match in matches)
            {
                var propertyName = match.Groups["propertyName"].Value;

                var temp = match.Value.Replace(propertyName, "\"" + propertyName + "\" ");

                sql = sql.Replace(match.Value, temp);
            }

            return sql;
        }


        private Dictionary<string, object> ConvertPropertyParametersToSqlParameters(
            Dictionary<string, object> oldParameters)
        {
            var sqlParameters = new Dictionary<string, object>();
            foreach (var key in oldParameters.Keys)
            {
                sqlParameters[key.Replace(".", "")] = oldParameters[key];
            }

            return sqlParameters;
        }
     

        protected EasylinkException CreateEasylinkException(string sqlId, Exception ex)
        {
            if (ex is EasylinkException)
            {
                throw ex as EasylinkException;
            }

            if (Command.Status == CommandStatus.Before)
            {
                throw new EasylinkException("Error occurred before executing sql:\n {0}, error: {1} ", sqlId,
                                            ex.Message);
            }

            if (Command.Status == CommandStatus.Executing)
            {
                throw new EasylinkException("Error occurred when executing sql:\n{0}, error: {1} ", sqlId,
                                            ex.Message);
            }

            if (Command.Status == CommandStatus.Executed)
            {
                throw new EasylinkException("Error occurred after executing sql:\n {0}, error: {1} ", sqlId,
                                            ex.Message);
            }

            throw ex;
        }

       
     

        private string CreateSqlTextByCriteriaNode(string sql, CriteriaNode criteriaNode)
        {
            var sb = new StringBuilder();

            sb.AppendLine(sql);

            var  whereClause = CreateWhereClause(criteriaNode);
 
            if (!String.IsNullOrEmpty(whereClause))
            {
               sb.AppendLine(" WHERE " + whereClause);
            }

       
            return sb.ToString();
        }

        private string CreateWhereClause(CriteriaNode criteriaNode)
        {
            var criterias = criteriaNode.SearchAll();

            if (criterias.Count == 0) return string.Empty;

            if (criteriaNode.Criteria != null)
            {
                return CreateCondition(criteriaNode.Criteria);

            }

            return string.Format("({0} {1} {2})", CreateWhereClause(criteriaNode.LeftChild), criteriaNode.NodeType,
                                 CreateWhereClause(criteriaNode.RightChild));


        }


        private string CreateCondition(Criteria criteria)
        {
            switch (criteria.Operator)
            {
                case "=":
                case "!=":
                    return CreateEqualOrNonEqualCondition(criteria);
                  

                case ">=":
                case ">":
                case "<":
                case "<=":
                    return CreateGreaterOrLessCondition(criteria);
                

                case "StartsWith":
                case "EndsWith":
                case "Contains":
                    return CreateLikeCondition(criteria);
                   
                case "In":
                     return CreateInListCondition(criteria);
                   
            }

            throw new EasylinkException("Error create criteria condition. Operator {0} is not supported!",
                                        criteria.Operator);

        }
     

        private string CreateInListCondition(Criteria criteria)
        {
            var list = criteria.Value as List<object>;

            if (list.Count == 0)
            {
                throw new EasylinkException("Can not create in list condition: list count is zero.");
            }

            var firstItem = list[0];

            if (firstItem is string)
            {
                var temp = from l in list select string.Format("'{0}'", l);
                return string.Format("({0} IN ({1}))", criteria.ColumnName, String.Join(",", temp));
            }

            if (firstItem is DateTime)
            {
                     var  temp = from l in list select string.Format("'{0:yyyy-MM-dd}'", l);
                     return string.Format("({0} IN ({1}))", criteria.ColumnName, String.Join(",", temp));
            }

            if (firstItem is int || firstItem is Int64 || firstItem is decimal || firstItem is double)
            {
                return string.Format("({0} IN ({1}))", criteria.ColumnName, String.Join(",", list));
            }


            throw new EasylinkException("Can not create in list condition: criteria.Value type {0} is not supported!",
                                        firstItem.GetType().Name);
        }
        
      
 

        private string CreateSqlTextByParameters(string sql, Dictionary<string, object> parameters)
        {

            foreach (var parameter in parameters.Keys)
            {
                object value = parameters[parameter];

                if (value is string || value is DateTime)
                {
                    sql = sql.Replace(parameter, string.Format("'{0}'", value));
                }
                else
                {
                    sql = sql.Replace(parameter, value.ToString());
                }
            }

            return sql;
        }



        private  Dictionary<string, string> CompareToOriginal<T>(T obj, Dictionary<String, Object> parameters) where T: new()
        {

            var temp = new Dictionary<string, string>();

            var  original = GetOriginal(obj);

            if (original == null) return temp; 

          

            foreach (var  parameterName in parameters.Keys)
            {
                var originalValue = Shared.FindPropertyValue(parameterName, original);

                var currentValue = parameters[parameterName];

                if (IsValueChanged(originalValue, currentValue))
                {
                    temp[parameterName] = string.Format("from [{0}] to [{1}]", originalValue, currentValue);
                }
            }

            return temp;
        }

        private object  GetOriginal(object  obj) 
        {

            var classConfig = ClassConfigContainer.FindClassConfig(obj.GetType());

            string idPropertyName = classConfig.IdPropertyName;

            var propertyInfo = obj.GetType().GetProperty(idPropertyName);

            object idPropertyValue = propertyInfo.GetValue(obj, null);

            return RetrieveOriginal (obj, new Criteria(idPropertyName, "=", idPropertyValue));
 
        }

        private object  RetrieveOriginal(object obj, Criteria  criteria)  
        {
            var selectSql = ClassConfigContainer.FindSelectSql(obj.GetType());

             criteria= UpdateCriteria(obj.GetType(),criteria);


            var table = RetrieveTable(selectSql, new CriteriaNode(null) {Criteria = criteria});

            var list = ObjectMapper.MapTableToList(obj.GetType(), table);

            if (list.Count == 0)
            {
                return null; 
            }

            if (list.Count > 1)
            {
                throw new ServerException("More than one objects matched with criteria are retrieved.");
            }

            return list[0];
        }



        private bool IsValueChanged(object originalValue, object currentValue)
        {
            if (originalValue == null)
            {
                if (currentValue == null) return false;
                if (currentValue.ToString() == string.Empty) return false;
                return true;
            }

            if (currentValue == null)
            {
                if (originalValue.ToString() == string.Empty) return false;
                return true;
            }

            if (currentValue.GetType() == typeof (double))
            {
                if (Math.Abs((double) currentValue - (double) originalValue) < 0.000000001)
                {
                    return false;
                }
                return true;
            }

            if (originalValue.Equals(currentValue)) return false;

            return true;
        }

        private  List<Criteria> UpdateCriteria<T>(List<Criteria> criterias)
        {
            var found = ClassConfigContainer.FindClassConfig1<T>();

            foreach (var criteria in criterias)
            {
                PropertyConfig propertyConfig = found.GetPropertyConfig(criteria.PropertyName);

                if (propertyConfig == null)
                {
                    var temp = criteria.PropertyName.Split(new[] { '.' }).ToList();

                    var propertyName = temp[temp.Count - 1];

                    temp.RemoveAt(temp.Count - 1);

                    var linkName = string.Join(".", temp);

                    var link = found.FindMatchedLink(linkName);


                    if (link != null)
                    {
                        var classConfig = ClassConfigContainer.FindClassConfig(link.RightType);

                        var propertyConfig1 = classConfig.GetPropertyConfig(propertyName);

                        if (propertyConfig1 == null)
                        {
                            throw new EasylinkException(
                            "Error occurred when updating criteria. property name {0}  is not defined in the {1} mapping.",
                            criteria.PropertyName, link.RightType.Name);
                        }

                        criteria.SetColumnName(string.Format("{0}.{1}", link.Alias, propertyConfig1.ColumnName));
                    }
                    else
                    {
                        throw new EasylinkException(
                            "Error occurred when updating criteria. link name {0} is not defined in the {1} mapping.",
                            linkName, typeof(T).Name);
                    }
                }
                else
                {
                    criteria.SetColumnName(propertyConfig.SelectColumn.Trim());
                }

            }

            return criterias;
        }


        private  Criteria UpdateCriteria(Type objectType, Criteria  criteria)
        {
            var found = ClassConfigContainer.FindClassConfig(objectType);


            PropertyConfig propertyConfig = found.GetPropertyConfig(criteria.PropertyName);

            if (propertyConfig == null)
            {
                var temp = criteria.PropertyName.Split(new[] { '.' }).ToList();

                var propertyName = temp[temp.Count - 1];

                temp.RemoveAt(temp.Count - 1);

                var linkName = string.Join(".", temp);

                var link = found.FindMatchedLink(linkName);


                if (link != null)
                {
                    var classConfig = ClassConfigContainer.FindClassConfig(link.RightType);

                    var propertyConfig1 = classConfig.GetPropertyConfig(propertyName);

                    criteria.SetColumnName(string.Format("{0}.{1}", link.Alias, propertyConfig1.ColumnName));
                }
                else
                {
                    throw new EasylinkException(
                        "Error occurred when updating criteria. property name {0} or link name{1} is not found in the {2} mapping.",
                        criteria.PropertyName, linkName, objectType.Name);
                }
            }
            else
            {
                criteria.SetColumnName(propertyConfig.SelectColumn.Trim());
            }

            return criteria; 
        }






    }
}