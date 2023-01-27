using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Models
{
    public class EmployeeRosterViewModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int EmployeeId { get; set; }
    }
}
