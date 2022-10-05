using Application.Features.SocialMedias.Dtos;
using Application.Features.SocialMedias.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Commands.CreateSocialMedia
{
    public class CreateSocialMediaCommand: IRequest<CreatedSocialMediaDto>
    {
        public int DeveloperId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public class CreateSocialMediaCommandHandler : IRequestHandler<CreateSocialMediaCommand, CreatedSocialMediaDto>
        {

            private readonly ISocialMediaRepository _socialMediaRepository;
            private readonly IMapper _mapper;
            private readonly SocialMediaBusinessRules _businessRules;

            public CreateSocialMediaCommandHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper, SocialMediaBusinessRules businessRules)
            {
                _socialMediaRepository = socialMediaRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }

            public async Task<CreatedSocialMediaDto> Handle(CreateSocialMediaCommand request, CancellationToken cancellationToken)
            {
                SocialMedia socialMedia = _mapper.Map<SocialMedia>(request);
                SocialMedia addedSocialMedia = await _socialMediaRepository.AddAsync(socialMedia);
                CreatedSocialMediaDto createdSocialMediaDto = _mapper.Map<CreatedSocialMediaDto>(addedSocialMedia);

                return createdSocialMediaDto;
            }
        }
    }
}
