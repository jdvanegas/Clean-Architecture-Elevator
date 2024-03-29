﻿using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState
{
    public class GetElevatorStateHandler : IRequestHandler<GetElevatorStateQuery, ElevatorDto>
    {
        private readonly IAsyncRepository<Domain.Entities.Elevator> _elevatorRepository;
        private readonly ILogger<GetElevatorStateHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Domain.Entities.Movement> _movementRepository;

        public GetElevatorStateHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Elevator> elevatorRepository,
            IAsyncRepository<Domain.Entities.Movement> movementRepository, ILogger<GetElevatorStateHandler> logger)
        {
            _mapper = mapper;
            _elevatorRepository = elevatorRepository;
            _movementRepository = movementRepository;
            _logger = logger;
        }

        public async Task<ElevatorDto> Handle(GetElevatorStateQuery request, CancellationToken cancellationToken)
        {
            var elevator = await _elevatorRepository.GetByIdAsync(request.Id) ??
                           throw new NotFoundException(nameof(Domain.Entities.Elevator), request.Id);

            var elevatorDto = _mapper.Map<ElevatorDto>(elevator);

            if (elevator.State == Domain.Enums.ElevatorState.Still) return elevatorDto;

            var movements = _movementRepository.GetByQuery(movement => movement.ElevatorId == elevator.ElevatorId);

            var upMovements = movements.Where(movement => movement.DestinationFloor > elevator.CurrentFloor);
            var downMovements = movements.Where(m => m.DestinationFloor < elevator.CurrentFloor);

            if (elevatorDto.Satate == Domain.Enums.ElevatorState.GoingUp)
            {
                elevatorDto.NextFloors.AddRange(upMovements.Select(movement => movement.DestinationFloor).OrderBy(m => m));
                elevatorDto.NextFloors.AddRange(downMovements.Select(movement => movement.DestinationFloor).OrderByDescending(m => m));
            }
            else
            {
                elevatorDto.NextFloors.AddRange(downMovements.Select(movement => movement.DestinationFloor).OrderByDescending(m => m));
                elevatorDto.NextFloors.AddRange(upMovements.Select(movement => movement.DestinationFloor).OrderBy(m => m));
            }

            _logger.LogInformation($"Successful Elevator get by Id {elevator.ElevatorId}");

            return elevatorDto;
        }
    }
}