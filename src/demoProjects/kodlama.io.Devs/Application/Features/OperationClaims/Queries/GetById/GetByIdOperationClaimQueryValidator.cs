using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Queries.GetById
{
    public class GetByIdOperationClaimQueryValidator:AbstractValidator<GetByIdOperationClaimQuery>
    {
        public GetByIdOperationClaimQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
