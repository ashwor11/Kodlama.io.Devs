using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Rules
{
    public class SocialMediaBusinessRules
    {
        public void RequestedSocialMediaCanNotBeNull(SocialMedia socialMedia)
        {
            if (socialMedia == null) throw new BusinessException("Requested social media does not exist.");
        }
    }
}
