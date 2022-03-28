using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Features.Elevators.Commands.CreateElevator
{
    public class CreateElevatorCommandHandler : IRequestHandler<CreateElevatorCommand, Guid>
    {
        private readonly IAsyncRepository<Building> _buildingRepository;
        private readonly IElevatorRepository _elevatorRepository;
        private readonly ILogger<CreateElevatorCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateElevatorCommandHandler(IMapper mapper, IElevatorRepository elevatorRepository,
            IAsyncRepository<Building> buildingRepository, ILogger<CreateElevatorCommandHandler> logger)
        {
            _mapper = mapper;
            _elevatorRepository = elevatorRepository;
            _buildingRepository = buildingRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateElevatorCommand request, CancellationToken cancellationToken)
        {
            var elevator = _mapper.Map<Domain.Entities.Elevator>(request);
            elevator = await _elevatorRepository.AddAsync(elevator);

            _logger.LogInformation($"Elevator created with Id {elevator.ElevatorId}");

            return elevator.ElevatorId;
        }
    }
}