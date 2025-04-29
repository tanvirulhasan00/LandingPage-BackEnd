using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class OrderCreateRequest
    {
        public string ProductId { get; set; }
        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
        public string DeliveryMethod { get; set; }
        public string? DeliveryStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string TotalPrice { get; set; }

        public int Quantity { get; set; }

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