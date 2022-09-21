using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Rules;
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

namespace Application.Features.Technologies.Queries.GetByIdTechnology
{
    public class GetByIdTechnologyQuery: IRequest<GetByIdTechnologyDto>
    {
        public int Id { get; set; }

        public class GetByIdTechnologyQueryHandler : IRequestHandler<GetByIdTechnologyQuery, GetByIdTechnologyDto>
        {
            private readonly ITechnologyRepository _technologyRepository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _businessRules;

            public GetByIdTechnologyQueryHandler(ITechnologyRepository technologyRepository, IMapper mapper, TechnologyBusinessRules businessRules)
            {
                _technologyRepository = technologyRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }

            public async Task<GetByIdTechnologyDto> Handle(GetByIdTechnologyQuery request, CancellationToken cancellationToken)
            {
                Technology technology = await _technologyRepository.GetAsync(t=> t.Id == request.Id, include : t=> t.Include(t=>t.ProgrammingLanguage));
                _businessRules.ProgrammingLanguageTechnologyShouldExistWhenRequested(technology);

                GetByIdTechnologyDto response = _mapper.Map<GetByIdTechnologyDto>(technology);
                return response;

            }
        }
    }
}
