using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Repositories.IRepositories.IAuth;

namespace LandingPage.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        public IProductRepository Products { get; }
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public IPaymentRepository Payments { get; }
        public IBkashRepository Bkash { get; }
        public IAuthRepository Auth { get; }
        public IUserRepository User { get; }
        public IFileRepository File { get; }
        public IProductDefinationRepository ProductDefination { get; }
        public IAllCostRepository AllCost { get; }
        public ICompanyRepository CompanyInfo { get; }

        Task<int> Save();
    }
}