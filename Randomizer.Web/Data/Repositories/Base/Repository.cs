using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Randomizer.Web.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicationDbContext Context {
            get
            {
                return this._dbContext;
            }
        }

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
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

        public async Task<T> GetSingle(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return  await query.AsNoTracking().Where(predicate).SingleOrDefaultAsync();
        }

        public IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).AsNoTracking();
        }

        public IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
             .Where(predicate)
             .AsNoTracking();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
