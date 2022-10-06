using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
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
    public class LoginCommand : IRequest<LoggedInUserDto>
    {
        public UserForLoginDto UserForLoginDto{get; set;}
        public string IpAddress { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInUserDto>
        {
            IDeveloperRepository _developerRepository;
            DeveloperBusinessRules _developerBusinessRules;
            IUserOperationClaimRepository _userOperationClaimRepository;
            ITokenHelper _tokenHelper;
            IDeveloperService _developerService;

            public LoginCommandHandler(IDeveloperRepository developerRepository, DeveloperBusinessRules developerBusinessRules, IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper, IDeveloperService developerService)
            {
                _developerRepository = developerRepository;
                _developerBusinessRules = developerBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _tokenHelper = tokenHelper;
                _developerService = developerService;
            }

            public async Task<LoggedInUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                Developer? developer = await _developerRepository.GetAsync(u => u.Email == request.UserForLoginDto.Email);
                _developerBusinessRules.UserEmailShouldBeOnSystemWhenRequested(developer);
                _developerBusinessRules.UserPasswordMustBeCorrect(request.UserForLoginDto.Password, developer.PasswordHash, developer.PasswordSalt);

                AccessToken accessToken = await _developerService.CreateAccessToken(developer);
                RefreshToken refreshToken = await _developerService.CreateRefreshToken(developer, request.IpAddress);


                LoggedInUserDto loggedInUserDto = new() { AccessToken = accessToken, RefreshToken = refreshToken };

                return loggedInUserDto;

                
            }
        }
    }
}
