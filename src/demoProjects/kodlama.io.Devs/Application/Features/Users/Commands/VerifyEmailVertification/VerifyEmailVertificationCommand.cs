using Application.Features.Users.Rules;
using Application.Services.Repositories;
using Core.Security.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.VerifyEmailVertification
{
    public class VerifyEmailVertificationCommand: IRequest
    {
        public string ActivationKey { get; set; }

        public class VerifyEmailVertificationCommandHandler : IRequestHandler<VerifyEmailVertificationCommand>
        {
            IEmailAuthenticatorRepository _emailAuthenticatorRepository;
            AuthBusinessRules _authBusinessRules;

            public VerifyEmailVertificationCommandHandler(IEmailAuthenticatorRepository emailAuthenticatorRepository, AuthBusinessRules authBusinessRules)
            {
                _emailAuthenticatorRepository = emailAuthenticatorRepository;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<Unit> Handle(VerifyEmailVertificationCommand request, CancellationToken cancellationToken)
            {
                EmailAuthenticator? authenticator = await _emailAuthenticatorRepository.GetAsync(x => x.ActivationKey == request.ActivationKey);
                _authBusinessRules.AuthenticatorMustBeExistWhenRequested(authenticator);
                _authBusinessRules.ActivationKeyMustBeExistWhenRequested(authenticator);

                authenticator.ActivationKey = null;
                authenticator.IsVerified = true;
                await _emailAuthenticatorRepository.UpdateAsync(authenticator);

                return Unit.Value;
            }
        }
    }
}
