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
    public interface IAuthService
    {
        public Task<AccessToken> CreateAccessToken(User user);
        public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
        public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
        public Task<UserOperationClaim> CreateAndAddUserClaim(User user);
        public bool IsTokenValid(AccessToken accessToken);
        public ClaimsPrincipal GetPrincipleFromToken(AccessToken accessToken);
        public Task<RefreshToken> RevokeRefreshToken(RefreshToken refreshToken,string ipAddress,string replacedToken,string reasonRevoked);
        public Task<EmailAuthenticator> CreateEmailAuthenticator(User user);
        public Task<OtpAuthenticator> CreateOtpAuthenticator(User user);

        public Task<string> ConvertSecretKeyToString(OtpAuthenticator authenticator);
        public Task VerifyOtpAuthenticatorCode(OtpAuthenticator otpAuthenticator, string code);
        public Task SendAuthenticatorCode(User user);
        public Task VerifyAuthenticatorCode(User user, string code);
        public void ResetVertificationType(User user);



    }
}
