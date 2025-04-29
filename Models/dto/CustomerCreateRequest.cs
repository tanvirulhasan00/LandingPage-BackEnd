using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class CustomerCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string Comment { get; set; }
    }
}