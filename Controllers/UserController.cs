using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LandingPage.Models;
using LandingPage.Models.Dto;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/user")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        [HttpGet]
        [Route("getall")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> GetAllUser(CancellationToken cancellationToken)
        {
            var response = new ApiResponse();
            var genericReq = new GenericRequest<ApplicationUser>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var users = await _unitOfWork.User.GetAllAsync(genericReq);
                if (users == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found";
                    response.Results = users;
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Sucessful";
                response.Results = users;
                return response;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet]
        [Route("get")]
        [Authorize(Roles = "admin,user,guest")]
        public async Task<ApiResponse> GetUser(string Id, CancellationToken cancellationToken)
        {
            var response = new ApiResponse();
            var genericReq = new GenericRequest<ApplicationUser>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var user = await _unitOfWork.User.GetAsync(genericReq);
                user.Password = "";
                user.PasswordHash = "";
                if (user == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Sucessful";
                response.Results = user;
                return response;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> UpdateUser(UserInfoUpdateReqDto userDto, CancellationToken cancellationToken)
        {
            var response = new ApiResponse();
            if (userDto.Id == "" || userDto.Id == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "userId is not provided";
                return response;
            }
            try
            {
                var genericReq = new GenericRequest<ApplicationUser>
                {
                    Expression = x => x.Id == userDto.Id,
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var user = await _unitOfWork.User.GetAsync(genericReq);
                if (user == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"user not found with the id {userDto.Id}";
                    return response;
                }
                if (userDto.ImageUrl != null)
                {
                    if (!string.IsNullOrEmpty(user.ImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(user.ImageUrl);
                    }
                }

                var imageUrl = "";
                if (userDto.ImageUrl != null)
                {
                    imageUrl = await _unitOfWork.File.FileUpload(userDto.ImageUrl, "images");
                }

                user.UserName = (userDto.UserName == null || userDto.UserName == "") ? user.UserName : userDto.UserName;
                user.PhoneNumber = (userDto.PhoneNumber == null || userDto.PhoneNumber == "") ? user.PhoneNumber : userDto.PhoneNumber;
                user.Email = (userDto.Email == null || userDto.Email == "") ? user.Email : userDto.Email;
                user.Address = (userDto.Address == null || userDto.Address == "") ? user.Address : userDto.Address;
                user.ImageUrl = userDto.ImageUrl == null ? user.ImageUrl : imageUrl;
                _unitOfWork.User.Update(user);
                int res = await _unitOfWork.Save();
                if (res <= 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong while updating user.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "User updated successfully";
                return response;

            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;

            }
        }

        [HttpPost]
        [Route("update-status")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdateUserStatus(UserStatusReqDto userDto, CancellationToken cancellationToken)
        {
            var response = new ApiResponse();
            if (userDto.Id == "" || userDto.Id == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "userId is not provided";
                return response;
            }
            try
            {
                var genericReq = new GenericRequest<ApplicationUser>
                {
                    Expression = x => x.Id == userDto.Id,
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var user = await _unitOfWork.User.GetAsync(genericReq);
                if (user == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"user not found with the id {userDto.Id}";
                    return response;
                }

                user.Active = (userDto.Active == null || userDto.Active == "") ? user.Active : int.Parse(userDto.Active);
                _unitOfWork.User.Update(user);
                int res = await _unitOfWork.Save();
                if (res <= 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong while deactivating user.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "User status changed successfully";
                return response;

            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;

            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteUser(List<string> userIds, CancellationToken cancellationToken)
        {
            var response = new ApiResponse();
            if (userIds.Count == 0 || userIds == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Unsuccessful - No UserIDs provided";
                return response;
            }
            try
            {
                var genericReq = new GenericRequest<ApplicationUser>
                {
                    Expression = x => userIds.Contains(x.Id.ToString()),
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var userData = await _unitOfWork.User.GetAllAsync(genericReq);
                if (userData == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"Unsuccessful - user not found with the id {userIds}";
                    return response;
                }

                foreach (var user in userData)
                {
                    if (!string.IsNullOrEmpty(user.ImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(user.ImageUrl);
                    }
                }

                _unitOfWork.User.RemoveRange(userData);
                int res = await _unitOfWork.Save();
                if (res == 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong while deleting user";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = $"{userData.Count} User(s) deleted Successfully";
                return response;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;

            }
        }

    }
}