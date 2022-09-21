using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Register
{
    public class CreateUserCommand: IRequest<AccessTokenDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AccessTokenDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly ITokenHelper _tokenHelper;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;

            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules, ITokenHelper tokenHelper, IUserOperationClaimRepository userOperationClaimRepository)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _tokenHelper = tokenHelper;
                _userOperationClaimRepository = userOperationClaimRepository;
            }

            public async Task<AccessTokenDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.EmailCanNotBeDuplicated(request.Email);

                Byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

                User user = _mapper.Map<User>(request);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Status = true;

                User newUser = await _userRepository.AddAsync(user);

                var token = _tokenHelper.CreateToken(newUser, new List<OperationClaim>());

                AccessTokenDto accessTokenDto = _mapper.Map<AccessTokenDto>(token);
                return accessTokenDto;

            }
        }
    }
}
