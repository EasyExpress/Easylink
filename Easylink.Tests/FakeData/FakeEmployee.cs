using System;
using System.Collections.Generic;
using Easylink.Tests.Business;
using Easylink.Tests.Model;

namespace Easylink.Tests
{
    internal static class FakeEmployee
    {
        public static Employee GetFakeEmployee(IDatabase database)
        {
           var program1 = database.RetrieveObject<Program>( p => p.Name == "Kitchen Life");
           var  program2 = database.RetrieveObject<Program>(p => p.Name == "Gardening");

            return new Employee
                {
                    FirstName = "david",
                    LastName = "smith",
                    Identifier =  Guid.NewGuid(),
                    Salary = 12045.27M,
                    EmployedSince = new DateTime(2010, 3, 12),
 
                    Active = true,
                    Age=41,
                    Department = new LookupBL(database).RetrieveLookupByName(Department.IT.ToString()),
                    Role = new LookupBL(database).RetrieveLookupByName(Role.Employee.ToString()),
                    Email = "davg@hotmail.com",
                    Phone = "403-245-4781",
                    Address = new Address
                        {
                            Country = new LookupBL(database).RetrieveLookupByName(Country.China.ToString()),
                            HouseNumber = "195",
                            PostalCode = "T2Y 3S9",
                            StreetName = "Bridlewood Way"
                        },
                    Programs = new List<EmployeeProgram>
                        {
                            new EmployeeProgram {ProgramId = program1.Id, Status = ProgramStatus.Registered},
                            new EmployeeProgram {ProgramId = program2.Id, Status = ProgramStatus.Completed},
                        }
                };
        }


        public static Employee GetFakeEmployee1(IDatabase database)
        {
            var  program1 = database.RetrieveObject<Program>(p => p.Name == "Kitchen Life");
            var  program2 = database.RetrieveObject<Program>(p => p.Name == "Gardening");


            return new Employee
                {
                    FirstName = "david",
                    LastName = "smith",
                    Salary = 12045.27M,
                    EmployedSince = new DateTime(2010, 3, 12),
                    Active = true,
                    Department = new LookupBL(database).RetrieveLookupByName(Department.IT.ToString()),
                    Role = new LookupBL(database).RetrieveLookupByName(Role.Employee.ToString()),
                    Address = new Address {HouseNumber = "31", PostalCode = "T2Y WS2", StreetName = "Centre Street"},
                    Programs = new List<EmployeeProgram>
                        {
                            new EmployeeProgram {ProgramId = program1.Id, Status = ProgramStatus.Registered},
                            new EmployeeProgram {ProgramId = program2.Id, Status = ProgramStatus.Completed},
                        }
                };
        }


        public static List<Employee> GetFakeEmployees(IDatabase database)
        {
            var   program1 = database.RetrieveObject<Program>(p => p.Name == "IT Infrastructure");
            var   program2 = database.RetrieveObject<Program>(p => p.Name == "Kitchen Life");
            var   program3 = database.RetrieveObject<Program>(p => p.Name ==  "Gardening");
            var   program4 = database.RetrieveObject<Program>(p => p.Name == "Financial Planning");


            return new List<Employee>
                {
                    new Employee
                        {
                            FirstName = "david",
                            LastName = "smith",
                            Salary = 12045.27M,
                            EmployedSince = new DateTime(2010, 3, 12),
                            Active = true,
                            Department = new LookupBL(database).RetrieveLookupByName(Department.IT.ToString()),
                            Programs = new List<EmployeeProgram>
                                {
                                    new EmployeeProgram {ProgramId = program1.Id, Status = ProgramStatus.Registered},
                                    new EmployeeProgram {ProgramId = program3.Id, Status = ProgramStatus.Completed},
                                }
                        },
                    new Employee
                        {
                            FirstName = "vincient",
                            LastName = "john",
                            Salary = 11045.39M,
                            EmployedSince = new DateTime(2011, 5, 6),
                            Active = false,
                            Department = new LookupBL(database).RetrieveLookupByName(Department.Admin.ToString()),
                            Programs = new List<EmployeeProgram>
                                {
                                    new EmployeeProgram {ProgramId = program2.Id, Status = ProgramStatus.Completed},
                                    new EmployeeProgram {ProgramId = program4.Id, Status = ProgramStatus.InProgress},
                                }
                        }
                };
        }
    }
}