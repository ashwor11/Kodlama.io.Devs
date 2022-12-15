using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.EnableOtpVertification
{
    public class EnableOtpVertificationCommandValidator : AbstractValidator<EnableOtpVertificationCommand>
    {
        public EnableOtpVertificationCommandValidator()
        {
            RuleFor(X=>X.UserId).NotEmpty();
        }
    }
}
