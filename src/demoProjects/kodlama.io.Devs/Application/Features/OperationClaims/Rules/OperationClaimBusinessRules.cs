using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository)
        {
            _operationClaimRepository = operationClaimRepository;
        }

        public async void OperationClaimMustNotBeExistAlreadyWhenCreated(string operationClaimName)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == operationClaimName);
            if (operationClaim != null) throw new BusinessException("Operation Claim name already exists.");
        }
        public async void OperationClaimMustNotBeExistAlreadyWhenUpdated(string operationClaimName)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == operationClaimName);

            if (operationClaim != null) throw new BusinessException("Operation Claim name already exists.");
        }
        public async void OperationClaimMustBeExistAlreadyWhenDeleted(int operationClaimId)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Id == operationClaimId);
            if (operationClaim == null) throw new BusinessException("Operation Claim does not exists.");
        }
        public void OperationClaimMustBeExistAlreadyWhenRequested(OperationClaim operationClaim)
        {
            if (operationClaim == null) throw new BusinessException("Operation Claim does not exists.");
        }
    }
}

