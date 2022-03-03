using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Exceptions;
using Elevator.Management.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Features.Movement.Commands.CreateMovement
{
    public class CreateMovementCommandHandler : IRequestHandler<CreateMovementCommand, Guid>
    {
        private readonly IElevatorRepository _elevatorRepository;
        private readonly IAsyncRepository<Building> _buildingRepository;
        private readonly IAsyncRepository<Domain.Entities.Movement> _movementRepository;
        private readonly ILogger<CreateMovementCommandHandler> _logger;
        private readonly IMapper _mapper;


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
            var validator = new CreateMovementCommandValidator(_elevatorRepository, _buildingRepository);
            var validationResult = await validator.ValidateAsync(request);
            
            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            var elevator = await _elevatorRepository.GetByIdAsync(request.ElevatorId);
            if(elevator == null) throw new NotFoundException(nameof(Domain.Entities.Elevator), request.ElevatorId);

            var movement = _mapper.Map<Domain.Entities.Movement>(request);
            movement = await _movementRepository.AddAsync(movement);

            var movements = _movementRepository.GetByQuery(m => m.ElevatorId == elevator.ElevatorId);

            if(movements.Count == 0 && elevator.State == Domain.Enums.ElevatorState.Still) 
                elevator.State = movement.DestinationFloor > elevator.CurrentFloor ? 
                    Domain.Enums.ElevatorState.GoingUp : Domain.Enums.ElevatorState.GoingDown;

            await _elevatorRepository.UpdateAsync(elevator);

            _logger.LogInformation($"Movement created successful with id {movement.MovementId}");
            
            return movement.MovementId;
        }
    }
}