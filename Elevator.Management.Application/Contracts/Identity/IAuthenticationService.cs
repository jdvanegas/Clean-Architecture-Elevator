using Elevator.Management.Application.Models.Authentication;
using System.Threading.Tasks;

namespace Elevator.Management.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}
