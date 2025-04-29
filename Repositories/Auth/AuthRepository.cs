using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Models.Dto;
using LandingPage.Repositories.IRepositories.IAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LandingPage.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LandingPageDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _secretKey;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(LandingPageDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string secretKey, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _secretKey = secretKey;
            _env = env;
            _httpContextAccessor = httpContextAccessor;

        }
        public bool IsUniqueUser(string phoneNumber)
        {
            var user = _context.ApplicationUsers?.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<ApiResponse> Login(LoginRequestDto request)
        {
            var response = new ApiResponse();
            var loginRes = new LoginResponseDto();
            try
            {
                var user = _context.ApplicationUsers?.FirstOrDefault(u => u.UserName.ToLower() == request.UserName.ToLower());
                bool deactivatedUser = user?.Active == 0;
                bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (user == null || isValid == false || deactivatedUser == true)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Username or password is incorrect!";
                    return response;
                }

                //if user found generate jwt token 
                var roles = await _userManager.GetRolesAsync(user);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                  ]),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                loginRes.Id = user.Id;
                loginRes.Role = roles.FirstOrDefault();
                loginRes.Token = tokenHandler.WriteToken(token);

                // var jwt = tokenHandler.ReadJwtToken(loginRes.Token);
                // loginRes.Role = jwt.Claims.FirstOrDefault(x => x.Type == "role")?.Value;

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Login Successful";
                response.Results = loginRes;
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

        public async Task<ApiResponse> Registration(RegistrationReqDto request)
        {
            var response = new ApiResponse();

            // // Get the root path of wwwroot
            var rootPath = _env.WebRootPath;

            // // Generate unique names for the files
            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageUrl?.FileName);

            // // Combine root path with file names to create file paths
            var imagePath = Path.Combine(rootPath, "images", imageName);

            // // Ensure the "images" folder exists in wwwroot
            var imagesFolder = Path.Combine(rootPath, "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            // // Save the profile picture
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await request.ImageUrl.CopyToAsync(stream);
            }

            // // Create URLs for the saved files
            var imageUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/images/{imageName}";

            ApplicationUser user = new()
            {
                UserName = request.UserName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Address,
                ImageUrl = request.ImageUrl != null ? imageUrl : null,
                Active = int.Parse(request.Active),
                CreatedDate = DateTime.UtcNow,

            };
            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {

                    //var ff =_roleManager.FindByNameAsync("admin");
                    // if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    // {
                    //     await _roleManager.CreateAsync(new IdentityRole("admin"));
                    //     await _roleManager.CreateAsync(new IdentityRole("user"));
                    // }
                    await _userManager.AddToRoleAsync(user, "user");

                    response.Success = true;
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = "User created successfully.";
                    //return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{string.Join("\n", result.Errors.Select(s => s.Code))}\n{string.Join("\n", result.Errors.Select(s => s.Description))}";
                }
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

        public Task<ApiResponse> ResetPassword(ResetPassReqDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> UpdatePassword(UpdatePassReqDto request)
        {
            var response = new ApiResponse();
            try
            {

                var user = _context.ApplicationUsers?.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (user?.Result == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Something went wrong while updating password";
                    return response;
                }
                await _userManager.ChangePasswordAsync(user.Result, request.OldPassword, request.NewPassword);

                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Password update successful";
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