using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LandingPage.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Results { get; set; }
    }
}