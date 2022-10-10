using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Models;
using Application.Features.UserOperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Persistence.Paging;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Queries.GetListByUser
{
    public class GetListByUserUserOperationClaimQuery: IRequest<GetListByUserUserOperationClaimModel>, ISecuredRequest
    {
        public int UserId { get; set; }

        public string[] Roles => new string[] {"admin"};

        public class GetListByUserUserOperationClaimQueryHandler : IRequestHandler<GetListByUserUserOperationClaimQuery, GetListByUserUserOperationClaimModel>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimsBusinessRules;

            public GetListByUserUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper, UserOperationClaimBusinessRules userOperationClaimsBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimsBusinessRules = userOperationClaimsBusinessRules;
            }

            public async Task<GetListByUserUserOperationClaimModel> Handle(GetListByUserUserOperationClaimQuery request, CancellationToken cancellationToken)
            {
                IPaginate<UserOperationClaim> userOperationClaims = await _userOperationClaimRepository.GetListAsync(x => x.UserId == request.UserId, include: x=>x.Include(x=>x.OperationClaim).Include(x=>x.User));
                
                GetListByUserUserOperationClaimModel model = _mapper.Map<GetListByUserUserOperationClaimModel>(userOperationClaims);
                return model;

            }
        }
    }
}
