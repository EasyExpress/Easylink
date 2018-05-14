using System;

namespace Easylink.Tests.Model
{
    public class FinancialInfo : BusinessBase
    {
        public Int64? Id { get; set; }

        public Int64 EmployeeId { get; set; }

        public string Bank { get; set; }
    }
}