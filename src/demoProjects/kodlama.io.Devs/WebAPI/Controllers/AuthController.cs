using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand request)
        {
            AccessTokenDto accessTokenDto = await Mediator.Send(request);
            return Ok(accessTokenDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            AccessTokenDto accessTokenDto = await Mediator.Send(request);
            return Ok(accessTokenDto);
        }
    }
}
