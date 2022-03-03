using Elevator.Management.Domain.Common;
using System;

namespace Elevator.Management.Domain.Entities
{
    public class Movement : AuditableEntity
    {
        public Guid MovementId { get; set; } = Guid.NewGuid();
        public Guid ElevatorId { get; set; }
        public int DestinationFloor { get; set; }
        public Elevator Elevator { get; set; }
    }
}
