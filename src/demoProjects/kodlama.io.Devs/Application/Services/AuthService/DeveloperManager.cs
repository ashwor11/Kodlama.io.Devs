using Application.Services.Repositories;
using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Application.Services.AuthService
{
    public class DeveloperManager : IDeveloperService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IConfiguration _configuration;

        public DeveloperManager(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository, IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository, TokenValidationParameters tokenValidationParameters, IConfiguration configuration)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;
            _configuration = configuration;
        }

        public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
            return addedRefreshToken;
        }

        public async Task<AccessToken> CreateAccessToken(User user)
        {
            var claims = await _userOperationClaimRepository.GetListAsync(predicate: u=> u.UserId == user.Id,
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
            RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
            return refreshToken;
        }

        public ClaimsPrincipal? GetPrincipleFromToken(AccessToken accessToken)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken.Token, GetTokenValidationParameters(), out var validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        
        public bool IsTokenValid(AccessToken accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

           
                tokenHandler.ValidateToken(accessToken.Token, GetTokenValidationParameters(), out var validatedToken);
                return true;
            
        }

        public async Task<RefreshToken> RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string replacedToken, string reasonRevoked)
        {
            refreshToken.Revoked = DateTime.Now;
            refreshToken.ReasonRevoked = reasonRevoked;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = replacedToken;
            return await _refreshTokenRepository.UpdateAsync(refreshToken);
        }

        private bool IsValidJwtAlghoritm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)
                   && jwtSecurityToken.Header.Alg
                       .Equals(SecurityAlgorithms.HmacSha512, System.StringComparison.InvariantCultureIgnoreCase);
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            TokenOptions tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = tokenOptions.Audience,
                ValidIssuer = tokenOptions.Issuer,
                ValidateLifetime = true,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
            };
            return tokenValidationParameters;
        }


    }
}
