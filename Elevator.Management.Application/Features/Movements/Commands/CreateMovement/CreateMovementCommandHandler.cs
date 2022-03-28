using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Exceptions;
using Elevator.Management.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Features.Movements.Commands.CreateMovement
{
    public class CreateMovementCommandHandler : IRequestHandler<CreateMovementCommand, Guid>
    {
        private readonly IAsyncRepository<Building> _buildingRepository;
        private readonly IElevatorRepository _elevatorRepository;
        private readonly ILogger<CreateMovementCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Domain.Entities.Movement> _movementRepository;

        public CreateMovementCommandHandler(IMapper mapper, IElevatorRepository elevatorRepository,
            IAsyncRepository<Building> buildingRepository, IAsyncRepository<Domain.Entities.Movement> movementRepository,
            ILogger<CreateMovementCommandHandler> logger)
        {
            _elevatorRepository = elevatorRepository;
            _buildingRepository = buildingRepository;
            _movementRepository = movementRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
        {
            var elevator = await _elevatorRepository.GetByIdAsync(request.ElevatorId) ??
                           throw new NotFoundException(nameof(Domain.Entities.Elevator), request.ElevatorId);

            var movement = _mapper.Map<Domain.Entities.Movement>(request);
            movement = await _movementRepository.AddAsync(movement);

            var movements = _movementRepository.GetByQuery(m => m.ElevatorId == elevator.ElevatorId);

            if (movements.Count == 0 && elevator.State == Domain.Enums.ElevatorState.Still)
                elevator.State = movement.DestinationFloor > elevator.CurrentFloor ?
                    Domain.Enums.ElevatorState.GoingUp : Domain.Enums.ElevatorState.GoingDown;

            await _elevatorRepository.UpdateAsync(elevator);

            _logger.LogInformation($"Movement created successful with id {movement.MovementId}");

            return movement.MovementId;
        }
    }
}