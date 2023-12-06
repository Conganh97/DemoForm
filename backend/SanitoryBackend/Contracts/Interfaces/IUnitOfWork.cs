using System;
using Microsoft.EntityFrameworkCore;

namespace Common.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task<int> CommitAsync();
    }
}

