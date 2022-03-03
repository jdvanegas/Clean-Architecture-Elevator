using Elevator.Management.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elevator.Management.Identity
{
    public class ElevatorIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ElevatorIdentityDbContext(DbContextOptions<ElevatorIdentityDbContext> options) : base(options)
        {
        }
    }
}
