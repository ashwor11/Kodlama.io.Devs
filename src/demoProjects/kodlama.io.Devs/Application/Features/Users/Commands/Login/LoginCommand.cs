using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
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
    public class LoginCommand: IRequest<LoggedInUserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInUserDto>
        {
            IMapper _mapper;
            IDeveloperRepository _developerRepository;
            DeveloperBusinessRules _developerBusinessRules;
            IUserOperationClaimRepository _userOperationClaimRepository;
            ITokenHelper _tokenHelper;

            public LoginCommandHandler(IMapper mapper, IDeveloperRepository developerRepository, DeveloperBusinessRules developerBusinessRules, IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper)
            {
                _mapper = mapper;
                _developerRepository = developerRepository;
                _developerBusinessRules = developerBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _tokenHelper = tokenHelper;
            }

            public async Task<LoggedInUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                Developer? developer = await _developerRepository.GetAsync(u => u.Email == request.Email);
                _developerBusinessRules.UserEmailShouldBeOnSystemWhenRequested(developer);
                _developerBusinessRules.UserPasswordMustBeCorrect(request.Password, developer.PasswordHash, developer.PasswordSalt);

                var userOperationClaims = await _userOperationClaimRepository.GetListAsync(
                                                                                     predicate: o => o.UserId == developer.Id,
                                                                                     include: o => o.Include(c => c.OperationClaim));
                

                AccessToken accessToken = _tokenHelper.CreateToken(developer, userOperationClaims.Items.Select(p => p.OperationClaim).ToList());

                LoggedInUserDto accessTokenDto = _mapper.Map<LoggedInUserDto>(accessToken);

                return accessTokenDto;

                
            }
        }
    }
}
