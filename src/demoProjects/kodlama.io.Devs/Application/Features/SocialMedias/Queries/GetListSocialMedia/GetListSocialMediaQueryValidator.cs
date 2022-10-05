using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Queries.GetListSocialMedia
{
    public class GetListSocialMediaQueryValidator:AbstractValidator<GetListSocialMediaQuery>
    {
        public GetListSocialMediaQueryValidator()
        {
            RuleFor(x => x.PageRequest).NotEmpty();
        }
    }
}
