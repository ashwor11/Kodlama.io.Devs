using Core.Security.Entities;
using Core.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthService
{
    public interface IDeveloperService
    {
        public Task<AccessToken> CreateAccessToken(User user);
        public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
        public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
        public Task<UserOperationClaim> CreateAndAddUserClaim(User user);
        public bool IsTokenValid(AccessToken accessToken);
        public ClaimsPrincipal GetPrincipleFromToken(AccessToken accessToken);
        public Task<RefreshToken> RevokeRefreshToken(RefreshToken refreshToken,string ipAddress,string replacedToken,string reasonRevoked);



    }
}
