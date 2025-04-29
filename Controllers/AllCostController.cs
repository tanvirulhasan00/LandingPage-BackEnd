using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LandingPage.Models;
using LandingPage.Models.dto;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/all-cost")]
    [ApiVersion("1.0")]
    public class AllCostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse response;
        public AllCostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]

        public async Task<ApiResponse> GetAllCost(CancellationToken cancellationToken)
        {
            try
            {
                var allCosts = await _unitOfWork.AllCost.GetAllAsync(new GenericRequest<AllCost>
                {
                    Expression = null,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (allCosts == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = allCosts;
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

        public async Task<ApiResponse> GetCost(int Id, CancellationToken cancellationToken)
        {
            try
            {
                if (Id < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid id required";
                    return response;
                }
                var allCosts = await _unitOfWork.AllCost.GetAsync(new GenericRequest<AllCost>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (allCosts == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = allCosts;
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
        public async Task<ApiResponse> CreateAllCost(AllCostCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {

                AllCost allCostToCreate = new()
                {
                    InsideDhaka = request.InsideDhaka,
                    OutsideDhaka = request.OutsideDhaka,
                    PickUp = request.PickUp,
                };
                await _unitOfWork.AllCost.AddAsync(allCostToCreate);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Shipping fees creation failed.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Shipping fees created successfully.";
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
        public async Task<ApiResponse> UpdateAllCost(AllCosUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var genericReq = new GenericRequest<AllCost>
                {
                    Expression = x => x.Id == request.Id,
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var allCostToUpdate = await _unitOfWork.AllCost.GetAsync(genericReq);
                if (allCostToUpdate == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"Data not found";
                    return response;
                }

                allCostToUpdate.InsideDhaka = request.InsideDhaka <= 0 ? allCostToUpdate.InsideDhaka : request.InsideDhaka;
                allCostToUpdate.OutsideDhaka = request.OutsideDhaka <= 0 ? allCostToUpdate.OutsideDhaka : request.OutsideDhaka;
                allCostToUpdate.PickUp = request.PickUp <= 0 ? allCostToUpdate.PickUp : request.PickUp;

                _unitOfWork.AllCost.Update(allCostToUpdate);
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
                response.Message = "Shipping fees updated successfully.";
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
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> DeleteProduct(int Id, CancellationToken cancellationToken)
        {
            try
            {
                if (Id < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid id required";
                    return response;
                }
                var allCost = await _unitOfWork.AllCost.GetAsync(new GenericRequest<AllCost>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (allCost == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                _unitOfWork.AllCost.Remove(allCost);
                await _unitOfWork.Save();

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Shipping fees deleted successfully.";
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