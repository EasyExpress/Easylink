using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Easylink;
 


namespace EasylinkApp.Business
{
    public  class EmployeeBL  :BaseBL 
    {


        public EmployeeBL(IDatabase database)
        {
             Database = database; 
        }



        public List<Employee> Search(List<Expression<Func<Employee,bool>>> criterias)
        {
           
           return  Database.RetrieveAll(criterias.ToArray());

        }



        public List<Employee> RetrieveAll()
        {

           return  Database.RetrieveAll<Employee>();

         
 
        }

        public Employee RetrieveById(Int64 employeeId)
        {

           var found = Database.RetrieveObject<Employee>(e=>e.Id== employeeId);

            found.Programs = Database.RetrieveAll<EmployeeProgram>(ep=>ep.EmployeeId == found.Id);

           return found; 

 
        }

       

 
        public void DeleteEmployee(Employee employee)
        {

            Action procedure = () =>
                {

                    Database.DeleteAll<EmployeeProgram>(ep=>ep.EmployeeId ==employee.Id);
                
                    Database.Delete(employee); 


            };

            Database.ExecuteInTransaction(procedure);

        }


       
    
        public  void UpdateEmployee(Employee employee)
        {
            employee.Validate();

            Action procedure = () =>
            {

                Database.Update(employee); 

                employee.Programs.ForEach(p =>
                {
                    p.EmployeeId = employee.Id; 

                    SaveEmployeeProgram(p);
                }); 
               
            };

            Database.ExecuteInTransaction(procedure);


        }



        
        public  void InsertEmployee(Employee employee)
        {
            employee.Validate();

            Action procedure = () =>
            {
 
                    Database.Insert(employee);

                    employee.Programs.ForEach(p =>
                    {
                        p.EmployeeId = employee.Id;

                        InsertEmployeeProgram(p);
                    }); 
             
            };

            Database.ExecuteInTransaction(procedure);

 
         }


        private  void SaveEmployeeProgram(EmployeeProgram employeeProgram)
        {
            if (employeeProgram.LifeCycleStatus == BusinessBaseLifeCycle.New)
            {
                InsertEmployeeProgram(employeeProgram);
            }

            else if (employeeProgram.LifeCycleStatus == BusinessBaseLifeCycle.Deleted)
            {
                Database.Delete(employeeProgram);
            }

            else if (employeeProgram.LifeCycleStatus == BusinessBaseLifeCycle.Old)
            {
                Database.Update(employeeProgram);
            }

        }

        private  void InsertEmployeeProgram(EmployeeProgram employeeProgram)
        {

            Database.Insert(employeeProgram);


        }
    
    
    
    }

         
 }
 
