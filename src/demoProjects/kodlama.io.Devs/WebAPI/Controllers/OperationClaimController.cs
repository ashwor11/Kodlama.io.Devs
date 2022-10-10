using Application.Features.OperationClaims.Commands.CreateOperationClaim;
using Application.Features.OperationClaims.Commands.DeleteOperationClaim;
using Application.Features.OperationClaims.Commands.UpdateOperationClaim;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Models;
using Application.Features.OperationClaims.Queries.GetById;
using Application.Features.OperationClaims.Queries.GetList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateOperationClaim([FromBody] CreateOperationClaimCommand request)
        {
            CreatedOperationClaimDto result = await Mediator.Send(request);
            return Created("", result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOperationClaim([FromBody] UpdateOperationClaimCommand request)
        {
            UpdatedOperationClaimDto result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOperationClaim([FromBody] DeleteOperationClaimCommand request)
        {
            DeletedOperationClaimDto result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListOperationClaim([FromQuery] GetListOperationClaimQuery request)
        {
            OperationClaimGetListModel result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByIdOperationClaim([FromQuery] GetByIdOperationClaimQuery request)
        {
            GetByIdOperationClaimDto result = await Mediator.Send(request);
            return Ok(result);
        }


    }
}
