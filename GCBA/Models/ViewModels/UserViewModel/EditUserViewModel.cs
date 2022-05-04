using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCBA.Models.ViewModels.UserViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required, MinLength(6)]
        [RegularExpression(@"^[ a-zA-Z0-9]+$", ErrorMessage = "Username should only contain characters and numbers"), MaxLength(40)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your Full Name")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Name should only contain characters and white spaces"), MaxLength(80)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address"), MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone Number")]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid phone number"), MinLength(11), MaxLength(16)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select a branch")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }


        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Role")]
        public int RoleID { get; set; }
    }
}