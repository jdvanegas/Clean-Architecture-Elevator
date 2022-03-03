using MediatR;
using System;

namespace Elevator.Management.Application.Features.Elevators.Commands.CreateElevator
{
    public class CreateElevatorCommand: IRequest<Guid>
    {
        public Guid BuildingId { get; set; }
        public string Name { get; set; }
        public int CurrentFloor { get; set; }
    }
}
