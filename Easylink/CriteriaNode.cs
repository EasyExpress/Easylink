
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Easylink
{

    internal class CriteriaNode
    {


        private  CriteriaNode _leftChild;
 
        private   CriteriaNode _rightChild; 
        
           
        internal CriteriaNode LeftChild
        {
            get { return _leftChild; }

            set {
                
                _leftChild = value;
                _leftChild.Parent = value; 

            }
        }


        internal CriteriaNode RightChild
        {
            get { return _rightChild; }

            set
            {

                _rightChild = value;
                _rightChild.Parent = value;

            }
        }

        internal CriteriaNode Parent { get; set; }
   
        internal CriteriaNodeType NodeType { get;  set; }

        internal Expression LeftExpression { get;   set; }

        internal Expression  RightExpression  { get; set; }

        internal Criteria Criteria { get; set; }

        internal CriteriaNode( )
        {
            
        }
        
         internal CriteriaNode(CriteriaNode parent)
         {
             Parent = parent;
         }

        internal List<Criteria> SearchAll()
        {
            var all = new List<Criteria>();
            if (Criteria != null)
            {
                 all.Add(Criteria);
            }

            if (LeftChild != null)
            {
                all.AddRange(LeftChild.SearchAll());
            }
            if (    RightChild != null)
            {
                all.AddRange(RightChild.SearchAll());
            }

       
            return all; 
        }

    }
}