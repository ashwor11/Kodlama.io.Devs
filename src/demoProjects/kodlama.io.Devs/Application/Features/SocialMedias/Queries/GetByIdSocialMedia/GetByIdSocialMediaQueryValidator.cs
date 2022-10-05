using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Queries.GetByIdSocialMedia
{
    public class GetByIdSocialMediaQueryValidator:AbstractValidator<GetByIdSocialMediaQuery>
    {
        public GetByIdSocialMediaQueryValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();
        }
    }
}
