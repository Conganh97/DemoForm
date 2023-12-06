using System;
using Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implements
{
    public class RepositoryQueryBase<T, K, TContext> : RepositoryBase<T, K, TContext>, IRepositoryQueryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        public RepositoryQueryBase(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base(dbContext, unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}

