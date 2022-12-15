using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ResetVertificationType
{
    public class ResetVertificationTypeCommand: IRequest
    {
        public int UserId { get; set; }

        public class ResetVertificationTypeCommandHandler : IRequestHandler<ResetVertificationTypeCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly AuthBusinessRules _authBusinessRules;
            private readonly IAuthService _authService;

            public ResetVertificationTypeCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules, IAuthService authService)
            {
                _userRepository = userRepository;
                _authBusinessRules = authBusinessRules;
                _authService = authService;
            }

            public async Task<Unit> Handle(ResetVertificationTypeCommand request, CancellationToken cancellationToken)
            {
                User? user = await _userRepository.GetAsync(x => x.Id == request.UserId);
                _authBusinessRules.UserMustBeExistWhenRequested(user);

                if(user.AuthenticatorType != AuthenticatorType.None) _authService.ResetVertificationType(user);

                return Unit.Value;
            }
        }
    }
}
