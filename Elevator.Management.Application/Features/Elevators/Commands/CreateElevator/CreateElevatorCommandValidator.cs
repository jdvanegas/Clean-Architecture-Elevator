using FluentValidation;
using Elevator.Management.Application.Contracts.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Elevator.Management.Domain.Entities;

namespace Elevator.Management.Application.Features.Elevators.Commands.CreateElevator
{
    public class CreateElevatorCommandValidator : AbstractValidator<CreateElevatorCommand>
    {
        private readonly IElevatorRepository _elevatorRepository;
        private readonly IAsyncRepository<Building> _buildingRepository;
        public CreateElevatorCommandValidator(IElevatorRepository elevatorRepository, 
            IAsyncRepository<Building> buildingRepository)
        {
            _elevatorRepository = elevatorRepository;
            _buildingRepository = buildingRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(30).WithMessage("{PropertyName} must not exceed 30 characters.");

            RuleFor(p => p.CurrentFloor)
            .NotEqual(0)
            .WithMessage("Current floor can't be '0'");

            RuleFor(e => e)
                .MustAsync(FloorValid)
                .WithMessage("An elevator with the same name already exists.");

            RuleFor(e => e)
                .MustAsync(ElevatorNameUnique)
                .WithMessage("An elevator with the same name already exists.");

        }

        private async Task<bool> ElevatorNameUnique(CreateElevatorCommand e, CancellationToken token)
        {
            return !(await _elevatorRepository.IsElevatorNameUnique(e.Name, e.BuildingId));
        }

        private async Task<bool> FloorValid(CreateElevatorCommand e, CancellationToken token)
        {
            var buildign = await _buildingRepository.GetByIdAsync(e.BuildingId);
            return e.CurrentFloor >= buildign.FirstFloor && e.CurrentFloor <= buildign.LastFloor;
        }
    }
}
