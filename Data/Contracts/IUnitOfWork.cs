﻿using System;
using Domain.Base;

namespace Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class, IEntity;
        int Complete();
    }
}