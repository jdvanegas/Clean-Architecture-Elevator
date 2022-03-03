using Elevator.Management.Application.Features.Movement.Commands.CreateMovement;
using Elevator.Management.Application.Features.Movement.Commands.ExecuteMovement;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevator.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create", Name = "CreateMovement")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateMovementCommand createEventCommand)
        {
            var id = await _mediator.Send(createEventCommand);
            return Ok(id);
        }

        [HttpPut("execute/{elevatorId}", Name = "ExecuteMovement")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Execute(Guid elevatorId)
        {
            await _mediator.Send(new ExecuteMovementCommand { ElevatorId = elevatorId});
            return NoContent();
        }
    }
}
