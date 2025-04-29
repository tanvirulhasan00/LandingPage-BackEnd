using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models;

namespace LandingPage.Repositories.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}