using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please provide name")]
        [Display(Name ="Employee Name")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage ="Please provide destination")]
        public string Designation { get; set; }
        [Required(ErrorMessage ="Please provide Mobile Number")]
        [Display(Name ="Mobile Number")]
        public string PhoneNo { get; set; }
        [Required]
        [Display(Name ="Email Id")]
        public string EmailId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name ="Driving License Number")]
        public string DrivingLicenseNo { get; set; }
    }
}
