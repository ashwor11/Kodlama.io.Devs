using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Application.Services.UserService;
using Core.Mailing;
using Core.Security.Entities;
using Core.Security.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Features.Users.Commands.EnableEmailVertification
{
    public class EnableEmailVertificationCommand: IRequest
    {
        public int UserId { get; set; }
        public string VerifyEmailPrefix { get; set; }

        public class EnableEmailVertificationCommandHandler : IRequestHandler<EnableEmailVertificationCommand>
        {
            IAuthService _authService;
            IUserService _userService;
            IEmailAuthenticatorRepository _emailAuthenticatorRepository;
            AuthBusinessRules _authBusinessRules;
            IMailService _mailService;

            public EnableEmailVertificationCommandHandler(IAuthService authService, IUserService userService, IEmailAuthenticatorRepository emailAuthenticatorRepository, AuthBusinessRules authBusinessRules, IMailService mailService)
            {
                _authService = authService;
                _userService = userService;
                _emailAuthenticatorRepository = emailAuthenticatorRepository;
                _authBusinessRules = authBusinessRules;
                _mailService = mailService;
            }

            public async Task<Unit> Handle(EnableEmailVertificationCommand request, CancellationToken cancellationToken)
            {


                User user = _userService.GetById(request.UserId).Result;
                _authBusinessRules.UserMustBeExistWhenRequested(user);
                _authBusinessRules.UserShouldNotHaveAuthenticator(user);

                user.AuthenticatorType = AuthenticatorType.Email;
                _userService.Update(user);

                EmailAuthenticator emailAuthenticator = await _authService.CreateEmailAuthenticator(user);

                EmailAuthenticator addedEmailAuthenticator = await _emailAuthenticatorRepository.AddAsync(emailAuthenticator);

                _mailService.SendMail(new()
                {
                    ToEmail = user.Email,
                    Subject = "Vertification for your Kodlama.Io Account",
                    TextBody = $"Click on the link to verify your email : {request.VerifyEmailPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}",
                    ToFullName = user.FirstName + " " + user.LastName
                });

                return Unit.Value; //to do return a dto



            }
        }
    }
}
