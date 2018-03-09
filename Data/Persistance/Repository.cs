using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Data.Contracts;
using Data.Services;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private Func<IQueryable<T>, IQueryable<T>> _includes;
        private readonly Mapper<T> _mapper = new Mapper<T>();

        protected readonly DbContext Context;
        protected readonly IUnitOfWork UnitOfWork;

        private IQueryable<T> Entities => _includes?.Invoke(Context.Set<T>()) ?? Context.Set<T>();

        public Repository(IUnitOfWork unitOfWork, DbContext context)
        {
            UnitOfWork = unitOfWork;
            Context = context;
        }

        public T Add(T entity)
        {
            return Context.Set<T>().Add(entity).Entity;
        }

        public IList<T> AddRange(IEnumerable<T> entities)
        {
            var entityList = entities.ToList();
            Context.Set<T>().AddRange(entityList);
            return entityList;
        }

        public IList<T> Find(Expression<Func<T, bool>> predicate = null)
        {
            var result = Entities;
            if (predicate != null)
                result = result.Where(predicate);

            return result.ToList();
        }

        public IRepository<T> ClearIncludes()
        {
            _includes = null;
            return this;
        }

        public T Get(int id)
        {
            return Entities.FirstOrDefault(t => t.Id == id);
        }

        public IRepository<T> Include(Func<IQueryable<T>, IQueryable<T>> includes)
        {
            _includes = (query) => includes(_includes(query));
            return this;
        }

        public void Remove(int id)
        {
            var entity = Context.Set<T>().Find(id);
            Context.Set<T>().Remove(entity);
        }

        public T Update(int id, object values)
        {
            return Context.Set<T>().Update(_mapper.Map(values, Get(id))).Entity;
        }
    }
}