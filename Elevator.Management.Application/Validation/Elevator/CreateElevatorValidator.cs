using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Features.Elevators.Commands.CreateElevator;
using Elevator.Management.Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Validation.Elevator
{
    public class CreateElevatorValidator : AbstractValidator<CreateElevatorCommand>
    {
        private readonly IElevatorRepository _elevatorRepository;
        private readonly IAsyncRepository<Building> _buildingRepository;

        public CreateElevatorValidator(IElevatorRepository elevatorRepository,
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
               .MustAsync(ExistBuilding)
               .WithMessage("Building with Id {PropertyValue} don't exists.")
               .DependentRules(() =>
               {
                   RuleFor(e => e)
                    .MustAsync(FloorValid)
                    .WithMessage("An elevator with the same name already exists.");
               });

            RuleFor(e => e)
                .MustAsync(ElevatorNameUnique)
                .WithMessage("An elevator with the same name already exists.");
        }

        private async Task<bool> ElevatorNameUnique(CreateElevatorCommand e, CancellationToken token) =>
            !(await _elevatorRepository.IsElevatorNameUnique(e.Name, e.BuildingId));

        private async Task<bool> ExistBuilding(CreateElevatorCommand e, CancellationToken token) =>
             await _buildingRepository.AnyAsync(b => b.BuildingId == e.BuildingId);

        private async Task<bool> FloorValid(CreateElevatorCommand e, CancellationToken token)
        {
            var building = await _buildingRepository.GetByIdAsync(e.BuildingId);
            return e.CurrentFloor >= building.FirstFloor && e.CurrentFloor <= building.LastFloor;
        }
    }
}