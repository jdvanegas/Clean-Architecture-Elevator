using Elevator.Management.Application.Features.Elevators.Commands.CreateElevator;
using Elevator.Management.Application.Features.Elevators.Queries.GetElevatorState;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevator.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElevatorsController : Controller
    {
        private readonly IMediator _mediator;

        public ElevatorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("state/{id:guid}", Name = "GetCurrentState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ElevatorDto>> GetCurrentState(Guid id) =>
            Ok(await _mediator.Send(new GetElevatorStateQuery { Id = id }));
        

        [HttpPost("create", Name = "CreateElevator"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateElevatorCommand createEventCommand) =>
            Ok(await _mediator.Send(createEventCommand));
        
    }
}
