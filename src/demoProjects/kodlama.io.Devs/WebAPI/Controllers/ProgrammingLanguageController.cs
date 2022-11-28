using Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Commands.DeleteProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Commands.UpdateProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Dtos;
using Application.Features.ProgrammingLanguages.Models;
using Application.Features.ProgrammingLanguages.Queries.GetByIdProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Queries.GetListProgrammingLanguage;
using Core.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammingLanguageController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProgrammingLanguageCommand request)
        {
            CreatedProgrammingLanguageDto result = await Mediator.Send(request);
            return Created("", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListProgrammingLanguageQuery query = new() { PageRequest = pageRequest };
            ProgrammingLanguageListModel result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProgrammingLanguageCommand request)
        {
            UpdatedProgrammingLanguageDto result = await Mediator.Send(request);
            return Created("", result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteProgrammingLanguageCommand request)
        {
            DeletedProgrammingLanguageDto result = await Mediator.Send(request);
            return NoContent();
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetList([FromRoute] GetByIdProgrammingLanguageQuery request)
        {
            ProgrammingLanguageGetByIdDto result = await Mediator.Send(request);
            return Ok(result);
        }

    }
}
