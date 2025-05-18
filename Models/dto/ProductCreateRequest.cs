using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ProductDescription { get; set; }
        public int Price { get; set; }
        public IFormFile? ProductImageUrl { get; set; }
        public string UserId { get; set; }

    }
}