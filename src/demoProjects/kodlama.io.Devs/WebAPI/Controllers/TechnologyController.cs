using Application.Features.Technologies.Commands.CreateTechnology;
using Application.Features.Technologies.Commands.DeleteTechnology;
using Application.Features.Technologies.Commands.UpdateTechnology;
using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Models;
using Application.Features.Technologies.Queries.GetByDynamicTechnology;
using Application.Features.Technologies.Queries.GetByIdTechnology;
using Application.Features.Technologies.Queries.GetListTechnology;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologyController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTechnologyCommand request)
        {
            CreatedTechnologyDto response = await Mediator.Send(request);
            return Created("", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListTechnologyQuery request = new() { PageRequest = pageRequest };
            TechnologyListModel response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteTechnologyCommand request)
        {
            DeletedTechnologyDto response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTechnologyCommand request)
        {
            UpdatedTechnologyDto response = await Mediator.Send(request);
            return Created("",response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdTechnologyQuery request)
        {
            GetByIdTechnologyDto response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("GetByDynamic")]
        public async Task<IActionResult> GetByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetByDynamicTechnologyQuery request = new() { PageRequest = pageRequest, Dynamic = dynamic };
            TechnologyListModel response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
