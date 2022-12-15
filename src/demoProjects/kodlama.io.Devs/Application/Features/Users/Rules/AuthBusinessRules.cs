using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Extensions;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Rules
{
    public class AuthBusinessRules
    {

        private readonly IDeveloperRepository _userRepository;
        private readonly IAuthService _developerService;

        public AuthBusinessRules(IDeveloperRepository userRepository, IAuthService developerService)
        {
            _userRepository = userRepository;
            _developerService = developerService;
        }

        public async Task EmailCanNotBeDuplicated(string email)
        {
            IPaginate<Developer> result = await _userRepository.GetListAsync(u => u.Email == email);
            if (result.Items.Any()) throw new BusinessException("This email has already been taken.");

        }

        public void UserEmailShouldBeOnSystemWhenRequested(User user)
        {
            if (user == null) throw new BusinessException("There is no user with given email");
        }

        public void UserPasswordMustBeCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (!HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt)) throw new BusinessException("Password is wrong");
        }

        public void AccessTokenMustBeExpiredToRefresh(AccessToken accessToken)
        {
            if (accessToken.Expiration > DateTime.Now) throw new BusinessException("Access token still valid");
        }

        public void RefreshTokenMustBeExistWhenRequested(RefreshToken refreshToken)
        {
            if (refreshToken == null) throw new AuthorizationException("Refresh token is null");
        }

        public void RefreshTokenMustNotBeExpireated(RefreshToken refreshToken)
        {
            if (refreshToken.Expires < DateTime.Now) throw new AuthorizationException("Refresh token is expired");
        }

        public void RefreshTokenMustNotBeRevoked(RefreshToken refreshToken)
        {
            if (refreshToken.Revoked != null) throw new AuthorizationException("Refresh token is already revoked");
        }

        public void RefreshTokenAndAccessTokenMustOwnSameUser(RefreshToken refreshToken, AccessToken accessToken)
        {
            var claims = _developerService.GetPrincipleFromToken(accessToken);

            if (claims.GetUserId() != refreshToken.UserId) throw new AuthorizationException("Refresh token and Access token does not match");

        }
        public void DeveloperMustBeExistWhenRequested(Developer developer)
        {
            if (developer == null) throw new BusinessException("Developer does not exist");
        }
        public Task UserShouldNotHaveAuthenticator(User user)
        {
            if (user.AuthenticatorType != AuthenticatorType.None) throw new BusinessException("User already have an authenticator type.");
            return Task.CompletedTask;
        }
        public void UserMustBeExistWhenRequested(User user)
        {
            if (user == null) throw new BusinessException("User does not exist");
        }
        public void AuthenticatorMustBeExistWhenRequested(EmailAuthenticator emailAuthenticator)
        {
            if (emailAuthenticator == null) throw new BusinessException("Email authenticator does not exist.");
        }

        public void ActivationKeyMustBeExistWhenRequested(EmailAuthenticator emailAuthenticator)
        {
            if (emailAuthenticator.ActivationKey == null) throw new BusinessException("Email authenticator activation key does not exist.");
        }
        public void OtpAuthenticatorThatVerifiedShouldNotBeExist(OtpAuthenticator? otpAuthenticator)
        {
            if (otpAuthenticator != null && otpAuthenticator.IsVerified == true) throw new BusinessException("Verified Otp Authenticator Should Not Be Exist.");
        }
        public void OtpAuthenticatorShouldBeExist(OtpAuthenticator otpAuthenticator)
        {
            if (otpAuthenticator == null) throw new BusinessException("Otp Authenticator shoul be exist.");
        }
    }
}
