using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Exceptions;
using Elevator.Management.Domain.Entities;
using Elevator.Management.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Features.Movement.Commands.ExecuteMovement
{
    public class ExecuteMovementCommandHandler : IRequestHandler<ExecuteMovementCommand>
    {
        private readonly IAsyncRepository<Domain.Entities.Elevator> _elevatorRepository;
        private readonly IAsyncRepository<Domain.Entities.Movement> _movementRepository;
        private readonly ILogger<ExecuteMovementCommandHandler> _logger;
        private readonly IMapper _mapper;

        public ExecuteMovementCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Elevator> elevatorRepository,
            IAsyncRepository<Domain.Entities.Movement> movementRepository, ILogger<ExecuteMovementCommandHandler> logger)
        {
            _mapper = mapper;
            _elevatorRepository = elevatorRepository;
            _movementRepository = movementRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(ExecuteMovementCommand request, CancellationToken cancellationToken)
        {
            var elevator = await _elevatorRepository.GetByIdAsync(request.ElevatorId);

            if (elevator == null)
                throw new NotFoundException(nameof(Domain.Entities.Elevator), request.ElevatorId);

            var movements = _movementRepository.GetByQuery(m => m.ElevatorId == elevator.ElevatorId);
            if (movements.Count() == 0) throw new BadRequestException("There are not movements for this elevator");

            var upMovements = movements.Where(m => m.DestinationFloor > elevator.CurrentFloor);
            var downMovements = movements.Where(m => m.DestinationFloor < elevator.CurrentFloor);

            var movementToExecute = elevator.State == ElevatorState.GoingUp ?
                upMovements.OrderBy(m => m.DestinationFloor).FirstOrDefault() :
                downMovements.OrderByDescending(m => m.DestinationFloor).FirstOrDefault();

            elevator.CurrentFloor = movementToExecute.DestinationFloor;
            
            await _movementRepository.DeleteAsync(movementToExecute);

            if(elevator.State == ElevatorState.GoingUp)
            {
                if (upMovements.Count() == 1 && downMovements.Count() > 0)
                    elevator.State = ElevatorState.GoingDown;
                if(upMovements.Count() == 1 && downMovements.Count() == 0)
                    elevator.State = ElevatorState.Still;
            }
            else
            {
                if (downMovements.Count() == 1 && upMovements.Count() > 0)
                    elevator.State = ElevatorState.GoingUp;
                if (downMovements.Count() == 1 && upMovements.Count() == 0)
                    elevator.State = ElevatorState.Still;
            }

            await _elevatorRepository.UpdateAsync(elevator);

            Thread.Sleep(1000);
            _logger.LogInformation($"Elevator movement executed successful");

            return Unit.Value;
        }
    }
}
