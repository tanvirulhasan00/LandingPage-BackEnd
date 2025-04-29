using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.bkash.createpayment
{
    public class CreatePaymentRequestDto
    {
        public string Amount { get; set; }
        public string Token { get; set; }

    }
}