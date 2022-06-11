using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display (Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display (Name ="Last Name")]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        [Remote (action:"IsEmailInUse" , controller: "Starting")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name= "Mobile Number")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        public string city { get; set; }

        public int Id { get; set; }
    }
}
