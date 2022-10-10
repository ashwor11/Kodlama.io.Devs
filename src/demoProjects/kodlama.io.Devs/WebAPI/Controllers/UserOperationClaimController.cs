using Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;
using Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim;
using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Models;
using Application.Features.UserOperationClaims.Queries.GetListByUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateUserOperationClaim([FromBody] GetListByUserUserOperationClaimCommand request)
        {
            CreatedUserOperationClaimDto result = await Mediator.Send(request);
            return Created("", result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserOperationClaim([FromBody] DeleteUserOperationClaimCommand request)
        {
            DeletedUserOperationClaimDto result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserOperationClaims([FromQuery] GetListByUserUserOperationClaimQuery request)
        {
            GetListByUserUserOperationClaimModel result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
