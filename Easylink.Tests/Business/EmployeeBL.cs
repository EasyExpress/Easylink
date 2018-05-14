using System;
using System.Collections.Generic;

using Easylink.Tests.Model;

namespace Easylink.Tests.Business
{
    public class EmployeeBL : BaseBL
    {
        public EmployeeBL(IDatabase database)
        {
            Database = database;
        }

        public List<Employee> RetrieveAll()
        {
            var  all = Database.RetrieveAll<Employee>();

            all.ForEach(BuildEntity);

            return all;
        }


        public Employee RetrieveById(Int64 employeeId)
        {
            var found = Database.RetrieveObject<Employee>( e => e.Id == employeeId);

            BuildEntity(found);

            return found;
        }

        private void BuildEntity(Employee e)
        {
            e.Programs = Database.RetrieveAll<EmployeeProgram>(ep => ep.EmployeeId == e.Id);
        }


        public void DeleteEmployee(Employee employee)
        {
            Action procedure = () =>
                {
                    Database.DeleteAll<EmployeeProgram>(ep => ep.EmployeeId == employee.Id);

                    Database.Delete(employee.FinancialInfo);

                    Database.Delete(employee.Address);

                    Database.Delete(employee);
                };

            Database.ExecuteInTransaction(procedure);
        }


        public void UpdateEmployee(Employee employee)
        {
            employee.ValidateRules();

            Action procedure = () =>
                {

                    Database.Save(employee.Address);

                    Database.Save(employee.FinancialInfo);

                    Database.Save(employee.AdditionalInfo);

                    Database.Update(employee);

                    employee.Programs.ForEach(p =>
                        {
                            p.EmployeeId = employee.Id;

                            Database.Save(p);
                        });
                };

            Database.ExecuteInTransaction(procedure);
        }


        public void InsertEmployee(Employee employee)
        {
            employee.ValidateRules();

            Action procedure = () =>
                {

                    if (employee.Address !=null && !string.IsNullOrEmpty(employee.Address.HouseNumber))
                    {
                        Database.Insert(employee.Address);
                    }

                    if (employee.AdditionalInfo!= null && !string.IsNullOrEmpty(employee.AdditionalInfo.Notes))
                    {
                        Database.Insert(employee.AdditionalInfo);
                    }
 
                    Database.Insert(employee);

                    employee.Programs.ForEach(p =>
                        {
                            p.EmployeeId = employee.Id;

                            Database.Insert(p);
                        });
                };

            Database.ExecuteInTransaction(procedure);
        }
  
    }
}