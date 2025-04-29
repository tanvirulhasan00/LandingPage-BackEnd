using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LandingPage.Models.Dto
{
    public class UserInfoUpdateReqDto
    {
        [Required]
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? Active { get; set; }

    }
}