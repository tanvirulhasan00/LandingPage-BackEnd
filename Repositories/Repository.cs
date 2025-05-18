using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LandingPage.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LandingPageDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(LandingPageDbContext dbContext)
        {
            _dbContext = dbContext;
            this._dbSet = _dbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync(GenericRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? _dbSet.AsNoTracking() : _dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var propery in request.IncludeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propery);
                }
                ;
            }
            return await query.ToListAsync(request.CancellationToken);
        }

        public async Task<T> GetAsync(GenericRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? _dbSet.AsNoTracking() : _dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var property in request.IncludeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
                ;
            }
            var result = await query.FirstOrDefaultAsync(request.CancellationToken);

            if (result == null)
            {
                // Log warning or handle the case here
                return null;
            }

            return result;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}