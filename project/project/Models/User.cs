using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace project.Models
{
    public class User : IdentityUser<int>
    {
        public override string Email { get; set; }
        public string city { get; set; }
        public string FirstName { get; set; }

        public int managerid { get; set; }

        public string manager {get; set;}
    }
}
