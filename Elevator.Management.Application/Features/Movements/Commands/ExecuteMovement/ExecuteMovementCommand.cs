using MediatR;
using System;

namespace Elevator.Management.Application.Features.Movement.Commands.ExecuteMovement
{
    public class ExecuteMovementCommand: IRequest
    {
        public Guid ElevatorId { get; set; }
    }
}
