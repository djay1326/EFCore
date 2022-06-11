using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class leave
    {
        public int leaveid { get; set; }
        [Display(Name = "AspNetUserRoles")]
        public virtual int? userid { get; set; }
        public int? roleid { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public int? statusid { get; set; }

        //public int? statuspos { get; set; }

        [Required]
        public string reason { get; set; }

        [ForeignKey("userid")]
        public virtual User AspNetUsers { get; set; }

        
    }
}
