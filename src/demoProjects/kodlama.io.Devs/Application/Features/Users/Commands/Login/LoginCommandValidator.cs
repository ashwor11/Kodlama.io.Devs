using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserForLoginDto.Email).EmailAddress();
            RuleFor(x => x.UserForLoginDto.Password).NotEmpty();
        }
    }
}
