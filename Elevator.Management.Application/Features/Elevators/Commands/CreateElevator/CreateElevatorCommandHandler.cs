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
        private readonly IElevatorRepository _elevatorRepository;
        private readonly IAsyncRepository<Building> _buildingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateElevatorCommandHandler> _logger;


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
            var validator = new CreateElevatorCommandValidator(_elevatorRepository, _buildingRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            var elevator = _mapper.Map<Domain.Entities.Elevator>(request);

            elevator = await _elevatorRepository.AddAsync(elevator);

            _logger.LogInformation($"Elevator created with Id {elevator.ElevatorId}");
            
            return elevator.ElevatorId;
        }
    }
}