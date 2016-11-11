using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Randomizer.Web.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<T> All()
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> All(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
              .Where(predicate)
              .AsNoTracking();
        }
    }
}
