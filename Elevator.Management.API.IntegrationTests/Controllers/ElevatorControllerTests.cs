using Elevator.Management.Api;
using Elevator.Management.API.IntegrationTests.Base;
using Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;

namespace Elevator.Management.API.IntegrationTests.Controllers
{

    public class ElevatorControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ElevatorControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsSuccessResult()
        {
            var client = _factory.GetAnonymousClient();

            var response = await client.GetAsync("/api/elevators/state/6313179F-7837-473A-A4D5-A5571B43E6A6");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ElevatorDto>(responseString);
            
            Assert.IsType<ElevatorDto>(result);
            Assert.NotNull(result);
        }
    }
}
