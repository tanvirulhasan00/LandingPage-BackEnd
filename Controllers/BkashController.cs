using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models.bkash.createpayment;
using LandingPage.Models.bkash.executepayment;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/bkash")]
    [ApiVersion("1.0")]
    public class BkashController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BkashController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("get-token")]
        public async Task<IActionResult> GetGrantToken()
        {
            try
            {
                var data = await _unitOfWork.Bkash.GrantTokenAsync();
                return Ok(data);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return BadRequest(ex.InnerException.Message);
                else return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create-payment")]
        public async Task<IActionResult> CreatePayment(CreatePaymentRequestDto req)
        {
            try
            {
                var data = await _unitOfWork.Bkash.CreatePaymentAsync(req);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return BadRequest(ex.InnerException.Message);
                else return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("execute-payment")]
        public async Task<IActionResult> ExecutePayment(ExecutePaymentRequest req)
        {
            try
            {
                var data = await _unitOfWork.Bkash.ExecutePaymentAsync(req);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return BadRequest(ex.InnerException.Message);
                else return BadRequest(ex.Message);
            }
        }

    }
}