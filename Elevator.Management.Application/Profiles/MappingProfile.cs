using AutoMapper;
using Elevator.Management.Application.Features.Elevators.Commands.CreateElevator;
using Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState;
using Elevator.Management.Application.Features.Movements.Commands.CreateMovement;
using Elevator.Management.Domain.Entities;

namespace Elevator.Management.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Elevator, ElevatorDto>().ReverseMap();
            CreateMap<Domain.Entities.Elevator, CreateElevatorCommand>().ReverseMap();
            CreateMap<Movement, CreateMovementCommand>().ReverseMap();
        }
    }
}
