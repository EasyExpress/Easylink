using System;
using EasylinkApp.Business;
 
using Microsoft.VisualStudio.TestTools.UnitTesting; 


namespace EasylinkApp.Tests
{
 
    [TestClass]
    public  class EmployeeBLTest: BaseTest 
    {
  

        [TestMethod]
        public void employee_should_be_able_to_create()
        {


            Action procedure = () =>
             {

                 //Act
                 var employee = SharedCode.InstallEmployee(database);


                 var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                 //Assert 
                 Assert.IsTrue(employeeRetrieved.FirstName == employee.FirstName);
                 Assert.IsTrue(employeeRetrieved.Salary == employee.Salary);
 
          
             
             };


            database.ExecuteInTest(procedure);


        }


        [TestMethod]
        public void employee_should_be_able_to_modify()
        {


            Action procedure = () =>
            {

                //arrange
                var employee = SharedCode.InstallEmployee(database);


                var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

               
                employeeRetrieved.FirstName = "New Name";

                employeeRetrieved.Department = new LookupBL(database).RetrieveLookupByName(Department.Finance.ToString());


                employeeRetrieved.EmployedSince = new DateTime(2014, 12, 16);

                employeeRetrieved.Programs[0].MarkAsDeleted();
                Assert.IsTrue(employeeRetrieved.Programs[1].ProgramId == 3);

                employeeRetrieved.Programs[1].Status = ProgramStatus.Registered; 
                employeeRetrieved.Programs.Add(new EmployeeProgram { ProgramId = 1, Status = ProgramStatus.Completed });
                

                new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                var employeeUpdated = new EmployeeBL(database).RetrieveById(employee.Id);


                //assert 
                Assert.IsTrue(employeeUpdated.FirstName == "New Name");

                Assert.IsTrue(employeeUpdated.Department.Name == Department.Finance.ToString());

                Assert.IsTrue(employeeUpdated.EmployedSince == employeeRetrieved.EmployedSince); 

                Assert.IsTrue(employeeUpdated.Programs.Count == 2);

                var found = employeeUpdated.Programs.Find(p => p.ProgramId == 3);
                Assert.IsTrue(found != null);
                Assert.IsTrue(found.Status == ProgramStatus.Registered);

                found = employeeUpdated.Programs.Find(p => p.ProgramId == 1);
                Assert.IsTrue(found != null);
                Assert.IsTrue(found.Status == ProgramStatus.Completed);

            

            };


            database.ExecuteInTest(procedure);


        }



       [TestMethod]
        public void employee_should_able_to_retrieve_and_save_nullable()
        {


            Action procedure = () =>
            {

                //Act
                var employee = SharedCode.InstallEmployee(database);


                var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                //Assert 
                Assert.IsTrue(employeeRetrieved.Age == null);
                Assert.IsTrue(employeeRetrieved.Weight == null);
                Assert.IsTrue(employeeRetrieved.Married == null);
                Assert.IsTrue(employeeRetrieved.EmployedSince != null);

                employeeRetrieved.Weight = 145.67m;
                employeeRetrieved.EmployedSince = null;


                new EmployeeBL(database).UpdateEmployee(employeeRetrieved);

                var employeeSaved = new EmployeeBL(database).RetrieveById(employee.Id);

                Assert.IsTrue(employeeSaved.Age == null);
                Assert.IsTrue(employeeSaved.Weight == 145.67m);
                Assert.IsTrue(employeeSaved.Married == null);
                Assert.IsTrue(employeeSaved.EmployedSince == null);




            };


            database.ExecuteInTest(procedure);


        }




    }
}
