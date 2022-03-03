using Elevator.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
        IReadOnlyList<T> GetByQuery(Func<T, bool> query);
    }
}
