using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LandingPage.Models;

namespace LandingPage.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(GenericRequest<T> request);
        Task<T> GetAsync(GenericRequest<T> request);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(List<T> entities);
    };
}