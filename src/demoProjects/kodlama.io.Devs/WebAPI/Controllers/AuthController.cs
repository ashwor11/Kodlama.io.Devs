using Application.Features.Users.Commands.EnableEmailVertification;
using Application.Features.Users.Commands.EnableOtpVertification;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Refresh;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Commands.ResetVertificationType;
using Application.Features.Users.Commands.VerifyEmailVertification;
using Application.Features.Users.Commands.VerifyOtpVertification;
using Application.Features.Users.Dtos;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Web;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController

        
    {
        private readonly WebAPIConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("WebAPIConfiguration").Get<WebAPIConfiguration>();
        }
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
            LoggedInUserDto result = await Mediator.Send(command);
            if(result.RefreshToken is not null) SetRefreshTokenToCookie(result.RefreshToken);
            return Ok(result.CreateResponseDto());
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

        [HttpGet("EnableEmailVertification")]
        public async Task<IActionResult> EnableEmailVertification()
        {
            EnableEmailVertificationCommand command = new()
            {
                UserId = GetUserId(),
                VerifyEmailPrefix = $"{_configuration.APIDomain}/Auth/VerifyEmailVertification"
            };
            await Mediator.Send(command);
            return Ok();
        }

        [HttpGet("VerifyEmailVertification")]
        public async Task<IActionResult> VerifyEmailVertification([FromQuery] VerifyEmailVertificationCommand command)
        {
            command.ActivationKey = HttpUtility.UrlDecode(command.ActivationKey);
           
            
            await Mediator.Send(command);
            return Ok();
            
        }

        [HttpGet("EnableOtpVertification")]
        public async Task<IActionResult> EnableOtpVertification()
        {
            EnableOtpVertificationCommand command = new()
            {
                UserId = GetUserId()
            };
            EnabledOtpAuthenticatorDto result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("VerifyOtpVertification")]
        public async Task<IActionResult> VerifyOtpVertification([FromBody] string authenticatorCode)
        {
            VerifyOtpVertificationCommand command = new()
            {
                UserId = GetUserId(),
                ActivationCode = authenticatorCode
            };
            await Mediator.Send(command);

            return Ok();
        }
        [HttpGet("ResetVertificationType")]
        public async Task<IActionResult> ResetVertificationType()
        {
            ResetVertificationTypeCommand request = new() { UserId = GetUserId() };
            Mediator.Send(request);
            return Ok();
        }

        private void SetRefreshTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
