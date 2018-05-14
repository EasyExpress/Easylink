using System;
using Easylink;


namespace EasylinkApp.Business
{
    public  class EmployeeProgram :BusinessBase 
    {

        public Int64 Id { get; set; }

        public Int64 ProgramId { get;  set; }

        public string  ProgramName { get; set; }

        public Int64 EmployeeId { get; set; }

        public ProgramStatus Status { get; set; }

    }
}
