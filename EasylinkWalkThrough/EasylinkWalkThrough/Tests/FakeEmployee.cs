 

namespace EasylinkWalkThrough.Tests
{
    public class FakeEmployee
    {
        public static Employee GetFakeEmployee()
        {
            return new Employee()
            {
                FirstName = "Mark",
                LastName = "Loren",
                LoginId ="MLoren",
                Active = true,
                Role =  new Lookup {Id=2}
            };

        }

    }
}


