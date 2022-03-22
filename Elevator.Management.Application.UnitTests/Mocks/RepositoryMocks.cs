using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Management.Application.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IAsyncRepository<Building>> GetBuildingRepository()
        {
            var buildingId = Guid.Parse("038bbcc5-d66e-4d12-948f-3757d54b6115");
            var buildings = new List<Building>
            {
                new Building
                {
                    BuildingId = buildingId,
                    Name = "Masiv",
                    FirstFloor = -2,
                    LastFloor = 14,
                }
            };

            var mockBuildingRepository = new Mock<IAsyncRepository<Building>>();
            mockBuildingRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(buildings.FirstOrDefault());
            return mockBuildingRepository;
        }

        public static Mock<IElevatorRepository> GetElevatorRepository()
        {
            var buildingId = Guid.Parse("038bbcc5-d66e-4d12-948f-3757d54b6115");
            var elevators = new List<Domain.Entities.Elevator>
            {
                new Domain.Entities.Elevator
                {
                    ElevatorId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}"),
                    CurrentFloor = 6,
                    State = Domain.Enums.ElevatorState.GoingUp,
                    BuildingId = buildingId,
                    Name = "A"
                },
                new Domain.Entities.Elevator
                {
                    ElevatorId = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}"),
                    CurrentFloor = 1,
                    State = Domain.Enums.ElevatorState.Still,
                    BuildingId = buildingId,
                    Name = "B"
                },
                new Domain.Entities.Elevator
                {
                    ElevatorId = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}"),
                    CurrentFloor = 10,
                    State = Domain.Enums.ElevatorState.GoingDown,
                    BuildingId = buildingId,
                    Name = "C"
                }
            };

            var mockElevatorRepository = new Mock<IElevatorRepository>();
            mockElevatorRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(elevators.FirstOrDefault());


            mockElevatorRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Elevator>())).ReturnsAsync(
                (Domain.Entities.Elevator elevator) =>
                {
                    elevators.Add(elevator);
                    return elevator;
                });

            return mockElevatorRepository;
        }
        public static Mock<IAsyncRepository<Movement>> GetMovementRepository()
        {
            var elevatorAId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var elevatorBId = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var movements = new List<Movement>
            {
                 new Movement
                {
                    ElevatorId = elevatorAId,
                    DestinationFloor = 3
                },
                new Movement
                {
                    ElevatorId = elevatorAId,
                    DestinationFloor = 2
                },
                new Movement
                {
                    ElevatorId = elevatorAId,
                    DestinationFloor = 4
                },
                new Domain.Entities.Movement
                {
                    ElevatorId = elevatorBId,
                    DestinationFloor = 5
                },
                new Movement
                {
                    ElevatorId = elevatorBId,
                    DestinationFloor = 8
                }
            };

            var mockMovementRepository = new Mock<IAsyncRepository<Movement>>();
            mockMovementRepository.Setup(repo => repo.GetByQuery(It.IsAny<Func<Movement, bool>>())).Returns(movements);

            mockMovementRepository.Setup(repo => repo.AddAsync(It.IsAny<Movement>())).ReturnsAsync(
                (Movement movement) =>
                {
                    movements.Add(movement);
                    return movement;
                });


            mockMovementRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Movement>()));

            return mockMovementRepository;
        }
    }
}
