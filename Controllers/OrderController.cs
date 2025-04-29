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
    [Route("api/v{version:apiVersion}/order")]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse response;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.response = new ApiResponse();
        }

        [HttpGet]
        [Route("getall")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> GetAllOrder(string user, CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync(new GenericRequest<Order>
                {
                    Expression = user == "all" ? null : x => x.Product.UserId == user,
                    NoTracking = true,
                    IncludeProperties = "Product",
                    CancellationToken = cancellationToken
                });
                if (orders == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = orders;
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
        public async Task<ApiResponse> GetOrder(int Id, CancellationToken cancellationToken)
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
                var orders = await _unitOfWork.Orders.GetAsync(new GenericRequest<Order>
                {
                    Expression = x => x.Id == Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });
                if (orders == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Results = orders;
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
        public async Task<ApiResponse> CreateOrder(OrderCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid data required.";
                    return response;
                }
                var orderNumber = GenerateOrderNumber();

                var orders = new Order
                {
                    OrderNumber = orderNumber,
                    ProductId = int.Parse(request.ProductId),
                    ProductSize = request.ProductSize,
                    ProductColor = request.ProductColor,
                    DeliveryMethod = request.DeliveryMethod,
                    DeliveryStatus = "In Process",
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = "In Process",
                    TotalPrice = int.Parse(request.TotalPrice),
                    Quantity = request.Quantity,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    District = request.District,
                    SubDistrict = request.SubDistrict,
                    Comment = request.Comment,
                };
                var orderRes = new OrderResponseDto()
                {
                    DeliveryMethod = request.DeliveryMethod,
                    DeliveryStatus = request.DeliveryStatus,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = request.PaymentStatus,
                    TotalPrice = request.TotalPrice,
                    OrderNumber = orderNumber
                };

                await _unitOfWork.Orders.AddAsync(orders);
                int res = await _unitOfWork.Save();
                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Order Successful";
                response.Results = orderRes;
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
        private static string GenerateOrderNumber()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        [HttpPost]
        [Route("update-payment-status")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> UpdatePaymentStatus(OrderUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Status == null || request.Status == "" || request.Id <= 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid data required.";
                    return response;
                }
                var orders = await _unitOfWork.Orders.GetAsync(new GenericRequest<Order>
                {
                    Expression = x => x.Id == request.Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });

                orders.PaymentStatus = request.Status;

                _unitOfWork.Orders.Update(orders);
                int res = await _unitOfWork.Save();

                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Payment status updated.";
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
        [Route("update-delivery-status")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse> UpdateDeliveryStatus(OrderUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Status == null || request.Status == "" || request.Id <= 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Valid data required.";
                    return response;
                }
                var orders = await _unitOfWork.Orders.GetAsync(new GenericRequest<Order>
                {
                    Expression = x => x.Id == request.Id,
                    NoTracking = true,
                    IncludeProperties = null,
                    CancellationToken = cancellationToken
                });

                orders.DeliveryStatus = request.Status;

                _unitOfWork.Orders.Update(orders);
                int res = await _unitOfWork.Save();

                if (res < 0)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong";
                    return response;
                }

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Delivery status updated.";
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