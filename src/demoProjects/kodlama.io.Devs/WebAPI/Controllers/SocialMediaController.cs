using Application.Features.SocialMedias.Commands.CreateSocialMedia;
using Application.Features.SocialMedias.Dtos;
using Application.Features.SocialMedias.Models;
using Application.Features.SocialMedias.Queries.GetByIdSocialMedia;
using Application.Features.SocialMedias.Queries.GetByUserIdSocialMedia;
using Application.Features.SocialMedias.Queries.GetListSocialMedia;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSocialMediaCommand request)
        {
            CreatedSocialMediaDto result = await Mediator.Send(request);
            return Created("", result);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdSocialMediaQuery request)
        {
            GetByIdSocialMediaDto result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetListSocialMediaQuery request)
        {
            SocialMediaListModel result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByDeveloperId([FromQuery] GetByUserIdSocialMediaQuery request)
        {
            SocialMediaByUserListModel result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
