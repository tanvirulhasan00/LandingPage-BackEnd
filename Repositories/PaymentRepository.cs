using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public PaymentRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Payment payment)
        {
            _dbContext.Update(payment);
        }
    }
}