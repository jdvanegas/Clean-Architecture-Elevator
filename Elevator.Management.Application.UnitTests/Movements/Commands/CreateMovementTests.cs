using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Application.Features.Movements.Commands.CreateMovement;
using Elevator.Management.Application.Profiles;
using Elevator.Management.Application.UnitTests.Mocks;
using Elevator.Management.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Elevator.Management.Application.UnitTests.Movements.Commands
{
    public class CreateMovementTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IElevatorRepository> _mockElevatorRepository;
        private readonly Mock<IAsyncRepository<Domain.Entities.Movement>> _mockMovementRepository;
        private readonly Mock<IAsyncRepository<Building>> _mockBuildingRepository;
        private readonly ILogger<CreateMovementCommandHandler> _logger;

        public CreateMovementTests()
        {
            _mockMovementRepository = RepositoryMocks.GetMovementRepository();
            _mockElevatorRepository = RepositoryMocks.GetElevatorRepository();
            _mockBuildingRepository = RepositoryMocks.GetBuildingRepository();
            _logger = Mock.Of<ILogger<CreateMovementCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidMovement_AddedToMovementsRepo()
        {
            var handler = new CreateMovementCommandHandler(_mapper, _mockElevatorRepository.Object, 
                _mockBuildingRepository.Object,
                _mockMovementRepository.Object, _logger);

            await handler.Handle(new CreateMovementCommand() 
            { 
                ElevatorId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}"),
                DestinationFloor = 5 
            }, CancellationToken.None);

            var allMovements = _mockMovementRepository.Object.GetByQuery(m => m.ElevatorId == Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}"));
            allMovements.Count.ShouldBe(6);
        }
    }
}
