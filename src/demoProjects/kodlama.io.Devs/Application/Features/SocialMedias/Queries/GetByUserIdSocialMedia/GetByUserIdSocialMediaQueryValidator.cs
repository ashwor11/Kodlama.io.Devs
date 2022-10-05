using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Queries.GetByUserIdSocialMedia
{
    public class GetByUserIdSocialMediaQueryValidator: AbstractValidator<GetByUserIdSocialMediaQuery>
    {
        public GetByUserIdSocialMediaQueryValidator()
        {
            RuleFor(x=>x.DeveloperId).NotEmpty();
            RuleFor(x=>x.PageRequest).NotEmpty();
        }
    }
}
