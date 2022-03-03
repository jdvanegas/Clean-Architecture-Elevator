using Elevator.Management.Application.Contracts;
using Elevator.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using Xunit;

namespace Elevator.Management.Persistence.IntegrationTests
{
    public class ElevatorDbContextTests
    {
        private readonly ElevatorDbContext _elevatorDbContext;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _loggedInUserId;

        public ElevatorDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ElevatorDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _loggedInUserId = "00000000-0000-0000-0000-000000000000";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);

            _elevatorDbContext = new ElevatorDbContext(dbContextOptions, _loggedInUserServiceMock.Object);
        }

        [Fact]
        public async void Save_SetCreatedByProperty()
        {
            var ev = new Domain.Entities.Elevator() { ElevatorId = Guid.NewGuid(), Name = "Test Elevator" };

            _elevatorDbContext.Elevators.Add(ev);
            await _elevatorDbContext.SaveChangesAsync();

            ev.CreatedBy.ShouldBe(_loggedInUserId);
        }
    }
}
