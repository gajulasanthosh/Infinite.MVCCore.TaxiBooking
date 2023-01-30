using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infinite.MVCCore.TaxiBooking.Models
{
    public class LoginViewModel
    {
        [Required]
        public string LoginID { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public int? CustomerID { get; set; }
        [Required(ErrorMessage = "Please enter valid Gmail Id")]
        [Display(Name = "Email Id")]
        [EmailAddress]
        public string LoginID { get; set; }
        [Required(ErrorMessage = "Please enter valid password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Please enter atleast 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter valid password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Please enter atleast 8 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please select Role")]
        public string Role { get; set; }
    }

    public class RolesRegisterVM
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public RegisterViewModel RegisterRoles { get; set; }

    }
}
