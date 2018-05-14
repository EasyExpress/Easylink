


using Easylink;
using EasylinkApp.Business;
 

namespace EasylinkApp.Tests
{
    static class SharedCode
    {


        public static Employee  InstallEmployee(IDatabase database)
        {

            var employee = FakeEmployee.GetFakeEmployee(database);

            new EmployeeBL(database).InsertEmployee(employee);

            return employee;

        }


       

    }
}
