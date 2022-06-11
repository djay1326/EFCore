using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class holidays
    {
        [Key]
        public int Id { get; set; }

        public string holiday { get; set; }

        public DateTime onDate { get; set; }
    }
}
