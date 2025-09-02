using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Application.Commands.Users.UserLogin;
using TaskApp.Application.Commands.Users.UserRegister;

namespace TaskApp.WepApi.Controllers.User
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
