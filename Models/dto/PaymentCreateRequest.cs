using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class PaymentCreateRequest
    {
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