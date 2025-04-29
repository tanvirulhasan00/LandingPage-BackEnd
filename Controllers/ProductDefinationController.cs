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
    [Route("api/v{version:apiVersion}/product-defination")]
    [ApiVersion("1.0")]
    public class ProductDefinationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse response;
        public ProductDefinationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllProductDefination(CancellationToken cancellationToken)
        {
            try
            {
                var productD = await _unitOfWork.ProductDefination.GetAllAsync(new GenericRequest<ProductDefination>
                {
                    Expression = null,
                    NoTracking = true,
                    IncludeProperties = "Product,ProductSize",
                    CancellationToken = cancellationToken
                });
                if (productD == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = productD;
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
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> GetProductDefination(int Id, CancellationToken cancellationToken)
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
                var productD = await _unitOfWork.ProductDefination.GetAsync(new GenericRequest<ProductDefination>
                {
                    Expression = x => x.ProductId == Id,
                    NoTracking = true,
                    IncludeProperties = "Product,ProductSize",
                    CancellationToken = cancellationToken
                });
                if (productD == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = productD;
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
        public async Task<ApiResponse> CreateProductDefination(ProductDefinationCreateRequest request)
        {
            try
            {
                ProductDefination producDtToCreate = new()
                {
                    ProductId = int.Parse(request.ProductId),
                    ProductSizeId = int.Parse(request.ProductSizeId),
                };
                await _unitOfWork.ProductDefination.AddAsync(producDtToCreate);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Product defination creation failed.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Product defination created successfully.";
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
        public async Task<ApiResponse> DeleteProductDefination(int Id, CancellationToken cancellationToken)
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
                var productD = await _unitOfWork.ProductDefination.GetAsync(new GenericRequest<ProductDefination>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (productD == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                _unitOfWork.ProductDefination.Remove(productD);
                await _unitOfWork.Save();

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Product defination deleted successfully.";
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