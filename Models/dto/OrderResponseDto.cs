using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class OrderResponseDto
    {
        public string OrderNumber { get; set; }
        public string DeliveryMethod { get; set; }
        public string? DeliveryStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string TotalPrice { get; set; }
        public int Quantity { get; set; }

    }
}