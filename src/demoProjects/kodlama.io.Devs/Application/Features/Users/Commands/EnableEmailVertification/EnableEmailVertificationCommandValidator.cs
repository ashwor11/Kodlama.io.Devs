using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.EnableEmailVertification
{
    public class EnableEmailVertificationCommandValidator : AbstractValidator<EnableEmailVertificationCommand>
    {
        public EnableEmailVertificationCommandValidator()
        {
            RuleFor(X=>X.VerifyEmailPrefix).NotEmpty();
            RuleFor(x=>x.UserId).NotEmpty();
        }
    }
}
