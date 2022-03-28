using System;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Contracts.Persistence
{
    public interface IElevatorRepository : IAsyncRepository<Domain.Entities.Elevator>
    {
        Task<bool> IsElevatorNameUnique(string name, Guid buildingId);
    }
}