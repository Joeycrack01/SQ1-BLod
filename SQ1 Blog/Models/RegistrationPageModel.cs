using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Models
{
    public class RegistrationPageModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter your email address")]
        [Display(Name = "Email")]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Enter a username", MinimumLength = 2)]
        [Display(Name = "UserName")]
        [BindProperty]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [BindProperty]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
