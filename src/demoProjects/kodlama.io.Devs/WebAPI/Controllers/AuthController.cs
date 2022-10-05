using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Dtos;
using Core.Security.Dtos;
using Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto registerDto)
        {
            
            
            RegisterCommand command = new () {
                UserForRegisterDto = registerDto,
                IpAddress = GetIpAddress() 
            };

            RegisteredDto registeredDto= await Mediator.Send(command);
            SetRefreshTokenToCookie(registeredDto.RefreshToken);
            return Ok(registeredDto.AccessToken);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginCommand request)
        //{
        //    AccessTokenDto accessTokenDto = await Mediator.Send(request);
        //    return Ok(accessTokenDto);
        //}

        private void SetRefreshTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
