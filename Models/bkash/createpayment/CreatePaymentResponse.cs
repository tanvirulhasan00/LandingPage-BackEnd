using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.bkash.createpayment
{
    public class CreatePaymentResponse
    {

        public string paymentID { get; set; }
        public string bkashURL { get; set; }
        public string callbackURL { get; set; }
        public string successCallbackURL { get; set; }
        public string failureCallbackURL { get; set; }
        public string cancelledCallbackURL { get; set; }
        public string payerReference { get; set; }
        public string agreementStatus { get; set; }
        public string agreementCreateTime { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
    }
}