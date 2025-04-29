using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly LandingPageDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public UserRepository(LandingPageDbContext dbContext, IWebHostEnvironment env) : base(dbContext)
        {
            _dbContext = dbContext;
            _env = env;
        }
        public void Update(ApplicationUser user)
        {
            _dbContext.Update(user);
        }
    }
}