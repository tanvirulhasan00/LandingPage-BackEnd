using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using LandingPage.Models;
using LandingPage.Models.dto;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/company-profile")]
    [ApiVersion("1.0")]
    public class CompanyInfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse response;
        public CompanyInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllCompanyInfo(string user, CancellationToken cancellationToken)
        {
            try
            {
                var companyInfo = await _unitOfWork.CompanyInfo.GetAllAsync(new GenericRequest<CompanyInfo>
                {
                    Expression = string.IsNullOrWhiteSpace(user) || user == "all" ? null : x => x.UserId == user,
                    NoTracking = true,
                    IncludeProperties = "User",
                    CancellationToken = cancellationToken
                });
                if (companyInfo == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = companyInfo;
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
        public async Task<ApiResponse> GetCompanyInfo(string Id, CancellationToken cancellationToken)
        {
            try
            {
                if (Id.Length < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid id required";
                    return response;
                }
                var companyInfo = await _unitOfWork.CompanyInfo.GetAsync(new GenericRequest<CompanyInfo>
                {
                    Expression = x => x.UserId == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (companyInfo == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = companyInfo;
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
        [Route("create")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> CreateCompanyInfo(CompanyInfoCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var companyInfo = await _unitOfWork.CompanyInfo.GetAsync(new GenericRequest<CompanyInfo>
                {
                    Expression = x => x.UserId == request.UserId,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (companyInfo != null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "You can't create more then one company profile.";
                    return response;
                }
                var companyLogoUrl = "";
                var reviewImageUrlOne = "";
                var reviewImageUrlTwo = "";
                var reviewImageUrlThree = "";

                if (request.CompanyLogoUrl != null)
                {
                    companyLogoUrl = await _unitOfWork.File.FileUpload(request.CompanyLogoUrl, "images");
                }
                if (request.ReviewImageUrlOne != null)
                {
                    reviewImageUrlOne = await _unitOfWork.File.FileUpload(request.ReviewImageUrlOne, "images");
                }
                if (request.ReviewImageUrlTwo != null)
                {
                    reviewImageUrlTwo = await _unitOfWork.File.FileUpload(request.ReviewImageUrlTwo, "images");
                }
                if (request.ReviewImageUrlThree != null)
                {
                    reviewImageUrlThree = await _unitOfWork.File.FileUpload(request.ReviewImageUrlThree, "images");
                }
                CompanyInfo companyInfoToCreate = new()
                {
                    CompanyName = request.CompanyName,
                    CompanyDescription = request.CompanyDescription,
                    CompanyPhoneNumber = request.CompanyPhoneNumber,
                    CompanyEmail = request.CompanyEmail,
                    ReviewTitle = request.ReviewTitle,
                    ReviewShortDes = request.ReviewShortDes,
                    ReviewLongDes = request.ReviewLongDes,
                    CompanyLogoUrl = companyLogoUrl,
                    ReviewImageUrlOne = reviewImageUrlOne,
                    ReviewImageUrlTwo = reviewImageUrlTwo,
                    ReviewImageUrlThree = reviewImageUrlThree,
                    InsideDhaka = int.Parse(request.InsideDhaka),
                    OutsideDhaka = int.Parse(request.OutsideDhaka),
                    UserId = request.UserId,
                };
                await _unitOfWork.CompanyInfo.AddAsync(companyInfoToCreate);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Company profile creation failed.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.Created;
                response.Message = "Company profile created successfully.";
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
        public async Task<ApiResponse> UpdateCompanyInfo(CompanyInfoUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var genericReq = new GenericRequest<CompanyInfo>
                {
                    Expression = x => x.Id == request.Id,
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var companyInfoToUpdate = await _unitOfWork.CompanyInfo.GetAsync(genericReq);
                if (companyInfoToUpdate == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"Data not found";
                    return response;
                }
                if (request.CompanyLogoUrl != null)
                {
                    if (!string.IsNullOrEmpty(companyInfoToUpdate.CompanyLogoUrl))
                    {
                        _unitOfWork.File.DeleteFile(companyInfoToUpdate.CompanyLogoUrl);
                    }
                    if (!string.IsNullOrEmpty(companyInfoToUpdate.ReviewImageUrlOne))
                    {
                        _unitOfWork.File.DeleteFile(companyInfoToUpdate.ReviewImageUrlOne);
                    }
                    if (!string.IsNullOrEmpty(companyInfoToUpdate.ReviewImageUrlTwo))
                    {
                        _unitOfWork.File.DeleteFile(companyInfoToUpdate.ReviewImageUrlTwo);
                    }
                    if (!string.IsNullOrEmpty(companyInfoToUpdate.ReviewImageUrlThree))
                    {
                        _unitOfWork.File.DeleteFile(companyInfoToUpdate.ReviewImageUrlThree);
                    }
                }

                var companyLogoUrl = "";
                var reviewImageUrlOne = "";
                var reviewImageUrlTwo = "";
                var reviewImageUrlThree = "";

                if (request.CompanyLogoUrl != null)
                {
                    companyLogoUrl = await _unitOfWork.File.FileUpload(request.CompanyLogoUrl, "images");
                }
                if (request.ReviewImageUrlOne != null)
                {
                    reviewImageUrlOne = await _unitOfWork.File.FileUpload(request.ReviewImageUrlOne, "images");
                }
                if (request.ReviewImageUrlTwo != null)
                {
                    reviewImageUrlTwo = await _unitOfWork.File.FileUpload(request.ReviewImageUrlTwo, "images");
                }
                if (request.ReviewImageUrlThree != null)
                {
                    reviewImageUrlThree = await _unitOfWork.File.FileUpload(request.ReviewImageUrlThree, "images");
                }

                companyInfoToUpdate.CompanyName = (request.CompanyName == null || request.CompanyName == "") ? companyInfoToUpdate.CompanyName : request.CompanyName;
                companyInfoToUpdate.CompanyDescription = (request.CompanyDescription == null || request.CompanyDescription == "") ? companyInfoToUpdate.CompanyDescription : request.CompanyDescription;
                companyInfoToUpdate.CompanyPhoneNumber = (request.CompanyPhoneNumber == null || request.CompanyPhoneNumber == "") ? companyInfoToUpdate.CompanyPhoneNumber : request.CompanyPhoneNumber;
                companyInfoToUpdate.CompanyEmail = (request.CompanyEmail == null || request.CompanyEmail == "") ? companyInfoToUpdate.CompanyEmail : request.CompanyEmail;
                companyInfoToUpdate.ReviewTitle = (request.ReviewTitle == null || request.ReviewTitle == "") ? companyInfoToUpdate.ReviewTitle : request.ReviewTitle;
                companyInfoToUpdate.ReviewShortDes = (request.ReviewShortDes == null || request.ReviewShortDes == "") ? companyInfoToUpdate.ReviewShortDes : request.ReviewShortDes;
                companyInfoToUpdate.ReviewLongDes = (request.ReviewLongDes == null || request.ReviewLongDes == "") ? companyInfoToUpdate.ReviewLongDes : request.ReviewLongDes;
                companyInfoToUpdate.InsideDhaka = (request.InsideDhaka == null || request.InsideDhaka == "") ? companyInfoToUpdate.InsideDhaka : int.Parse(request.InsideDhaka);
                companyInfoToUpdate.OutsideDhaka = (request.OutsideDhaka == null || request.OutsideDhaka == "") ? companyInfoToUpdate.OutsideDhaka : int.Parse(request.OutsideDhaka);
                companyInfoToUpdate.CompanyEmail = (request.CompanyEmail == null || request.CompanyEmail == "") ? companyInfoToUpdate.CompanyEmail : request.CompanyEmail;
                companyInfoToUpdate.CompanyLogoUrl = request.CompanyLogoUrl == null ? companyInfoToUpdate.CompanyLogoUrl : companyLogoUrl;
                companyInfoToUpdate.ReviewImageUrlOne = request.ReviewImageUrlOne == null ? companyInfoToUpdate.ReviewImageUrlOne : reviewImageUrlOne;
                companyInfoToUpdate.ReviewImageUrlTwo = request.ReviewImageUrlTwo == null ? companyInfoToUpdate.ReviewImageUrlTwo : reviewImageUrlTwo;
                companyInfoToUpdate.ReviewImageUrlThree = request.ReviewImageUrlThree == null ? companyInfoToUpdate.ReviewImageUrlThree : reviewImageUrlThree;

                _unitOfWork.CompanyInfo.Update(companyInfoToUpdate);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Failed to update.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Company profile updated successfully.";
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

        [HttpDelete("delete")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> DeleteCompanyInfo(int Id, CancellationToken cancellationToken)
        {
            try
            {
                if (Id <= 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid id required";
                    return response;
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                var companyInfo = await _unitOfWork.CompanyInfo.GetAsync(new GenericRequest<CompanyInfo>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });

                if (companyInfo == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Company profile not found.";
                    return response;
                }

                // Check if the user is an admin or the owner of the product
                if (userRole == "admin" || (userRole == "user" && companyInfo.UserId == userId))
                {
                    // Delete the image file if it exists
                    if (!string.IsNullOrEmpty(companyInfo.CompanyLogoUrl))
                    {
                        _unitOfWork.File.DeleteFile(companyInfo.CompanyLogoUrl);
                    }
                    if (!string.IsNullOrEmpty(companyInfo.ReviewImageUrlOne))
                    {
                        _unitOfWork.File.DeleteFile(companyInfo.ReviewImageUrlOne);
                    }
                    if (!string.IsNullOrEmpty(companyInfo.ReviewImageUrlTwo))
                    {
                        _unitOfWork.File.DeleteFile(companyInfo.ReviewImageUrlTwo);
                    }
                    if (!string.IsNullOrEmpty(companyInfo.ReviewImageUrlThree))
                    {
                        _unitOfWork.File.DeleteFile(companyInfo.ReviewImageUrlThree);
                    }

                    _unitOfWork.CompanyInfo.Remove(companyInfo);
                    await _unitOfWork.Save();

                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Company profile deleted successfully.";
                    return response;
                }

                // If not an admin and not the owner
                response.Success = false;
                response.StatusCode = HttpStatusCode.Forbidden;
                response.Message = "You do not have permission to delete this profile.";
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