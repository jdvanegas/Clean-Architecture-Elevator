using Elevator.Management.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState
{
    public class ElevatorDto
    {
        public Guid ElevatorId { get; set; }
        public string Name { get; set; }
        public ElevatorState Satate { get; set; }
        public int CurrentFloor { get; set; }
        public List<int> NextFloors { get; set; } = new List<int>();
    }
}
