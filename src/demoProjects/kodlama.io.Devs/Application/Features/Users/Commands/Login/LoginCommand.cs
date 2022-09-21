using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand: IRequest<AccessTokenDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, AccessTokenDto>
        {
            IMapper _mapper;
            IUserRepository _userRepository;
            UserBusinessRules _userBusinessRules;
            IUserOperationClaimRepository _userOperationClaimRepository;
            ITokenHelper _tokenHelper;

            public LoginCommandHandler(IMapper mapper, IUserRepository userRepository, UserBusinessRules userBusinessRules, IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper)
            {
                _mapper = mapper;
                _userRepository = userRepository;
                _userBusinessRules = userBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _tokenHelper = tokenHelper;
            }

            public async Task<AccessTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                User? developer = await _userRepository.GetAsync(u => u.Email == request.Email);
                _userBusinessRules.UserEmailShouldBeOnSystemWhenRequested(developer);
                _userBusinessRules.UserPasswordMustBeCorrect(request.Password, developer.PasswordHash, developer.PasswordSalt);

                var userOperationClaims = await _userOperationClaimRepository.GetListAsync(
                                                                                     predicate: o => o.UserId == developer.Id,
                                                                                     include: o => o.Include(c => c.OperationClaim));
                IList<OperationClaim> operationClaims = new List<OperationClaim>();

                AccessToken accessToken = _tokenHelper.CreateToken(developer, operationClaims);

                AccessTokenDto accessTokenDto = _mapper.Map<AccessTokenDto>(accessToken);

                return accessTokenDto;

                
            }
        }
    }
}
