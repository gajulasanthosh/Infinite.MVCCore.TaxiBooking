using System.ComponentModel.DataAnnotations;

namespace Infinite.MVCCore.TaxiBooking.Models
{
    public class LoginViewModel
    {
        [Required]
        public string  LoginID { get; set; }
        [Required]
        public string Password { get; set; }    
    }
}
