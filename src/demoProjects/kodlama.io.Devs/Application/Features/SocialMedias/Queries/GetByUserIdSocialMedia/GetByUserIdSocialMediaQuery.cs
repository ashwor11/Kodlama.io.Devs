using Application.Features.SocialMedias.Dtos;
using Application.Features.SocialMedias.Models;
using Application.Features.SocialMedias.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Queries.GetByUserIdSocialMedia
{
    public class GetByUserIdSocialMediaQuery:IRequest<SocialMediaByUserListModel>
    {
        public int DeveloperId { get; set; }
        public PageRequest PageRequest { get; set; }

        public class GetByUserIdSocialMediaQueryHandler : IRequestHandler<GetByUserIdSocialMediaQuery, SocialMediaByUserListModel>
        {
            private readonly ISocialMediaRepository _socialMediaRepository;
            private readonly IMapper _mapper;
            private readonly SocialMediaBusinessRules _businessRules;

            public GetByUserIdSocialMediaQueryHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper, SocialMediaBusinessRules businessRules)
            {
                _socialMediaRepository = socialMediaRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }
            public async Task<SocialMediaByUserListModel> Handle(GetByUserIdSocialMediaQuery request, CancellationToken cancellationToken)
            {
                IPaginate<SocialMedia> result = await _socialMediaRepository.GetListAsync(x => x.DeveloperId == request.DeveloperId, include: x => x.Include(x => x.Developer), size: request.PageRequest.PageSize, index: request.PageRequest.Page);
                
                SocialMediaByUserListModel model = _mapper.Map<SocialMediaByUserListModel>(result);
                return model;
            }
        }

    }
}
