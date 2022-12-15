using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Application.Services.UserService;
using Core.Security.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.EnableOtpVertification
{
    public class EnableOtpVertificationCommand : IRequest<EnabledOtpAuthenticatorDto>
    {
        public int UserId { get; set; }

        public class EnableOtpVertificationCommandHandler : IRequestHandler<EnableOtpVertificationCommand, EnabledOtpAuthenticatorDto>
        {
            private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
            private readonly AuthBusinessRules _authBusinessRules;
            private readonly IUserRepository _userRepository;
            private readonly IAuthService _authService;
            private readonly IUserService _userService;

            public EnableOtpVertificationCommandHandler(IOtpAuthenticatorRepository otpAuthenticatorRepository, AuthBusinessRules authBusinessRules, IUserRepository userRepository, IAuthService authService, IUserService userService)
            {
                _otpAuthenticatorRepository = otpAuthenticatorRepository;
                _authBusinessRules = authBusinessRules;
                _userRepository = userRepository;
                _authService = authService;
                _userService = userService;
            }

            public async Task<EnabledOtpAuthenticatorDto> Handle(EnableOtpVertificationCommand request, CancellationToken cancellationToken)
            {
                User user = _userRepository.Get(x => x.Id == request.UserId);
                _authBusinessRules.UserMustBeExistWhenRequested(user);
                await _authBusinessRules.UserShouldNotHaveAuthenticator(user);

                OtpAuthenticator? isExistOtpAuthenticator = await _otpAuthenticatorRepository.GetAsync(x => x.UserId == request.UserId);
                _authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExist(isExistOtpAuthenticator);

                if (isExistOtpAuthenticator != null) await _otpAuthenticatorRepository.DeleteAsync(isExistOtpAuthenticator);

                OtpAuthenticator newOtpAuthenticator = await _authService.CreateOtpAuthenticator(user);

                OtpAuthenticator addedAuthenticator = await _otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);

                EnabledOtpAuthenticatorDto enabledOtpAuthenticatorDto = new()
                {
                    SecretKey = await _authService.ConvertSecretKeyToString(addedAuthenticator)
                };
                
                return enabledOtpAuthenticatorDto;
            }
        }
    }
}
