using System;
using MediatR;

namespace Elevator.Management.Application.Features.Movements.Commands.CreateMovement
{
    public class CreateMovementCommand: IRequest<Guid>
    {
        public Guid ElevatorId { get; set; }
        public int DestinationFloor { get; set; }
    }
}
