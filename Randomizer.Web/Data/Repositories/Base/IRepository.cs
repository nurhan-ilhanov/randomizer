using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Randomizer.Web.Data.Repositories
{
    public interface IRepository<T> : IDisposable where T: IBaseEntity, new()
    {
        Task<T> GetById(int id);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All();
        IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> AllWhere(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task Save();
    }
}
