using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LandingPage.Models.Dto
{
    public class UserStatusReqDto
    {
        [Required]
        public string Id { get; set; }
        public string? Active { get; set; }

    }
}