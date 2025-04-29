using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models
{
    public class ProductDefination
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ProductSizeId { get; set; }
        [ForeignKey("ProductSizeId")]
        public ProductSize ProductSize { get; set; }
    }
}