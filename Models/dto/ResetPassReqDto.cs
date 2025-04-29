using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.Dto
{
    public class ResetPassReqDto
    {
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}