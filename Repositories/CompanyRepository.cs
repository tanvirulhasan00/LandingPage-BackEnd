using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class CompanyRepository : Repository<CompanyInfo>, ICompanyRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public CompanyRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(CompanyInfo companyInfo)
        {
            _dbContext.Update(companyInfo);
        }
    }
}