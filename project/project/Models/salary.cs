using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class salary
    {
        [Key]
        public int salaryid { get; set; }

        public int? basic { get; set; }

        public int? tax { get; set; }

        public int? final { get; set; }

        public DateTime? createddate { get; set; }

        public virtual int? userid { get; set; }

        public string username { get; set; }

        [ForeignKey("userid")]
        public virtual User AspNetUsers { get; set; }

        [NotMapped]
        public string date { get; set; }

        [NotMapped]
        public int increment { get; set; }


    }
}
