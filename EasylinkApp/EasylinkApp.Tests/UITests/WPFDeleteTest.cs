using System;
 
using EasylinkApp.WPF;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EasylinkApp.Tests
{

    [TestClass]
    public class WPFDeleteTest : BaseTest
    {

       
        [TestMethod]
        public void  should_delete_an_employee()
        {

            Action procedure = () =>
            {

                //Act
                var employee = SharedCode.InstallEmployee(database);

                var viewModel = new MainWindowViewModel(database);

                viewModel.FirstNameSearch = employee.FirstName;

                viewModel.RunSearchCommand.Execute(database);

                Assert.IsTrue(viewModel.EmployeesFound.Count == 1);
                Assert.IsTrue(viewModel.EmployeesFound[0].FirstName == employee.FirstName);

                viewModel.SelectedEmployee = viewModel.EmployeesFound[0];

                viewModel.RunDeleteCommand.Execute(viewModel.SelectedEmployee);

                Assert.IsTrue(viewModel.EmployeesFound.Count == 0);
              


            };


            database.ExecuteInTest(procedure);


        }

 
 
    }
}
