using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShoppingCart.Data.UOW.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ShoppingContext _db;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;
        public IUnitOfWork Uof { get; set; }


        public GenericRepository(ShoppingContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
            _dbSet = _db.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            var t = await _dbSet.FindAsync(id);

            if (t != null)
            {
                _dbSet.Remove(t);
                return true;
            }
            else
                return false;
        }

        public bool Remove(T entity)
        {
            var t = _dbSet.Remove(entity);
            if (t != null)
            {
                return true;
            }
            else
                return false;
        }

        public Task<bool> UpsertAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
