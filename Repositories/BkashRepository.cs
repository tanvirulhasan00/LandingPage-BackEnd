using LandingPage.Models.bkash.createpayment;
using LandingPage.Models.bkash.executepayment;
using LandingPage.Models.bkash.granttoken;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace LandingPage.Repositories
{
    public class BkashRepository : IBkashRepository
    {
        public readonly string app_key = "4f6o0cjiki2rfm34kfdadl1eqq";
        public readonly string app_secret = "2is7hdktrekvrbljjh44ll3d9l1dtjo4pasmjvs5vl5qr3fug4b";
        public async Task<GrantTokenResponse> GrantTokenAsync()
        {
            var response = new GrantTokenResponse();
            try
            {
                GrantTokenRequest granttokenReq = new()
                {
                    username = "sandboxTokenizedUser02",
                    password = "sandboxTokenizedUser02@12345"
                };
                var req = new
                {
                    app_key = app_key,
                    app_secret = app_secret
                };

                var options = new RestClientOptions("https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/token/grant");
                var client = new RestClient(options);
                var request = new RestRequest("");
                var jsonObj = JsonConvert.SerializeObject(req);

                request.AddHeader("content-type", "application/json");
                request.AddHeader("accept", "application/json");
                request.AddHeader("username", granttokenReq.username);
                request.AddHeader("password", granttokenReq.password);
                request.AddJsonBody(jsonObj, false);
                var res = await client.PostAsync(request);
                response = JsonConvert.DeserializeObject<GrantTokenResponse>(res.Content.ToString());

                Console.WriteLine("response", response);

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine("", ex);
                response.statusMessage = ex.Message;
                return response;
            }
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequestDto req)
        {
            var response = new CreatePaymentResponse();
            try
            {
                var createReq = new
                {
                    mode = "0011",
                    payerReference = "0172388",
                    callbackURL = "http://localhost:5173/payment-success",
                    merchantAssociationInfo = "MI123",
                    amount = "1",
                    currency = "BDT",
                    intent = "sale",
                    merchantInvoiceNumber = "INV123456",

                };
                var jsonObj = JsonConvert.SerializeObject(createReq);
                var options = new RestClientOptions("https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/create");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("authorization", req.Token);
                request.AddHeader("x-app-key", app_key);
                request.AddJsonBody(jsonObj, false);
                var res = await client.PostAsync(request);
                response = JsonConvert.DeserializeObject<CreatePaymentResponse>(res.Content.ToString());


                // Console.WriteLine("Create", res.Content);
                return response;
            }
            catch (Exception ex)
            {
                // response.statusMessage = ex.Message;
                return response;
            }
        }

        public async Task<ExecutePaymentResponse> ExecutePaymentAsync(ExecutePaymentRequest req)
        {
            var response = new ExecutePaymentResponse();
            try
            {
                var options = new RestClientOptions($"https://checkout.sandbox.bka.sh/v1.2.0-beta/checkout/payment/execute/{req.PaymentID}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", req.Token);
                request.AddHeader("X-APP-Key", app_key);
                var res = await client.PostAsync(request);
                response = JsonConvert.DeserializeObject<ExecutePaymentResponse>(res.Content.ToString());

                Console.WriteLine("response", response);

                return response;
            }
            catch (Exception ex)
            {
                response.errorMessage = ex.Message;
                return response;
            }
        }
    }
}