using MediatR;
using System;

namespace Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState
{
    public class GetElevatorStateQuery: IRequest<ElevatorDto>
    {
        public Guid Id { get; set; }
    }
}
