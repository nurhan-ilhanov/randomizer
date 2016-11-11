using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Randomizer.Web.Data
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<T> GetById(int id);
        IQueryable<T> All();
        IQueryable<T> All(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
