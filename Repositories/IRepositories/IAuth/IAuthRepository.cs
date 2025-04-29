using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models;
using LandingPage.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace LandingPage.Repositories.IRepositories.IAuth
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string phoneNumber);
        Task<ApiResponse> Login(LoginRequestDto request);
        Task<ApiResponse> Registration(RegistrationReqDto request);
        Task<ApiResponse> ResetPassword(ResetPassReqDto request);
        Task<ApiResponse> UpdatePassword(UpdatePassReqDto request);
        // Task<ApiResponse> UpdateUserInfo(UserInfoUpdateReqDto request);
    }
}