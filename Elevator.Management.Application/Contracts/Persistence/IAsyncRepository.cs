using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task<T> GetByIdAsync(Guid id);

        IReadOnlyList<T> GetByQuery(Func<T, bool> query);

        Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task UpdateAsync(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> query);
    }
}