using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.bkash.createpayment
{
    public class CreatePaymentRequest
    {
        public string amount { get; set; }
        public string token { get; set; }

    }
}