using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class ProductDefinationRepository : Repository<ProductDefination>, IProductDefinationRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public ProductDefinationRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(ProductDefination productD)
        {
            _dbContext.Update(productD);
        }
    }
}