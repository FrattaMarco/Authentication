using Authentication.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost]
        [Route("Token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetToken([FromBody] CreateTokenCommand command)
        {
            string token = await _mediator.Send(command);
            return Ok(token);
        }
    }
}
