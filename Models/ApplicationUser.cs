using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LandingPage.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? Active { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}