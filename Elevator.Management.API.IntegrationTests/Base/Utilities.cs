using Elevator.Management.Domain.Entities;
using Elevator.Management.Persistence;
using System;

namespace Elevator.Management.API.IntegrationTests.Base
{
    public class Utilities
    {
        public static void InitializeDbForTests(ElevatorDbContext context)
        {
            context.Buildings.Add(new Building
            {
                BuildingId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"),
                Name = "Masiv",
                FirstFloor = -2,
                LastFloor = 14
            });

            context.Elevators.Add(new Domain.Entities.Elevator
            {
                ElevatorId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}"),
                BuildingId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"),
                Name = "A",
                State = Domain.Enums.ElevatorState.Still,
                CurrentFloor = 2
            });            

            context.SaveChanges();
        }
    }
}
