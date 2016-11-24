using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Randomizer.Web.Data.Repositories
{
    /// <summary>
    /// A base repository whitch act as an abstract layer for accessing the DbSets.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        /// <summary>
        /// Returns a single entity found by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
        }

        /// <summary>
        /// Returns all entities of the DbSet.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> All()
        {
            return _dbContext.Set<T>();
        }

        /// <summary>
        /// Takes a single element from a DbSet.
        /// </summary>
        /// <param name="predicate">A specified condition</param>
        /// <returns></returns>
        public async Task<T> GetSingle(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Takes a single element from a DbSet.
        /// </summary>
        /// <param name="predicate">A specified condition</param>
        /// <param name="includeProperties">Related entities to include in the query</param>
        /// <returns></returns>
        public async Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return  await query.Where(predicate).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Filters a sequence based on a condition.
        /// </summary>
        /// <param name="predicate">A specified condition</param>
        /// <param name="includeProperties">Related entities to include in the query</param>
        /// <returns></returns>
        public IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate);
        }

        /// <summary>
        /// Filters a sequence based on a condition.
        /// </summary>
        /// <param name="predicate">A specified condition</param>
        /// <returns></returns>
        public IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
             .Where(predicate);
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
