using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Refresh
{
    public class RefreshTokenCommandValidator: AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            
        }
    }
}
