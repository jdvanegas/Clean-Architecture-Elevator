using Elevator.Management.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elevator.Management.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ElevatorDbContext DbContext;

        public BaseRepository(ElevatorDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> query) =>
            await DbContext.Set<T>().AnyAsync(query);

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public IReadOnlyList<T> GetByQuery(Func<T, bool> query)
        {
            return DbContext.Set<T>().Where(query).ToList();
        }

        public virtual async Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size)
        {
            return await DbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
    }
}