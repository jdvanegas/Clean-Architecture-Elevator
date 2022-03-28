using AutoMapper;
using Elevator.Management.Application.Validation;
using Elevator.Management.Application.Validation.Elevator;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Elevator.Management.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining(typeof(CreateElevatorValidator)));
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
            return services;
        }
    }
}