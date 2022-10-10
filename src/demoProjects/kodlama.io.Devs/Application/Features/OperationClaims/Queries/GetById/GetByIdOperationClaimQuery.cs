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

namespace Application.Features.OperationClaims.Queries.GetById
{
    public class GetByIdOperationClaimQuery:IRequest<GetByIdOperationClaimDto>
    {
        public int Id { get; set; }

        public class GetByIdOperationClaimQueryHandler : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public GetByIdOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<GetByIdOperationClaimDto> Handle(GetByIdOperationClaimQuery request, CancellationToken cancellationToken)
            {
                OperationClaim operationClaim = await _operationClaimRepository.GetAsync(x=>x.Id == request.Id);
                _operationClaimBusinessRules.OperationClaimMustBeExistAlreadyWhenRequested(operationClaim);

                GetByIdOperationClaimDto getByIdOperationClaimDto = new()
                {
                    Id = operationClaim.Id,
                    Name = operationClaim.Name
                };

                return getByIdOperationClaimDto;
            }
        }
    }
}
