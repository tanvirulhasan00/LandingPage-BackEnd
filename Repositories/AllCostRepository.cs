using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;

namespace LandingPage.Repositories
{
    public class AllCostRepository : Repository<AllCost>, IAllCostRepository
    {
        private readonly LandingPageDbContext _dbContext;
        public AllCostRepository(LandingPageDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(AllCost allCost)
        {
            _dbContext.Update(allCost);
        }
    }
}