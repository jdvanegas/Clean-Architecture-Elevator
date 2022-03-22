using Elevator.Management.Domain.Common;
using Elevator.Management.Domain.Enums;
using System;

namespace Elevator.Management.Domain.Entities
{
    public class Elevator : AuditableEntity
    {
        public Guid ElevatorId { get; set; } = Guid.NewGuid();
        public Guid BuildingId { get; set; }
        public string Name { get; set; }
        public ElevatorState State { get; set; }
        public int CurrentFloor { get; set; }
    }
}
