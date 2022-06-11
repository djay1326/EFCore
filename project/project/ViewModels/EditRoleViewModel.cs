using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()  // constructor to pass list of Users
        {
            Users = new List<string>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }  // here we are passing a list of values so declared constructor above 

       
    }
}
