using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Features.Movements.Commands.CreateMovement;
using Elevator.Management.Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Validation.Movement
{
    public class CreateMovementValidator : AbstractValidator<CreateMovementCommand>
    {
        private readonly IAsyncRepository<Building> _buildingRepository;
        private readonly IElevatorRepository _elevatorRepository;

        public CreateMovementValidator(IElevatorRepository elevatorRepository, IAsyncRepository<Building> buildingRepository)
        {
            _elevatorRepository = elevatorRepository;
            _buildingRepository = buildingRepository;

            RuleFor(e => e.DestinationFloor)
                .NotEqual(0)
                .WithMessage("Destination floor cannot be '0'");

            RuleFor(e => e)
                .MustAsync(FloorValid)
                .WithMessage("The floor requested is not valid");

            RuleFor(e => e)
                .MustAsync(ElevatorExists)
                .WithMessage("An elevator with that id not exist.");
        }

        private async Task<bool> ElevatorExists(CreateMovementCommand m, CancellationToken token)
        {
            return (await _elevatorRepository.GetByIdAsync(m.ElevatorId)) != null;
        }

        private async Task<bool> FloorValid(CreateMovementCommand m, CancellationToken token)
        {
            var elevator = await _elevatorRepository.GetByIdAsync(m.ElevatorId);
            var building = await _buildingRepository.GetByIdAsync(elevator.BuildingId);
            return m.DestinationFloor >= building.FirstFloor && m.DestinationFloor <= building.LastFloor;
        }
    }
}