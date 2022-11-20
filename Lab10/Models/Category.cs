using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
