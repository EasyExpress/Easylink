using System;

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

 
using EasylinkApp.WPF;


namespace EasylinkApp.Tests
{

    [TestClass]
    public class WPFSearchTest : BaseTest
    {

        public void  search_employee_by_first_name()
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
 


            };


            database.ExecuteInTest(procedure);


        }


        [TestMethod]
        public void search_employee_by_status()
        {

            Action procedure = () =>
            {

                //Act
                 SharedCode.InstallEmployee(database);

                var viewModel = new MainWindowViewModel(database);

                viewModel.ActiveSearch = "yes";

                viewModel.RunSearchCommand.Execute(this);


                var total = viewModel.EmployeesFound.Count;
                var totalActive = viewModel.EmployeesFound.Where(e => e.Active).Count();

                Assert.IsTrue(total > 0);

                Assert.IsTrue(total == totalActive);

               


            };


            database.ExecuteInTest(procedure);


        }

 
    }
}
