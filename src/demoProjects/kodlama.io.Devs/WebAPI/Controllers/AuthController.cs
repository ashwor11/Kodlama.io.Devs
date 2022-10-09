using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Refresh;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Dtos;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto loginDto)
        {
            LoginCommand command = new()
            {
                UserForLoginDto = loginDto,
                IpAddress = GetIpAddress()
            };
            LoggedInUserDto loggedInUserDto = await Mediator.Send(command);
            SetRefreshTokenToCookie(loggedInUserDto.RefreshToken);
            return Ok(loggedInUserDto.AccessToken);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Login([FromBody] AccessToken accessToken)
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) return BadRequest();
            RefreshTokenCommand command = new()
            {
                RefreshTokenDto = new () { AccessToken = accessToken, RefreshToken = refreshToken },
                IpAddress = GetIpAddress()
            };
            RefreshedTokenDto refreshedTokenDto = await Mediator.Send(command);
            SetRefreshTokenToCookie(refreshedTokenDto.RefreshToken);
            return Ok(refreshedTokenDto.AccessToken);
        }

        private void SetRefreshTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
