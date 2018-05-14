using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Text;

namespace Easylink
{
    internal class ClassConfig    
    {

         

        private string _selectSql;
        private string _insertSql;
        private string _insertSql0;
        private string _updateSql;
        private string _deleteSql;
        private string _whereSql;


        internal  Type  Type { get; private set;  }
        internal  string TableName { get; private set; }

    
        internal  NextIdOption NextIdOption { get;  set; }
        internal string SequenceName { get; set; }

        internal  List<PropertyConfig> Properties { get; private set; }

        internal List<LinkConfig> Links { get; private set; }

        internal bool Auditable { get; private set; }
     
 
        internal  ClassConfig(IMappingConfig  mappingConfig)
        {

            Properties = new List<PropertyConfig>();

            Links = new List<LinkConfig>();

            NextIdOption= mappingConfig.NextIdOption;

            SequenceName = mappingConfig.SequenceName;

            TableName = mappingConfig.TableName;

            Type = mappingConfig.Type;

            Auditable = mappingConfig.Auditable; 

            Properties.AddRange(mappingConfig.Properties);

            Links.AddRange(mappingConfig.Links);

            SetLinksAlias();
        }


        internal void SetNextId(NextIdOption nextIdOption, string sequenceName = null)
        {
            NextIdOption = nextIdOption;

            SequenceName = sequenceName;
        }


        internal  void Inherit(List<IMappingConfig> parentMappingConfigs)
        {
            foreach (var m in parentMappingConfigs)
            {
                var notFoundProperties = m.Properties.FindAll(p => !PropertyNotFound(p.PropertyName));
                Properties.AddRange(notFoundProperties);
                Links.AddRange(m.Links); 

            }

            SetLinksAlias();

        }

        internal bool PropertyNotFound(string propertyName)
        {
            return Properties.Find(p => p.PropertyName == propertyName) != null;

        }
       


        private void SetLinksAlias()
        {
            int index = 0; 
            foreach (var link in Links)
            {
                link.SetAlias(string.Format("A{0}", index));
                index++;
            }
        }



        internal  string IdPropertyName
        {
            get
            {
                var  found = Properties.Find(i => i.IsIdColumn);
                if (found == null)
                {
                    throw new EasylinkException("class {0} Id Column is not set.", Type.FullName);
                }

                return found.PropertyName;
            }
        }


        internal   PropertyConfig GetPropertyConfig(string propertyName)
        {
            
               return   Properties.Find(p => p.PropertyName == propertyName);

              

        }

 
       internal string SelectSql
        {
            get
            {
                if (String.IsNullOrEmpty(_selectSql))
                {
                    _selectSql = GenerateSelectSql();
                }

                return _selectSql;
            }
        }

 
    
        internal   string InsertSql0
        {
            get
            {
                if (String.IsNullOrEmpty(_insertSql0))
                {
                    _insertSql0 = GenerateInsertSql0();
                }

                return _insertSql0;
            }
        }

        internal  string InsertSql
        {
            get
            {
                if (String.IsNullOrEmpty(_insertSql))
                {
                    _insertSql = GenerateInsertSql();
                }

                return _insertSql;
            }
        }

        internal  string UpdateSql
        {
            get
            {
                if (String.IsNullOrEmpty(_updateSql))
                {
                    _updateSql = GenerateUpdateSql();
                }

                return _updateSql;
            }
        }

        internal  string DeleteSql
        {
            get
            {
                if (String.IsNullOrEmpty(_deleteSql))
                {
                    _deleteSql = GenerateDeleteSql();
                }

                return _deleteSql;
            }
        }

        internal string WhereSql
        {
            get
            {
                if (String.IsNullOrEmpty(_whereSql))
                {
                    _whereSql = GenerateWhereSql();
                }

                return _whereSql;
            }
        }


       

        internal   LinkConfig FindMatchedLink(string linkName)
        {
            var  found = from link in Links
                                            where link.LinkName == linkName
                                            select link;

            return found.FirstOrDefault();
        }

      
     

        private string GenerateSelectSql()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();

            var sb = new StringBuilder();

            sb.Append("SELECT ");

            var allProperties = new List<PropertyConfig>();

            allProperties.AddRange(Properties);

            foreach (var link in Links)
            {

                var derivedProperties = link.GetDerivedPropertyConfigs();
                 
                 foreach (var derived in derivedProperties)
                 {
                     var found = allProperties.Find(p => p.PropertyName == derived.PropertyName);
                     if (found == null)
                     {
                         allProperties.Add(derived);
                     }
                 }

           
            }

            var  selectList = from p in allProperties
                                             select p.SelectPart;


            sb.AppendLine(string.Join(",\n ", selectList));

            sb.AppendLine(String.Format(" FROM [Schema].{0}", TableName));


            Links.ForEach(l =>
                {
 
                        var join = l.JoinPart;
                        sb.AppendLine(" " + join + " ");
 
                });


            return sb.ToString();
        }

     


        private string GenerateInsertSql0()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();

            var sb = new StringBuilder();

            sb.AppendLine(String.Format("INSERT INTO [Schema].{0} ",TableName));


            var  found =
                Properties.FindAll(
                    p => p.IsIdColumn == false && p.IsReadOnly == false && !p.IsDerived);

            var columnNames = from p in found
                                              select p.ColumnName;


            var  propertyNames = from p in found
                                                select "@" + p.PropertyName;

            sb.AppendLine("(");

            sb.AppendLine(string.Join(",\n", columnNames));
            sb.AppendLine(")");

            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine(string.Join(",\n", propertyNames));
            sb.AppendLine(")");

            return sb.ToString();
        }

        private string GenerateInsertSql()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();

            var sb = new StringBuilder();

            sb.AppendLine(String.Format("INSERT INTO [Schema].{0} ", TableName));


            var  found =
                Properties.FindAll(p => p.IsReadOnly == false && !p.IsDerived);

            var columnNames = from p in found
                                              select p.ColumnName;


            var  propertyNames = from p in found
                                                select "@" + p.PropertyName;


            sb.AppendLine("(");

            sb.AppendLine(string.Join(",\n", columnNames));
            sb.AppendLine(")");

            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine(string.Join(",\n", propertyNames));
            sb.AppendLine(")");

            return sb.ToString();
        }


        private string GenerateUpdateSql()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();

            var sb = new StringBuilder();

            sb.AppendLine(String.Format("UPDATE [Schema].{0} ", TableName));

            var  updateList =
                from p in
                    Properties.FindAll(
                        i =>
                        i.IsIdColumn == false && i.IsReadOnly == false && !i.IsDerived)
                select p.UpdatePart;


            sb.AppendLine("SET ");

            sb.AppendLine(string.Join(",\n", updateList));

            return sb.ToString();
        }


        private string GenerateWhereSql()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();


            var sb = new StringBuilder();

            sb.Append(" WHERE ");

            var  idProperties = Properties.FindAll(i => i.IsIdColumn);

            var updateParts = from idProperty in idProperties select idProperty.UpdatePart;

            string whereList = string.Join(" AND ", updateParts);

            sb.Append(whereList);

            return sb.ToString();
        }


        private string GenerateDeleteSql()
        {
            ThrowExceptionIfMappingConfigIsNotComplete();

            var sb = new StringBuilder();

            sb.Append(string.Format("DELETE FROM [Schema].{0}", TableName));

            return sb.ToString();
        }


        private void ThrowExceptionIfMappingConfigIsNotComplete()
        {
            if (Properties.Count == 0)
            {
                throw new EasylinkException(" {0}Mapping  properties are not configured.", Type.FullName);
            }

            if (string.IsNullOrEmpty(TableName))
            {
                throw new EasylinkException("{0}Mapping table name is not set.", Type.FullName);
            }
        }
    }
}