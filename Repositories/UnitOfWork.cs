using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.Auth;
using LandingPage.Repositories.IRepositories;
using LandingPage.Repositories.IRepositories.IAuth;
using Microsoft.AspNetCore.Identity;

namespace LandingPage.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LandingPageDbContext _dbContext;
        public IProductRepository Products { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IBkashRepository Bkash { get; private set; }
        public IAuthRepository Auth { get; private set; }
        public IUserRepository User { get; private set; }
        public IFileRepository File { get; }
        public IProductDefinationRepository ProductDefination { get; private set; }
        public IAllCostRepository AllCost { get; private set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string SecretKey;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(LandingPageDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            SecretKey = configuration["TokenSetting:SecretKey"] ?? "";
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            File = new FileRepository(_env, _httpContextAccessor);
            Products = new ProductRepository(_dbContext);
            AllCost = new AllCostRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Orders = new OrderRepository(_dbContext);
            Payments = new PaymentRepository(_dbContext);
            Bkash = new BkashRepository();
            Auth = new AuthRepository(_dbContext, _userManager, _roleManager, SecretKey, _env, _httpContextAccessor);
            User = new UserRepository(_dbContext, _env);
            ProductDefination = new ProductDefinationRepository(_dbContext);
        }
        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}