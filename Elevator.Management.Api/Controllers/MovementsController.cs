using Elevator.Management.Application.Features.Movement.Commands.ExecuteMovement;
using Elevator.Management.Application.Features.Movements.Commands.CreateMovement;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateMovementCommand createEventCommand) =>
             Created("", await _mediator.Send(createEventCommand));

        [HttpPut("execute/{elevatorId:guid}", Name = "ExecuteMovement")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Execute(Guid elevatorId)
        {
            await _mediator.Send(new ExecuteMovementCommand { ElevatorId = elevatorId });
            return NoContent();
        }
    }
}