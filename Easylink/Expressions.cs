using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Easylink
{
    internal static class Expressions
    {

        internal static CriteriaNode CreateCriteriaNode<T>(params Expression<Func<T, bool>>[] expressions) where T : new()
        {
             
               if (expressions.Length == 0) return new CriteriaNode(null);

               var nodes = new List<CriteriaNode>();

               foreach (var exp in expressions)
               {
                   var node = new CriteriaNode(null);
                   VisitExpression<T>(exp, node);

                   nodes.Add(node);
               }

               if (nodes.Count == 1) return nodes[0];

               var firstNode = nodes[0];
               nodes.RemoveAt(0);

               return MergeCriteriaNodes(firstNode, nodes);

        }

         private static CriteriaNode MergeCriteriaNodes(CriteriaNode criteriaNode, IList<CriteriaNode> nodes)
         {
             if (nodes.Count == 0) return criteriaNode;

             var mergedNode = new CriteriaNode();
 
             mergedNode.LeftChild = criteriaNode;
             mergedNode.RightChild = nodes[0];
             mergedNode.NodeType = CriteriaNodeType.AND;

             nodes.RemoveAt(0);

             return MergeCriteriaNodes(mergedNode, nodes);
         }


         private static void VisitExpression<T>(Expression expression,  CriteriaNode criteriaNode)
         {

             if (expression is Expression<Func<T, bool>>)
             {
                 var temp = expression as Expression<Func<T, bool>>;

                 VisitExpression<T>(temp.Body, criteriaNode);

                 return;

             }

             if (expression is BinaryExpression)
              {
                  var binaryExpresion = expression as BinaryExpression;

                  if (binaryExpresion.NodeType == ExpressionType.AndAlso ||
                      binaryExpresion.NodeType == ExpressionType.OrElse)
                  {
                      criteriaNode.LeftExpression = binaryExpresion.Left;

                      criteriaNode.RightExpression = binaryExpresion.Right;

                      if (binaryExpresion.NodeType == ExpressionType.AndAlso)
                      {
                          criteriaNode.NodeType = CriteriaNodeType.AND;
                      }
                      else
                      {
                          criteriaNode.NodeType = CriteriaNodeType.OR;
                      }

                      criteriaNode.LeftChild = new CriteriaNode(criteriaNode);

                      criteriaNode.RightChild = new CriteriaNode(criteriaNode);

                      VisitExpression<T>(criteriaNode.LeftExpression, criteriaNode.LeftChild);

                      VisitExpression<T>(criteriaNode.RightExpression, criteriaNode.RightChild);

                      return; 

                  }
  
              }

              criteriaNode.Criteria = CreateCriteria<T>(expression);
 
        }
         


        internal   static Criteria CreateCriteria<T>(Expression expression)
        {


            if (expression is Expression<Func<T,bool>> )
            {

                var temp = expression as Expression<Func<T, bool>>;


                if (temp.Body is BinaryExpression)
                {
                    return ConvertBinaryExpressionToCriteria(temp.Body as BinaryExpression);
                }

                if (temp.Body is MethodCallExpression)
                {
                    return ConvertMethodCallExpressionToCriteria(temp.Body as MethodCallExpression);

                }

                return null; 
            }

            if (expression is BinaryExpression)
            {
                return ConvertBinaryExpressionToCriteria(expression  as BinaryExpression);
            
            }


            if (expression is MethodCallExpression)                       
            {
                 return  ConvertMethodCallExpressionToCriteria(expression  as MethodCallExpression);
                
 
            }

            return null;
        }


        private static Criteria ConvertMethodCallExpressionToCriteria(MethodCallExpression expression)
        {

            if (expression.Method.DeclaringType == typeof(string))
            {

                var propertyName = GetPropertyName(expression.Object);

                object value = null;

                if (expression.Arguments[0] is MemberExpression)
                {
                    value = GetValueFromMemberExpression(expression.Arguments[0] as MemberExpression);
                }
                else if (expression.Arguments[0] is ConstantExpression)
                {
                    value = (expression.Arguments[0] as ConstantExpression).Value; 
                }
                else if (expression.Arguments[0] is MethodCallExpression)
                {
                    var methodCallExpression = expression.Arguments[0] as MethodCallExpression;

                    if (methodCallExpression.Method.DeclaringType == typeof (Condition))
                    {
                        if (methodCallExpression.Method.Name == "CaseSensitive")
                        {
                            var temp = Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();

                            value = new CaseSensitiveObject {Value = temp.ToString()};
                        }
                    }
                    else
                    {
                        value = Expression.Lambda(expression.Arguments[0] as MethodCallExpression).Compile().DynamicInvoke();
                    }

                }
                else
                {
                    throw new EasylinkException(
                        "Error occurred when converting   method call expression. Invalid argument expression {0}.",
                        expression.Arguments[0].Type);

                }



                var op = expression.Method.Name;

                if (op != "StartsWith" && op != "EndsWith" && op != "Contains")
                {
                    throw new EasylinkException("Method {0} of type string is not supported in Easylink.", op);

                }

                if (value is CaseSensitiveObject)
                {
                    return new Criteria(propertyName, op, (value as CaseSensitiveObject).Value, true);
                }


                return new Criteria(propertyName, op, value);
 

            }

         
            if (expression.Method.DeclaringType == typeof (Condition))
            {
                if (expression.Method.Name == "In")
                {

                    var  expr = (MemberExpression) expression.Arguments[0];

                     var propertyName =GetPropertyName(expr);

              
                    var member = Expression.Convert(expression.Arguments[1], typeof(object));
                    var lambda = Expression.Lambda<Func<object>>(member);
                    var getter = lambda.Compile();

                    var inArgs = Condition.Flatten(getter() as IEnumerable);

                    return new Criteria(propertyName, "In", inArgs);

                }
            }

            throw new EasylinkException("Method {0} of type {1} is not supported in Easylink.", expression.Method.Name, expression.Method.DeclaringType);


        }


        private static Criteria ConvertBinaryExpressionToCriteria(BinaryExpression expression)
        {
         
            var left = (MemberExpression)expression.Left;

            var propertyName = GetPropertyName(left);

            var op = GetOp(expression.NodeType);

            var right = GetValue(expression.Right);

            if (right is CaseSensitiveObject)
            {
                return new Criteria(propertyName, op, (right as CaseSensitiveObject).Value, true);
            }

            return new Criteria(propertyName, op, right);
        }


        private static object GetValue(Expression expression)
        {
            if (expression is ConstantExpression)
            {
                return (expression as ConstantExpression).Value; 
            }

            if (expression is MemberExpression)
            {
                return GetValueFromMemberExpression(expression as MemberExpression);

            }


            if (expression is UnaryExpression)
            {
                return GetValueFromUnaryrExpression(expression as UnaryExpression);

            }

            if (expression is BinaryExpression)
            {

                return GetValueFromBinaryExpression(expression as BinaryExpression);

            }

            if (expression is MethodCallExpression)
            {

                var methodCallExpression = expression as MethodCallExpression;


                if (methodCallExpression.Method.DeclaringType == typeof(Condition))
                {
                    if (methodCallExpression.Method.Name == "CaseSensitive")
                    {
                        var temp = Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();

                        return new CaseSensitiveObject {Value = temp.ToString()};
                    }

                  
                }


                if (methodCallExpression.Method.DeclaringType == typeof(Int32))
                {
                     

                    if (methodCallExpression.Method.Name == "Parse")
                    {
                        var temp = Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();

                        return (int)temp;
                    }
                }
            }

            throw new EasylinkException("Error occurred when get value from  expression. {0} is not supported.", expression.Type);

        }



        private static object GetValueFromBinaryExpression(BinaryExpression expression)
        {
                var left = expression.Left;
 
                if (left.Type== typeof(string))
                {

                    var lambda1 = Expression.Lambda<Func<string>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type ==  typeof(int?))
                {

                    var lambda1 = Expression.Lambda<Func<Int32?>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(int))
                {

                    var lambda1 = Expression.Lambda<Func<int>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(decimal?))
                {

                    var lambda1 = Expression.Lambda<Func<decimal?>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(decimal))
                {

                    var lambda1 = Expression.Lambda<Func<decimal>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }



                if (left.Type == typeof(double?))
                {

                    var lambda1 = Expression.Lambda<Func<double?>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(double))
                {

                    var lambda1 = Expression.Lambda<Func<double>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(long?))
                {

                    var lambda1 = Expression.Lambda<Func<long?>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }

                if (left.Type == typeof(long))
                {

                    var lambda1 = Expression.Lambda<Func<long>>(expression);

                    var getter1 = lambda1.Compile();

                    return getter1();
                }


            throw new EasylinkException("Error occurred when get value from  binary expression...");

        }

        private static object GetValueFromUnaryrExpression(UnaryExpression expression)
        {

            if (expression.NodeType == ExpressionType.Convert) 
            {

                if (expression.Operand.NodeType == ExpressionType.Constant)
                {
                    return (expression.Operand as ConstantExpression).Value; 
                }


                if (expression.Operand.NodeType == ExpressionType.New)
                {
                    var member = Expression.Convert(expression.Operand, typeof(object));
                    var lambda = Expression.Lambda<Func<object>>(member);

                    var getter = lambda.Compile();

                    return getter(); 
                }
              
                if (expression.Operand.NodeType == ExpressionType.MemberAccess)
                {
                        return  GetValueFromMemberExpression(expression.Operand as MemberExpression);

                }

                if (expression.Operand.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = expression.Operand as MethodCallExpression;

                    return  Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
                }

            }

            if (expression.NodeType == ExpressionType.Not)
            {

                if (expression.Operand.NodeType == ExpressionType.MemberAccess)
                {
                    var temp = GetValueFromMemberExpression(expression.Operand as MemberExpression);

                    return !((bool)temp);

                }

                if (expression.Operand.NodeType == ExpressionType.Constant)
                {
                    var temp = (expression.Operand as ConstantExpression).Value;

                    return !((bool)temp);

                }

                if (expression.Operand.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = expression.Operand as MethodCallExpression;

                     var temp =Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();

                    return !((bool)temp);

                }


            }
        
            throw new EasylinkException("Error occurred when get value from unary expression (node type: {0}).",
                                        expression.NodeType);
        }

 


        private static object GetValueFromMemberExpression(Expression expression)
        {
            
            var objectMember = Expression.Convert(expression, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }


        private static string GetOp(ExpressionType expressionType)
        {

            switch (expressionType)
            {

                case ExpressionType.Equal:
                    return "=";

                case ExpressionType.NotEqual:
                    return "!=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
            }

            throw new EasylinkException("Expression  {0} is not supported.", expressionType.ToString());


        }



        internal   static string GetPropertyName(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;

                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    return GetPropertyName(memberExpression.Expression) + "." + memberExpression.Member.Name;
                }


                return memberExpression.Member.Name;
            }

            if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;

                if (unaryExpression.NodeType != ExpressionType.Convert)
                {
                    throw new Exception(string.Format("Cannot interpret member from {0}", expression));
                }

                return GetPropertyName(unaryExpression.Operand);
            }


            throw new EasylinkException("Could not determine member from {0}", expression);
        }
    }
}