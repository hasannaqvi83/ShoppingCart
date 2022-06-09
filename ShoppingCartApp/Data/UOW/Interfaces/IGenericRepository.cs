using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<bool> AddAsync(T entity);
        Task<bool> RemoveByIdAsync(int id);
        bool Remove(T entity);
        Task<bool> UpsertAsync(T entity);
        IUnitOfWork Uof { get; }
    }
}