using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Application.Services.UserService;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.VerifyOtpVertification
{
    public class VerifyOtpVertificationCommand : IRequest
    {
        public int UserId { get; set; }
        public string ActivationCode { get; set; }

        public class VerifyOtpVertificationCommandHandler : IRequestHandler<VerifyOtpVertificationCommand>
        {
            private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
            private readonly IUserService _userService;
            private readonly IAuthService _authService;
            private readonly AuthBusinessRules _authBusinessRules;

            public VerifyOtpVertificationCommandHandler(IOtpAuthenticatorRepository otpAuthenticatorRepository, IUserService userService, IAuthService authService, AuthBusinessRules authBusinessRules)
            {
                _otpAuthenticatorRepository = otpAuthenticatorRepository;
                _userService = userService;
                _authService = authService;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<Unit> Handle(VerifyOtpVertificationCommand request, CancellationToken cancellationToken)
            {
                OtpAuthenticator otpAuthenticator = await _otpAuthenticatorRepository.GetAsync(x => x.UserId == request.UserId);
                _authBusinessRules.OtpAuthenticatorShouldBeExist(otpAuthenticator);

                User user = await _userService.GetById(request.UserId);

                user.AuthenticatorType = AuthenticatorType.Otp;

                otpAuthenticator.IsVerified = true;

                await _authService.VerifyOtpAuthenticatorCode(otpAuthenticator, request.ActivationCode);

                await _otpAuthenticatorRepository.UpdateAsync(otpAuthenticator);
                await _userService.Update(user);

                return Unit.Value;

            }
        }
    }
}
