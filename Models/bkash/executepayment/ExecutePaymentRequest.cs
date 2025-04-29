using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.bkash.executepayment
{
    public class ExecutePaymentRequest
    {
        public string PaymentID { get; set; }
        public string Token { get; set; }
    }
}