using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;


namespace Easylink
{
    public class MappingConfig<T> :IMappingConfig where T: new()
    {

  
        public List<PropertyConfig> Properties { get; private set; }

        public List<LinkConfig> Links { get; private set; }

        private LinkConfig _currentLink;

        private PropertyConfig _currentProperty;

        public bool  Auditable { get; private set; }

        public NextIdOption NextIdOption { get; private set; }

        internal MappingConfig()
        {
            Properties = new List<PropertyConfig>();

            Links = new List<LinkConfig>();

            Auditable = false;

            NextIdOption = NextIdOption.None;

        }



        public string TableName { get; private set; }

        public string SequenceName { get; private set; }
   
        public  Type Type
        {
            get { return typeof (T); }
        }


        public MappingConfig<T>  EnableAudit()
        {
            Auditable = true; 

            return this;
        }


        public MappingConfig<T> ToTable(string tableName)
        {
            TableName = tableName;

            return this;
        }


        public MappingConfig<T> NextId(NextIdOption nextIdOption, string sequence= null)
        {
            SequenceName = sequence;
            NextIdOption = nextIdOption;

            return this;
        }

        public void SetNextId(NextIdOption nextIdOption, string sequence= null)
        {
            SequenceName = sequence;
            NextIdOption = nextIdOption;
        }


        public LinkConfig Link<TU, TV>(Expression<Func<TU, object>> expression1,
                                                Expression<Func<TV, object>> expression2, LinkType linkType =LinkType.Object)
        {


            var linkName = Expressions.GetPropertyName(expression1.Body);

            if (linkType == LinkType.Object)
            {
                var pos =  linkName.LastIndexOf(".");

                if (pos != -1)
                {
                    linkName = linkName.Substring(0, pos);

                }
                else
                {

                    linkName = FindLinkName(typeof (TV));

                }


            }

            var link = new LinkConfig(Type, linkName, linkType);


            Links.Add(link);

            _currentLink = link;

            _currentLink.Link(expression1, expression2);

            
            return _currentLink;

          

        }


        private string FindLinkName(Type propertyType)
        {

            var propertyFound = Type.GetProperty(propertyType.Name);

            if (propertyFound != null) return propertyFound.Name;
 
 
            throw new EasylinkException("Error occurred when finding link name.  no  property  {0} is declared in {1}.",
                                        propertyType.Name, Type.Name);

        }

 

        public MappingConfig<T> Property(Expression<Func<T, object>> expression)
        {
            var body = expression.Body;

            var propertyName = Expressions.GetPropertyName(body);

            var property = new PropertyConfig(Type, propertyName);


             Properties.Add(property);

             _currentProperty = property; 


             return this; 
        }


        public MappingConfig<T> ToColumn(string columnName)
        {
            _currentProperty.SetColumnName(columnName);

            return this;
        }

        public MappingConfig<T> ToIdColumn(string columnName)
        {
           
            _currentProperty.IsIdColumn = true;

            return ToColumn(columnName);
        }

    
       
        public  MappingConfig<T>  AsReadOnly()
        {
            _currentProperty.IsReadOnly = true;

            return this;
        }


        public void GetFrom<TU>(Expression<Func<TU, object>> expression, LinkConfig link)
        {
          
            _currentProperty.GetFrom(expression, link);

        }

     

    }
}