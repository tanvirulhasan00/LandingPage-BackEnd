using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public ProductRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Product product)
        {
            _dbContext.Update(product);
        }
    }
}