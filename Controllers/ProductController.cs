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
    [Route("api/v{version:apiVersion}/product")]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse response;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllProduct(string user, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllAsync(new GenericRequest<Product>
                {
                    Expression = user == "all" ? null : x => x.UserId == user,
                    NoTracking = true,
                    IncludeProperties = "User",
                    CancellationToken = cancellationToken
                });
                if (products == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = products;
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
        public async Task<ApiResponse> GetProduct(int Id, CancellationToken cancellationToken)
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
                var products = await _unitOfWork.Products.GetAsync(new GenericRequest<Product>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (products == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = products;
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
        public async Task<ApiResponse> CreateProduct(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllAsync(new GenericRequest<Product>
                {
                    Expression = null,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (products.Count > 4)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "You can add maximum 5 Products.";
                    return response;
                }
                var productImageUrl = "";

                if (request.ProductImageUrl != null)
                {

                    productImageUrl = await _unitOfWork.File.FileUpload(request.ProductImageUrl, "images");
                }
                Product productToCreate = new()
                {
                    Name = request.Name,
                    Color = request.Color,
                    Size = request.Size,
                    ProductDescription = request.ProductDescription,
                    Price = request.Price,
                    ProductImageUrl = productImageUrl,
                    UserId = request.UserId,
                };
                await _unitOfWork.Products.AddAsync(productToCreate);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Product creation failed.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.Created;
                response.Message = "Product created successfully.";
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
        public async Task<ApiResponse> UpdateProduct(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var genericReq = new GenericRequest<Product>
                {
                    Expression = x => x.Id == request.Id,
                    IncludeProperties = null,
                    NoTracking = true,
                    CancellationToken = cancellationToken
                };
                var productToUpdate = await _unitOfWork.Products.GetAsync(genericReq);
                if (productToUpdate == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"Data not found";
                    return response;
                }
                if (request.ProductImageUrl != null)
                {
                    if (!string.IsNullOrEmpty(productToUpdate.ProductImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(productToUpdate.ProductImageUrl);
                    }
                }

                var productImageUrl = "";

                if (request.ProductImageUrl != null)
                {

                    productImageUrl = await _unitOfWork.File.FileUpload(request.ProductImageUrl, "images");
                }

                productToUpdate.Name = (request.Name == null || request.Name == "") ? productToUpdate.Name : request.Name;
                productToUpdate.Color = (request.Color == null || request.Color == "") ? productToUpdate.Color : request.Color;
                productToUpdate.Size = (request.Size == null || request.Size == "") ? productToUpdate.Size : request.Size;
                productToUpdate.ProductDescription = (request.ProductDescription == null || request.ProductDescription == "") ? productToUpdate.ProductDescription : request.ProductDescription;
                productToUpdate.Price = request.Price == 0 ? productToUpdate.Price : request.Price;
                productToUpdate.ProductImageUrl = request.ProductImageUrl == null ? productToUpdate.ProductImageUrl : productImageUrl;

                _unitOfWork.Products.Update(productToUpdate);
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
                response.Message = "Product updated successfully.";
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
        public async Task<ApiResponse> DeleteProduct(int Id, CancellationToken cancellationToken)
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

                var product = await _unitOfWork.Products.GetAsync(new GenericRequest<Product>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });

                if (product == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Product not found.";
                    return response;
                }

                // Check if the user is an admin or the owner of the product
                if (userRole == "admin" || (userRole == "user" && product.UserId == userId))
                {
                    // Delete the image file if it exists
                    if (!string.IsNullOrEmpty(product.ProductImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(product.ProductImageUrl);
                    }

                    _unitOfWork.Products.Remove(product);
                    await _unitOfWork.Save();

                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Product deleted successfully.";
                    return response;
                }

                // If not an admin and not the owner
                response.Success = false;
                response.StatusCode = HttpStatusCode.Forbidden;
                response.Message = "You do not have permission to delete this product.";
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