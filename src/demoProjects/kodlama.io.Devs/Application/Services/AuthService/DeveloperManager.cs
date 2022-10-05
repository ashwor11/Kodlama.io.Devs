using Application.Services.Repositories;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthService
{
    public class DeveloperManager : IDeveloperService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;

        public DeveloperManager(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository, IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;

        }

        public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
            return addedRefreshToken;
        }

        public async Task<AccessToken> CreateAccessToken(User user)
        {
            var claims = await _userOperationClaimRepository.GetListAsync(predicate: u=> u.Id == user.Id,
                                                                    include: u => u.Include(u=> u.OperationClaim));
            IList<OperationClaim> operationClaims =claims.Items.Select(u => new OperationClaim() { Id = u.OperationClaim.Id, Name = u.OperationClaim.Name }).ToList();

            return _tokenHelper.CreateToken(user, operationClaims);


        }

        public async Task<UserOperationClaim> CreateAndAddUserClaim(User user)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == "User");
            UserOperationClaim? userOperationClaim = new() { UserId = user.Id, OperationClaimId = operationClaim.Id };
            return await _userOperationClaimRepository.AddAsync(userOperationClaim);
        }

        public async Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
        {
            RefreshToken refreshToken =  _tokenHelper.CreateRefreshToken(user, ipAddress);
            return refreshToken;
        }

        
    }
}
