using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LandingPage.Models.Dto
{
    public class RegistrationReqDto
    {
        public string? UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? Active { get; set; }

    }
}