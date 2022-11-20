using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab10.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }

        [Display(Name="Photo")]
        [NotMapped]
        public IFormFile FormFile { get; set; }

        [MaxLength(100)]
        public string FileName { get; set; } = "/image/default-product.jpg";

    }
}
