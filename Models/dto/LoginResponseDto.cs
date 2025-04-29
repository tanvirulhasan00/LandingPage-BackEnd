using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.Dto
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}