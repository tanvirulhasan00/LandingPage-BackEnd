using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models;

namespace LandingPage.Repositories.IRepositories
{
    public interface IAllCostRepository : IRepository<AllCost>
    {
        void Update(AllCost allCost);
    }
}