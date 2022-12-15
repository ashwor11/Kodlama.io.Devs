using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Mailing;
using Core.Security.EmailAuthenticator;
using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.JWT;
using Core.Security.OtpAuthenticator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Application.Services.AuthService
{
    public class AuthManager : IAuthService
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
        private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserRepository _userRepository;

        public AuthManager(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository, IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository, TokenValidationParameters tokenValidationParameters, IConfiguration configuration, IEmailAuthenticatorHelper emailAuthenticatorHelper, IOtpAuthenticatorHelper otpAuthenticatorHelper, IMailService mailService, IEmailAuthenticatorRepository emailAuthenticatorRepository, IOtpAuthenticatorRepository otpAuthenticatorRepository, IUserRepository userRepository)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;
            _configuration = configuration;
            _emailAuthenticatorHelper = emailAuthenticatorHelper;
            _otpAuthenticatorHelper = otpAuthenticatorHelper;
            _mailService = mailService;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _userRepository = userRepository;
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

        public async Task<EmailAuthenticator> CreateEmailAuthenticator(User user)
        {
            EmailAuthenticator emailAuthenticator = new()
            {
                ActivationKey = await _emailAuthenticatorHelper.CreateEmailActivationKey(),
                UserId = user.Id,
                IsVerified = false,
            };
            return emailAuthenticator;
        }

        public async Task<OtpAuthenticator> CreateOtpAuthenticator(User user)
        {
            OtpAuthenticator otpAuthenticator = new()
            {
                IsVerified = false,
                UserId = user.Id,
                SecretKey = await _otpAuthenticatorHelper.GenerateSecretKey(),
            };
            return otpAuthenticator;
        }

        public Task<string> ConvertSecretKeyToString(OtpAuthenticator authenticator)
        {
            return _otpAuthenticatorHelper.ConvertSecretKeyToString(authenticator.SecretKey);
        }

        private async Task VerifyOtpAuthenticatorCode(User user, string code)
        {
            OtpAuthenticator otpAuthenticator = await _otpAuthenticatorRepository.GetAsync(x => x.UserId == user.Id);
            bool result = await _otpAuthenticatorHelper.VerifyCode(otpAuthenticator.SecretKey, code);
            if (!result) throw new BusinessException("Authenticator code is not valid.");
        }

        public async Task SendAuthenticatorCode(User user)
        {
            if (user.AuthenticatorType is AuthenticatorType.Email) sendAuthenticatorCodeWithEmail(user);
        }
        private async Task sendAuthenticatorCodeWithEmail(User user)
        {
            EmailAuthenticator emailAuthenticator = _emailAuthenticatorRepository.Get(x => x.UserId == user.Id);

            string authenticatorCode = await _emailAuthenticatorHelper.CreateEmailActivationCode();
            emailAuthenticator.ActivationKey = authenticatorCode;
            _emailAuthenticatorRepository.Update(emailAuthenticator);

            Mail mail = new()
            {
                ToEmail = user.Email,
                ToFullName = user.FirstName + " " + user.LastName,
                Subject = "Authenticator Code - Kodlama.IoDevs",
                TextBody = $"Authenticator code is {authenticatorCode}."
            };
            _mailService.SendMail(mail);

           
        }
        public async Task VerifyOtpAuthenticatorCode(OtpAuthenticator otpAuthenticator, string code)
        {
            bool result = await _otpAuthenticatorHelper.VerifyCode(otpAuthenticator.SecretKey, code);
            if (!result) throw new BusinessException("Authenticator code is not valid.");
        }

        public async Task VerifyAuthenticatorCode(User user, string code)
        {
            if(user.AuthenticatorType == AuthenticatorType.Otp)
            {
                await VerifyOtpAuthenticatorCode(user, code);
            }else if(user.AuthenticatorType == AuthenticatorType.Email)
            {
                await VerifyEmailAuthenticatorCode(user, code);
            }
        }
        private async Task VerifyEmailAuthenticatorCode(User user, string code)
        {
            EmailAuthenticator? emailAuthenticator = await _emailAuthenticatorRepository.GetAsync(x=> x.UserId == user.Id);
            if (emailAuthenticator.ActivationKey != code) throw new BusinessException("Email activation code is not valid.");
        }

        public void ResetVertificationType(User user)
        {
            if(user.AuthenticatorType == AuthenticatorType.Email)
            {
                DeleteEmailAuthenticator(user);
            }else if(user.AuthenticatorType == AuthenticatorType.Otp)
            {
                DeleteOtpAuthenticator(user);
            }
            

            user.AuthenticatorType = AuthenticatorType.None;
            _userRepository.UpdateAsync(user);
            return;
            
            
            
        }
        private void DeleteOtpAuthenticator(User user)
        {
            OtpAuthenticator? otpAuthenticator = _otpAuthenticatorRepository.Get(x=> x.UserId == 1);
            _otpAuthenticatorRepository.Delete(otpAuthenticator);
        }
        private void DeleteEmailAuthenticator(User user)
        {
            EmailAuthenticator? emailAuthenticator = _emailAuthenticatorRepository.Get(x=> x.UserId == user.Id);
            _emailAuthenticatorRepository.Delete(emailAuthenticator);
        }
    }
}
