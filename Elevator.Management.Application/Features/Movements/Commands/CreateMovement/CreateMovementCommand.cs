using MediatR;
using System;

namespace Elevator.Management.Application.Features.Movement.Commands.CreateMovement
{
    public class CreateMovementCommand: IRequest<Guid>
    {
        public Guid ElevatorId { get; set; }
        public int DestinationFloor { get; set; }
    }
}
