using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class OrderUpdateRequest
    {
        public int Id { get; set; }
        public string Status { get; set; }

    }
}