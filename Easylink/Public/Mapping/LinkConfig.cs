 

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
 

namespace Easylink
{
 

    public  enum LinkType
    {
        Property,
        Object
    };

    public  class  LinkConfig
    {

        internal Type Type { get;  set; }
        internal string LinkName { get;   set; }


       
        internal LinkConfig Parent { get; private set; }
     

        internal Type LeftType { get; private set; }
        internal Type RightType { get; private set; }

        internal string Alias { get; private set; }
       


        internal string LeftPropertyName { get; private set; }
        internal string RightPropertyName { get; private set; }


        private LinkType _linkType;

        private List<PropertyConfig> _derivedPropertyConfigs;





        public LinkConfig Extend(LinkConfig link)
        {
            Parent = link;

            LinkName = string.Format("{0}.{1}", Parent.LinkName, LinkName);

            return this;
        }



        internal LinkConfig Link<TU, TV>(Expression<Func<TU, object>> expression1,
       
                                     Expression<Func<TV, object>> expression2)
        {
            LeftType = typeof(TU);
            RightType = typeof(TV);

            LeftPropertyName = Expressions.GetPropertyName(expression1.Body);
            RightPropertyName = Expressions.GetPropertyName(expression2.Body);

            return this;
        }



        internal LinkConfig(Type type, string linkName,LinkType linkType)
        {

            Type = type;

            LinkName = linkName;

            _linkType = linkType;
          
        }

 

        internal string JoinPart
        {
            get
            {

               
                var leftColumnName = GetLeftColumnName();

                var rightTableName = GetRightTableName();
                var rightColumnName = GetRightColumnName();

                var parentAlias = string.Empty;
 
                if (Parent != null)
                {
                    parentAlias = Parent.Alias;
                }
                else
                {
                    parentAlias = GetLeftTableName();
                }

                return string.Format("LEFT JOIN [Schema].{0} {1}  ON {2}.{3} = {1}.{4} ", rightTableName,
                                     Alias, parentAlias, leftColumnName, rightColumnName);
            }
        }

        internal   List<PropertyConfig> GetDerivedPropertyConfigs()
        {
             
                if (_derivedPropertyConfigs == null)
                {
                    if (_linkType == LinkType.Property) return new List<PropertyConfig>();

                    _derivedPropertyConfigs = new List<PropertyConfig>();

                    var rightClassConfig = ClassConfigContainer.FindClassConfig(RightType);
 
                    foreach (var property in rightClassConfig.Properties)
                    {
                        if (!property.IsDerived)
                        {
                            var derived = CreateDerivedProperty(property);

                            _derivedPropertyConfigs.Add(derived);

                          
                        }


                    }

                  
                }

                return _derivedPropertyConfigs;
       
        }

 
 


        private PropertyConfig CreateDerivedProperty(PropertyConfig property)
        {

            var propertyName = LinkName + "." + property.PropertyName;

            var derived = new PropertyConfig(Type, propertyName);


            derived.SetLink(this);

            derived.SetColumnName(property.ColumnName);
            return derived;
        }


      


        internal void SetAlias(string alias)
        {
            Alias = alias;
        }




        internal   string GetLeftTableName()
        {
            return ClassConfigContainer.FindClassConfig(LeftType).TableName;
        }


        internal string GetLeftColumnName()
        {
            var classConfig = ClassConfigContainer.FindClassConfig(LeftType);

            var propertyConfig = classConfig.GetPropertyConfig(LeftPropertyName);

             if (propertyConfig== null)
                {
                    throw new EasylinkException("Property name {0} is not defined in {1} mapping class.",
                                     LeftPropertyName, LeftType.Name);

                }

            return propertyConfig.ColumnName;
        }

         
         


        internal string GetRightTableName()
        {
            return ClassConfigContainer.FindClassConfig(RightType).TableName;
        }


        internal string GetRightColumnName()
        {

            var classConfig = ClassConfigContainer.FindClassConfig(RightType);

            var propertyConfig = classConfig.GetPropertyConfig(RightPropertyName);

            if (propertyConfig == null)
            {
                throw new EasylinkException("Property name {0} is not defined in {1} mapping class.",
                                 RightPropertyName, RightType.Name);

            }

            return propertyConfig.ColumnName;

           
        }

   
        internal  string GetMainTableName()
        {
             return Parent.GetMainTableName();
        }

      

    }

}