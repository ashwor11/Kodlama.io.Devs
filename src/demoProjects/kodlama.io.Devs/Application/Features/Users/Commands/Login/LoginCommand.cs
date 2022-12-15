using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Logging;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<LoggedInUserDto>, ILoggableRequest
    {
        public UserForLoginDto? UserForLoginDto{get; set;}
        public string? IpAddress { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInUserDto>
        {
            IUserRepository _userRepository;
            AuthBusinessRules _developerBusinessRules;
            IUserOperationClaimRepository _userOperationClaimRepository;
            ITokenHelper _tokenHelper;
            IAuthService _authService;

            public LoginCommandHandler(IUserRepository userRepository, AuthBusinessRules developerBusinessRules, IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper, IAuthService developerService)
            {
                _userRepository = userRepository;
                _developerBusinessRules = developerBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _tokenHelper = tokenHelper;
                _authService = developerService;
            }

            public async Task<LoggedInUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                User? user = _userRepository.Get(u => u.Email == request.UserForLoginDto.Email);
                _developerBusinessRules.UserEmailShouldBeOnSystemWhenRequested(user);
                _developerBusinessRules.UserPasswordMustBeCorrect(request.UserForLoginDto.Password, user.PasswordHash, user.PasswordSalt);

                LoggedInUserDto userDto = new();
                if(user.AuthenticatorType is not AuthenticatorType.None)
                {
                    if(request.UserForLoginDto.AuthenticatorCode is null || request.UserForLoginDto.AuthenticatorCode == "")
                    {
                        await _authService.SendAuthenticatorCode(user);
                        userDto.RequiredAuthenticatorType = user.AuthenticatorType;
                        return userDto;
                    }

                    await _authService.VerifyAuthenticatorCode(user, request.UserForLoginDto.AuthenticatorCode);
                }

                

                AccessToken accessToken = await _authService.CreateAccessToken(user);
                RefreshToken refreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);

                await _authService.AddRefreshToken(refreshToken);


                LoggedInUserDto loggedInUserDto = new() { AccessToken = accessToken, RefreshToken = refreshToken };

                return loggedInUserDto;

                
            }
        }
    }
}
