using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Queries.GetListByUser
{
    public class GetListByUserUserOperationClaimQueryValidator:AbstractValidator<GetListByUserUserOperationClaimQuery>
    {
        public GetListByUserUserOperationClaimQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
