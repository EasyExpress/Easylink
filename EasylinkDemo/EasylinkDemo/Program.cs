using System;
using Easylink;


namespace EasylinkDemo
{
    class Program
    {
        static void Main(string[] args)
        {
               var dbConfig = new DbConfig()
               {
                   ConnectionString = @"Server=meng;Database=EasylinkWalkThrough;User Id=demo;Password=password;",
                   DatabaseType = DatabaseType.SqlServer,
                   SchemaName = "dbo"

               };

               DatabaseFactory.Initialize(dbConfig);


               var database = DatabaseFactory.Create();

                  database.ExecuteInTest(()=>
                  {
                      var employee = new Employee
                      {
                          FirstName = "Mark",
                          LastName = "Loren",
                          LoginId = "MLoren",
                          Active = true,
                          Role = new Lookup {Id = 2}
                          
                      };

                      //insert
                      database.Insert(employee);
                      Console.WriteLine("Employee is inserted!");
                      var employeeRetrieved = database.RetrieveObject<Employee>(e=>e.Id== employee.Id);
                      Console.WriteLine("Employee name: {0}", employeeRetrieved.FirstName);
                      Console.WriteLine("Employee Role: {0}", employeeRetrieved.Role.Name);

                      //update
                      employeeRetrieved.FirstName = "NewWorld";
                      database.Update(employeeRetrieved);
                      Console.WriteLine("Employee is updated!");
                      var employeeUpdated = database.RetrieveObject<Employee>(e=>e.Id== employee.Id);
                      Console.WriteLine("Employee new name: {0}", employeeUpdated.FirstName);

                      //delete
                      database.Delete(employeeUpdated);
                      Console.WriteLine("Employee is deleted!");
                      var employeeFound = database.RetrieveObject<Employee>(e=>e.Id==employee.Id);
                      if (employeeFound == null)
                      {
                          Console.WriteLine("Employee is not found!");
                      }

                      //validate
                      employee.LoginId = string.Empty;
                      var brokenRules = employee.ValidateRules();
                      Console.WriteLine(brokenRules[0].ErrorMessage);
                     
                      Console.ReadKey();

                  });

        }
       
    }
}
