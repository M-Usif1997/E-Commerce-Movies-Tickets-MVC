using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.ViewModels
{
    public class RegisterVM
    {

        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Username is required")]
        [Remote(action: "IsUsernameInUse", controller: "Account")]

        [MaxLength(50, ErrorMessage = "Name Cannot Be More Than 50 Character")]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Remote(action: "CheckUniqueEmailAddress", controller: "Account")]
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
