using Elevator.Management.Domain.Common;
using System;

namespace Elevator.Management.Domain.Entities
{
    public class Building : AuditableEntity
    {
        public Guid BuildingId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int FirstFloor { get; set; }
        public int LastFloor { get; set; }
    }
}
