using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

using Easylink;
using EasylinkApp.Business;
 
 
 

namespace EasylinkApp.WPF
{
    public class MainWindowViewModel : ObservableObject
    {

        private IDatabase _database; 


        private string    _firstNameSearch ="";
        private string    _lastNameSearch = "";
        private string    _activeSearch = "Any";
        private Int64     _departmentSearch=-1;

        private ObservableCollection<Employee> _employeesFound;

        private static List<Lookup> _departments;

        public ObservableCollection<Employee> EmployeesFound
        {
            get 
            { 
                return _employeesFound; 
            }
            set 
            {
                _employeesFound = value;

                RaisePropertyChanged(() =>EmployeesFound);
            }
        }


        private Employee  _selectedEmployee;

        public Employee  SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;

                RaisePropertyChanged(() => SelectedEmployee);
            }
        } 


        public  List<Lookup> Departments
        {
            get
            {
                if (_departments == null)
                {
                    _departments = new List<Lookup> {new Lookup  {Id = -1, Name = "Any"}};

                    _departments.AddRange(new LookupBL(_database).RetrieveLookupsByGroupName("Department"));
                     
                }

                return _departments;

            }
        }
       


        public string  FirstNameSearch
        {
            get { return _firstNameSearch; }

            set
            {
       
                    _firstNameSearch = value;

                    RaisePropertyChanged(() => this.FirstNameSearch);
 
            }

        }


        public string LastNameSearch
        {
            get { return _lastNameSearch; }

            set
            {
                   _lastNameSearch = value;

                   RaisePropertyChanged(() =>LastNameSearch);
  
            }

        }



        public string  ActiveSearch
        {
            get { return _activeSearch; }

            set
            {
                _activeSearch = value;

               RaisePropertyChanged(() => ActiveSearch);

            }

        }



        public Int64  DepartmentSearch
        {
            get { return _departmentSearch; }

            set
            {
                _departmentSearch = value;

                RaisePropertyChanged(() => DepartmentSearch);

            }

        }


        public MainWindowViewModel(IDatabase database)
        {
            _database = database; 

        }



        private ICommand _runSearchCommand;

        private ICommand _runCreateCommand;

        private ICommand _runDeleteCommand;

        private ICommand _runEditCommand;
       
        public ICommand RunSearchCommand
        {
            get
            {
                if (_runSearchCommand == null)
                {
                    _runSearchCommand = new RelayCommand(
                        param => RunSearch(),
                        param => true
                    );
                }
                return _runSearchCommand;
            }
        }


        public ICommand RunCreateCommand
        {
            get
            {
                if (_runCreateCommand == null)
                {
                    _runCreateCommand = new RelayCommand(
                        param => RunCreate(),
                        param => true
                    );
                }
                return _runCreateCommand;
            }
        }

        public ICommand RunDeleteCommand
        {
            get
            {
                if (_runDeleteCommand == null)
                {
                    _runDeleteCommand = new RelayCommand(
                        param => RunDelete(param as Employee),
                        param => true
                    );
                }
                return _runDeleteCommand;
            }
        }



       
        public ICommand RunEditCommand
        {
            get
            {
                if (_runEditCommand == null)
                {
                    _runEditCommand = new RelayCommand(
                        param => RunEdit(param as Employee),
                        param => true
                    );
                }
                return _runEditCommand;
            }
        }


        private void RunCreate()
        {

             
                var employeeWindow = new EmployeeWindow();


                employeeWindow.DataContext = new EmployeeWindowViewModel(_database, new Employee());
                employeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                employeeWindow.ShowDialog();


                RunSearch();
             

          

        }




        private void RunDelete(Employee employee)
        {

            
                if (InforCenter.IsInTestMode)
                {
                    new EmployeeBL(_database).DeleteEmployee(employee);
                    RunSearch();

                }

                else if (ConfirmDeleteEmployee())
                {

                    new EmployeeBL(_database).DeleteEmployee(employee);
                    RunSearch();

                }
            

            

        }


        private void RunEdit(Employee employee)
        {
               
                    var employeeWindow = new EmployeeWindow();


                    employeeWindow.DataContext = new EmployeeWindowViewModel(_database, employee);
                    employeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    employeeWindow.ShowDialog();

                    RunSearch();
              

        }

        

        private bool ConfirmDeleteEmployee()
        {

            var  messageBoxText = "Are you sure you want to delete the selected employee?";

            var  caption = "Confirm";

            var  button = MessageBoxButton.YesNo;

            var  icon = MessageBoxImage.Question;

            var  result = MessageBox.Show(messageBoxText, caption, button, icon);

 
            if (result == MessageBoxResult.Yes)
            {

                return true;

            }

            return false;

        }

        private   void RunSearch()
        {
            Action proc = ()=> {


               var  criteria = new List<Expression<Func<Employee,bool>>>();

               if (!string.IsNullOrWhiteSpace(FirstNameSearch))
               {
                   criteria.Add( e=>e.FirstName.StartsWith(FirstNameSearch));

               }

               if (!string.IsNullOrWhiteSpace(LastNameSearch))
               {
                 
                   criteria.Add(e => e.LastName.StartsWith(LastNameSearch));

               }

               if (ActiveSearch.ToLower() == "yes")
               {
                   criteria.Add(e => e.Active == true);
                
               }

               if (ActiveSearch.ToLower() == "no")
               {
                   criteria.Add( e => e.Active ==false);
                
               }

               if (DepartmentSearch != -1)
               {

                   criteria.Add(e => e.Department.Id==DepartmentSearch);
                
      
               }

               EmployeesFound = new EmployeeBL(_database).Search(criteria).ToObservable(); 
              
            };

            SharedCode.Try(proc);


        }


     

       
    }
}
