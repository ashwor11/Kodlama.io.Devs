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

namespace Application.Features.OperationClaims.Commands.CreateOperationClaim
{
    public class CreateOperationClaimCommand: IRequest<CreatedOperationClaimDto>
    {
        public string Name { get; set; }

        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreatedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public CreateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<CreatedOperationClaimDto> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                _operationClaimBusinessRules.OperationClaimMustNotBeExistAlreadyWhenCreated(request.Name);

                OperationClaim operationClaim = new()
                {
                    Name = request.Name,
                };
                OperationClaim createdOperationClaim = await _operationClaimRepository.AddAsync(operationClaim);

                CreatedOperationClaimDto createdOperationClaimDto = new()
                {
                    Id = createdOperationClaim.Id,
                    Name = createdOperationClaim.Name
                };
                return createdOperationClaimDto;



            }
        }
    }
}
