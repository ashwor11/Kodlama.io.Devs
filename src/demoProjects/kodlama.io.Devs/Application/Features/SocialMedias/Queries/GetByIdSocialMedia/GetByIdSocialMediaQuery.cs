using Application.Features.SocialMedias.Dtos;
using Application.Features.SocialMedias.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Queries.GetByIdSocialMedia
{
    public class GetByIdSocialMediaQuery: IRequest<GetByIdSocialMediaDto>
    {
        public int Id { get; set; }

        public class GetByIdSocialMediaQueryHandler : IRequestHandler<GetByIdSocialMediaQuery, GetByIdSocialMediaDto>
        {
            private readonly ISocialMediaRepository _socialMediaRepository;
            private readonly IMapper _mapper;
            private readonly SocialMediaBusinessRules _businessRules;

            public GetByIdSocialMediaQueryHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper, SocialMediaBusinessRules businessRules)
            {
                _socialMediaRepository = socialMediaRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }

            public async Task<GetByIdSocialMediaDto> Handle(GetByIdSocialMediaQuery request, CancellationToken cancellationToken)
            {
                SocialMedia? socialMedia = await _socialMediaRepository.GetAsync(c => c.Id == request.Id, include: t => t.Include(t => t.Developer));
                _businessRules.RequestedSocialMediaCanNotBeNull(socialMedia);

                GetByIdSocialMediaDto getByIdSocialMediaDto = _mapper.Map<GetByIdSocialMediaDto>(socialMedia);
                return getByIdSocialMediaDto;
            }
        }
    }
}
