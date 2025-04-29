using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public OrderRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Order order)
        {
            _dbContext.Update(order);
        }
    }
}