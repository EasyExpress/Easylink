using System;
using System.Linq.Expressions;

namespace Easylink
{
   

    public  class PropertyConfig
    {
      
        private LinkConfig _link;

        private Type _type;

        private Type  _getFromType;

        private string _getFromProperty;

        internal  PropertyConfig(Type type, string propertyName)
        {

            _type = type; 

            PropertyName = propertyName;

            IsIdColumn = false;

            IsReadOnly = false;

        

        }


        internal  void  GetFrom<T>(Expression<Func<T, object>> expression, LinkConfig link)
        {
            _getFromType = typeof (T);

            _getFromProperty = Expressions.GetPropertyName(expression.Body);

            
            _link = link;

       

        }


      

        internal bool IsIdColumn { get; set; }

        internal bool IsReadOnly { get;  set; }

        internal string PropertyName { get; private set; }

        private string _columnName; 

        internal string ColumnName
        {
            get
            {
                if (string.IsNullOrEmpty(_columnName))
                {
                    if (_link == null)
                    {
                        _columnName = PropertyName;
                    }
                    else
                    {
                        _columnName = GetColumnName();
                    }
                    
                }

                return _columnName; 
            }

           private  set { _columnName = value; }

        }

        internal string ScreenName { get; private set; }

 


        internal  string SelectColumn
        {
            get
            {
                
                return string.Format("{0}.{1}  ", GetTableName(), ColumnName);
            }
        }

        internal  string SelectPart
        {
            get
            {

                
                
                return string.Format("{0} AS {1}", SelectColumn, PropertyName);
 
            }
        }


        internal  string UpdatePart
        {
            get { return string.Format("{0} = @{1}", ColumnName, PropertyName); }
        }


        internal void SetColumnName(string columnName)
        {
            ColumnName = columnName;

          
        }

        internal void SetScreenName(string columnName)
        {
            ColumnName = columnName;
        }

        internal  void SetLink(LinkConfig link)
        {
            _link = link;
        }


        internal  bool IsDerived
        {
            get { return _link != null; }
        }

        private string GetTableName()
        {
            if (IsDerived)
            {
                return _link.Alias;
             
            }
            var myClassConfig = ClassConfigContainer.FindClassConfig(_type);

            return myClassConfig.TableName;
        }

        private string GetColumnName()
        {

            
                var getFromClassConfig = ClassConfigContainer.FindClassConfig(_getFromType);

                var getFromPropertyConfig = getFromClassConfig.GetPropertyConfig(_getFromProperty);

                if (getFromPropertyConfig == null)
                {
                    throw new EasylinkException("Property [{0}] in class [{1}] column mapping is not set.",
                                                _getFromProperty, _getFromType.Name);

                }
               var columnName = getFromPropertyConfig.ColumnName;

              
                return columnName;

        }
    }
}