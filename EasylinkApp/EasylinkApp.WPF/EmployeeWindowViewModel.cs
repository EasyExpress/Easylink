using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using Easylink;
using EasylinkApp.Business;
 


namespace EasylinkApp.WPF
{
    public class EmployeeWindowViewModel : ObservableObject
    {

        private IDatabase _database;

        private Employee  _employee;
 
       
        public string   FirstName
        {
            get { return _employee.FirstName; }
            set
            {
                _employee.FirstName = value;

                RaisePropertyChanged(() =>FirstName);
            }
        } 


        public string   LastName
        {
            get { return _employee.LastName; }
            set
            {
                _employee.LastName = value;

                RaisePropertyChanged(() =>LastName);
            }
        } 



        public decimal Salary
        {
            get { return _employee.Salary; }

            set
            {
       
                   _employee.Salary = value;

                    RaisePropertyChanged(() => this.Salary);
 
            }

        }


        public bool Active 
        {
            get { return _employee.Active; }

            set
            {
                  _employee.Active = value;

                   RaisePropertyChanged(() =>Active);
  
            }

        }


        
        public DateTime? EmployedSince
        {
            get { return _employee.EmployedSince; }

            set
            {
                _employee.EmployedSince = value;

                   RaisePropertyChanged(() =>EmployedSince);
  
            }

        }


        public int? Age
        {
            get { return _employee.Age; }

            set
            {
                _employee.Age = value;

                   RaisePropertyChanged(() =>Age);
  
            }

        }



        public decimal? Weight
        {
            get { return _employee.Weight; }

            set
            {
                _employee.Weight = value;

                RaisePropertyChanged(() => Weight);

            }

        }



        public bool? Married
        {
            get { return  _employee.Married; }

            set
            {
                _employee.Married = value;

                RaisePropertyChanged(() => Married);

            }

        }



        public Int64  Department
        {
            get { return _employee.Department.Id; }

            set
            {
                _employee.Department.Id = value;

                RaisePropertyChanged(() => Department);

            }

        }

        private static List<Lookup> _departments;
        public List<Lookup> Departments
        {
            get
            {
                if (_departments == null)
                {
                    
                    _departments= new LookupBL(_database).RetrieveLookupsByGroupName("Department");

                }

                return _departments;

            }
        }

        private string _validationResult;

        public string ValidationResult
        {
            get { return _validationResult; }

            set
            {

                _validationResult = value;

                RaisePropertyChanged(() => this.ValidationResult);

            }

        }

       

        public EmployeeWindowViewModel(IDatabase database, Employee employee=null)
        {
            _database = database;

            _employee = employee; 

            if (_employee == null)
            {
                 _employee = new Employee();
            }

        }



        private ICommand _runSaveCommand;

        private ICommand _runCancelCommand;
       
        public ICommand RunSaveCommand
        {
            get
            {
                if (_runSaveCommand == null)
                {
                    _runSaveCommand = new RelayCommand(
                        param => RunSave(param as Window),
                        param => true
                    );
                }
                return _runSaveCommand;
            }
        }




        public ICommand RunCancelCommand
        {
            get
            {
                if (_runCancelCommand == null)
                {
                    _runCancelCommand = new RelayCommand(
                        param => RunCancel(param as Window),
                        param => true
                    );
                }
                return _runCancelCommand;
            }
        }


        private void RunSave(Window window)
        {

            var rules = _employee.ValidateRules();

            if (rules.Count > 0)
            {
                ValidationResult = rules[0].ErrorMessage;
                return;
            }

            if (_employee.LifeCycleStatus == BusinessBaseLifeCycle.New)
            {
                new EmployeeBL(_database).InsertEmployee(_employee);
            }
            else
            {
                new EmployeeBL(_database).UpdateEmployee(_employee);

            }

            if (window != null)
            {
                window.Close(); 
            }
           

           

        }


        private void RunCancel(Window window)
        {
            window.Close();

        }




    }
}
