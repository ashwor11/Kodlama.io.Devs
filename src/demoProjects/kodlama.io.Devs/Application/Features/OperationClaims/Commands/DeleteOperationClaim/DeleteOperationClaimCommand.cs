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

namespace Application.Features.OperationClaims.Commands.DeleteOperationClaim
{
    public class DeleteOperationClaimCommand:IRequest<DeletedOperationClaimDto>
    {
        public int Id { get; set; }

        public class DeleteOperationClaimCommandHandler : IRequestHandler<DeleteOperationClaimCommand, DeletedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public DeleteOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<DeletedOperationClaimDto> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
            {
                _operationClaimBusinessRules.OperationClaimMustBeExistAlreadyWhenDeleted(request.Id);

                OperationClaim operationClaim = new()
                {
                    Id = request.Id
                };
                OperationClaim deletedOperationClaim = await _operationClaimRepository.DeleteAsync(operationClaim);
                DeletedOperationClaimDto deletedOperationClaimDto = new()
                {
                    Id = deletedOperationClaim.Id,
                    Name = deletedOperationClaim.Name
                };
                return deletedOperationClaimDto;
            }
        }
    }
}
