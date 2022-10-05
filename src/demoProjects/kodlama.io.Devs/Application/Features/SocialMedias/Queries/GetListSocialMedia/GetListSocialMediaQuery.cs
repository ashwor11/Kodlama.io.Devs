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

namespace Application.Features.SocialMedias.Queries.GetListSocialMedia
{
    public class GetListSocialMediaQuery: IRequest<SocialMediaListModel>
    {
        public PageRequest PageRequest{ get; set; }


        public class GetListSocialMediaQueryHandler : IRequestHandler<GetListSocialMediaQuery, SocialMediaListModel>
        {
            private readonly ISocialMediaRepository _socialMediaRepository;
            private readonly IMapper _mapper;
            private readonly SocialMediaBusinessRules _businessRules;

            public GetListSocialMediaQueryHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper, SocialMediaBusinessRules businessRules)
            {
                _socialMediaRepository = socialMediaRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }
            public async Task<SocialMediaListModel> Handle(GetListSocialMediaQuery request, CancellationToken cancellationToken)
            {
                IPaginate<SocialMedia> result = await _socialMediaRepository.GetListAsync(include: x => x.Include(x => x.Developer), size: request.PageRequest.PageSize, index: request.PageRequest.Page);

                SocialMediaListModel model = _mapper.Map<SocialMediaListModel>(result);
                return model;
            }
        }
    }
}
