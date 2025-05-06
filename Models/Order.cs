using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
        public int TotalPrice { get; set; }

        public string DeliveryMethod { get; set; }
        public string DeliveryStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public int Quantity { get; set; }

        public string? PaymentAccountNumber { get; set; }
        public string? TransactionId { get; set; }

        public string? FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }

    }
}