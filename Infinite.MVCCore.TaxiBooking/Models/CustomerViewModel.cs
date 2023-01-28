using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage ="Please provide name")]
        [Display(Name ="Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Please provide Mobile Number")]
        [Phone]
        [Display(Name ="Mobile Number")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage ="Please provide Email Id")]
        [EmailAddress]
        [Display(Name ="Email Id")]
        public string EmailId { get; set; }
        [Required(ErrorMessage ="Please provide address")]
        public string Address { get; set; }
    }
}
