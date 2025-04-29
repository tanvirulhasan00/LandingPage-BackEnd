using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionStatus { get; set; }
        public string MerchantInvoiceNumber { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Intent { get; set; }
        public DateTime CreateDate { get; set; }
        public string OrderNumber { get; set; }

    }
}