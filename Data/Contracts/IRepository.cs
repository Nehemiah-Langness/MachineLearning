using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base;

namespace Data.Contracts
{
    public interface IRepository<T> where T : class, IEntity
    {
        IRepository<T> Include(Func<IQueryable<T>, IQueryable<T>> includes);
        IRepository<T> ClearIncludes();

        T Get(int id);
        IList<T> Find(Expression<Func<T, bool>> predicate = null);
        T Add(T entity);
        IList<T> AddRange(IEnumerable<T> entities);
        void Remove(int id);
        T Update(int id, object values);
    }
}