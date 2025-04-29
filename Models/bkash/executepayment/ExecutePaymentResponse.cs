using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.bkash.executepayment
{
    public class ExecutePaymentResponse : ErrorResponse
    {
        public string paymentID { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string trxID { get; set; }
        public string transactionStatus { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string intent { get; set; }
        public string merchantInvoiceNumber { get; set; }
    }

    public class ErrorResponse
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}