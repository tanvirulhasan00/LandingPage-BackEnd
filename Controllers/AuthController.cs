using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LandingPage.Models;
using LandingPage.Models.Dto;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Route("login")]
        public ApiResponse Login(LoginRequestDto request)
        {
            // username = admin
            // password = aDmin@00#
            var response = new ApiResponse();
            var user = _unitOfWork.Auth.Login(request);
            response = user.Result;
            return response;
        }

        [HttpPost]
        [Route("registration")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Registration(RegistrationReqDto request)
        {
            var response = new ApiResponse();

            var isUniqueUser = _unitOfWork.Auth.IsUniqueUser(request.PhoneNumber);
            if (isUniqueUser == false)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "User already exists with this phone number";
                return response;
            }
            if (request.UserName == null || request.UserName == "" || request.Password == null || request.Password == "")
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "User name and password can not be empty or null.";
                return response;
            }
            var PasswordRegex = @"^(?=(.*[A-Z]))(?=(.*\d))(?=(.*\W))(?=.{6,})[A-Za-z\d\W]*$";
            var regex = new Regex(PasswordRegex);
            var validPassword = regex.IsMatch(request.Password);
            if (!validPassword)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Password must be at least 6 characters long, include at least one uppercase letter, one digit, and one non-alphanumeric character.";
                return response;
            }
            response = await _unitOfWork.Auth.Registration(request);
            await _unitOfWork.Save();

            return response;
        }

        [HttpPost]
        [Route("reset-credentials")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> ResetPassword([FromBody] ResetPassReqDto request)
        {
            var response = new ApiResponse();
            return response;
        }

        [HttpPost]
        [Route("update-credentials")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> UpdatePassword(UpdatePassReqDto request)
        {
            var response = new ApiResponse();
            if (request.NewPassword != request.ConfirmPassword)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Password does not match";
                return response;
            }
            var res = await _unitOfWork.Auth.UpdatePassword(request);
            await _unitOfWork.Save();
            response = res;
            return response;
        }


    }
}