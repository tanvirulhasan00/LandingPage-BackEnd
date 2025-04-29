using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models.bkash.createpayment;
using LandingPage.Models.bkash.executepayment;
using LandingPage.Models.bkash.granttoken;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Repositories.IRepositories
{
    public interface IBkashRepository
    {
        Task<GrantTokenResponse> GrantTokenAsync();
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequestDto request);

        Task<ExecutePaymentResponse> ExecutePaymentAsync(ExecutePaymentRequest request);
    }
}