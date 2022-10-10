using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;
using Application.Services.Repositories;
using Core.Security.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Commands.UpdateOperationClaim
{
    public class UpdateOperationClaimCommand:IRequest<UpdatedOperationClaimDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommand, UpdatedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public UpdateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<UpdatedOperationClaimDto> Handle(UpdateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                _operationClaimBusinessRules.OperationClaimMustNotBeExistAlreadyWhenUpdated(request.Name);

                OperationClaim operationClaim = new()
                {
                    Id = request.Id,
                    Name = request.Name
                };
                OperationClaim updatedOperationClaim = await _operationClaimRepository.UpdateAsync(operationClaim);

                UpdatedOperationClaimDto updatedOperationClaimDto = new()
                {
                    Id = updatedOperationClaim.Id,
                    Name = updatedOperationClaim.Name
                };
                return updatedOperationClaimDto;
            }
        }
    }
}
