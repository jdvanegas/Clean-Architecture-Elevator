using Elevator.Management.Application.Contracts.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elevator.Management.Persistence.Repositories
{
    public class ElevatorRepository : BaseRepository<Domain.Entities.Elevator>, IElevatorRepository
    {
        public ElevatorRepository(ElevatorDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> IsElevatorNameUnique(string name, Guid buildingId)
        {
            var matches =  _dbContext.Elevators.Any(e => e.Name.Equals(name) && e.BuildingId.Equals(buildingId));
            return Task.FromResult(matches);
        }
    }
}
