using System;
using System.Linq.Expressions;
using Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Implements
{
    public class RepositoryBase<T, K, TContext>
   : IRepositoryBase<T, K, TContext> where T : EntityBase<K>
                                          where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBase(
            TContext dbContext,
            IUnitOfWork<TContext> unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IQueryable<T> FindAll(
            bool trackChanges = false)
        {
            return !trackChanges ? _dbContext.Set<T>().AsNoTracking() : _dbContext.Set<T>();
        }



        public IQueryable<T> FindAll(
            bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAll(trackChanges);

            items = includeProperties.Aggregate(
                items,
                (
                    current,
                    includeProperty) => current.Include(includeProperty));

            return items;
        }

        public IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression,
            bool trackChanges = false)
        {
            return !trackChanges
                       ? _dbContext.Set<T>().Where(expression).AsNoTracking()
                       : _dbContext.Set<T>().Where(expression);
        }

        public IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression,
            bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindByCondition(expression, trackChanges);
            items = includeProperties.Aggregate(items,
                                                (
                                                    current,
                                                    includeProperty) => current.Include(includeProperty));
            return items;
        }

        public async Task<T?> GetByIdAsync(
            K id)
        {
            return await FindByCondition(x => x.Id.Equals(id))
                       .FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(
            K id,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByCondition(x => x.Id.Equals(id),
                                         false,
                                         includeProperties)
                       .FirstOrDefaultAsync();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _dbContext.Database.BeginTransactionAsync();
        }

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        public Task RollbackTransactionAsync()
        {
            return _dbContext.Database.RollbackTransactionAsync();
        }

        public void Create(T entity) => _dbContext.Set<T>().Add(entity);

        public async Task<K> CreateAsync(
            T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IList<K>> CreateListAsync(
            IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities.Select(x => x.Id).ToList();
        }

        public async Task<int> UpdateAsync(
            T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged)
                return 0;

            var exist = _dbContext.Set<T>()
                                  .Find(entity.Id);

            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return await SaveChangesAsync();

        }

        public async Task UpdateListAsync(
            IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await SaveChangesAsync();
        }

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
        public async Task<int> DeleteAsync(
            T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return await SaveChangesAsync();
        }

        public Task DeleteListAsync(
            IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync()
        {
            return _unitOfWork.CommitAsync();
        }
    }
}


