using Elevator.Management.Application.Contracts.Persistence;
using Elevator.Management.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elevator.Management.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ElevatorDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ElevatorManagementConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IElevatorRepository, ElevatorRepository>();

            return services;    
        }
    }
}
