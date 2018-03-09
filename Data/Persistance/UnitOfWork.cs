using System;
using Data.Contracts;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly Func<DbContext> _createContext;
        private DbContext _context;
        protected DbContext Context => _context ?? (_context = _createContext());
        

        public UnitOfWork(Func<DbContext> context)
        {
            _createContext = context;
        }

        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            return new Repository<T>(this, Context);
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}