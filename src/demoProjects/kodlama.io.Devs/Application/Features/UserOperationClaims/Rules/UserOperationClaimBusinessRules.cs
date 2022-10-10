using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Rules
{
    public class UserOperationClaimBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;

        public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
        }

        public async void UserOperationClaimMustNotBeExistWhenCreated(UserOperationClaim userOperationClaim)
        {
            UserOperationClaim? storedUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x == userOperationClaim);

            if (storedUserOperationClaim != null) throw new BusinessException("User operation already exists.");
        }

        public void UserOperationClaimMustBeExistWhenDeleted(UserOperationClaim? userOperationClaim)
        {
            if (userOperationClaim == null) throw new BusinessException("User operation does not exist.");
        }
    }
}
