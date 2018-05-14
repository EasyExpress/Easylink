using System;
 
using EasylinkApp.Business;
using EasylinkApp.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EasylinkApp.Tests
{

    [TestClass]
    public class WPFSaveTest : BaseTest
    {



        [TestMethod]
        public void should_insert_an_employee()
        {

            Action procedure = () =>
            {

                //Act
                var employee = FakeEmployee.GetFakeEmployee(database);

                var viewModel = new EmployeeWindowViewModel(database, employee);


                viewModel.RunSaveCommand.Execute(null);

                var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);



                Assert.IsTrue(employeeRetrieved != null);


                Assert.IsTrue(employeeRetrieved.FirstName == employee.FirstName);

              


            };


            database.ExecuteInTest(procedure);


        }



        [TestMethod]
        public void should_update_an_employee()
        {

            Action procedure = () =>
            {

                //Act
                var employee = SharedCode.InstallEmployee(database);


                var employeeRetrieved = new EmployeeBL(database).RetrieveById(employee.Id);

                var viewModel = new EmployeeWindowViewModel(database, employeeRetrieved);

                viewModel.Active = !viewModel.Active;
                viewModel.FirstName = "newTest";


                viewModel.RunSaveCommand.Execute(null);

                var employeeUpdated = new EmployeeBL(database).RetrieveById(employeeRetrieved.Id);



                Assert.IsTrue(employeeUpdated != null);


                Assert.IsTrue(employeeUpdated.FirstName == "newTest");
                Assert.IsTrue(employeeUpdated.Active == viewModel.Active);



            };


            database.ExecuteInTest(procedure);


        }

 
    }
}
