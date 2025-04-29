using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models;

namespace LandingPage.Repositories.IRepositories
{
    public interface IProductDefinationRepository : IRepository<ProductDefination>
    {
        void Update(ProductDefination productD);
    }
}