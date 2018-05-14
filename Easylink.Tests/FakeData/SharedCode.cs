using Easylink.Tests.Business;
using Easylink.Tests.Model;

namespace Easylink.Tests
{
    internal static class SharedCode
    {
        public static Employee InstallEmployee(IDatabase database)
        {
            Employee employee = FakeEmployee.GetFakeEmployee(database);

            new EmployeeBL(database).InsertEmployee(employee);

            return employee;
        }


        public static Invoice InstallInvoice(IDatabase database)
        {
            Invoice invoice = FakeInvoice.GetFakeInvoice();

            database.Insert(invoice);

            return invoice;
        }


        public static AspNetRole  InstallAspNetRole(IDatabase database)
        {
            var  aspNetRole  = FakeAspNetRole.GetFakeAspNetRole();

            database.Insert(aspNetRole);

            return aspNetRole;
        }


        public static void UninstallEmployee(Employee employee, IDatabase database)
        {
            new EmployeeBL(database).DeleteEmployee(employee);
        }
    }
}