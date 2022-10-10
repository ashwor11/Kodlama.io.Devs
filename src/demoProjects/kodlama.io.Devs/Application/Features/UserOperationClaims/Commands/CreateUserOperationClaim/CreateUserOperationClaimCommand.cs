using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim
{
    public class GetListByUserUserOperationClaimCommand: IRequest<CreatedUserOperationClaimDto> 
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }

        public class CreateUserOperationClaimCommandHandler : IRequestHandler<GetListByUserUserOperationClaimCommand, CreatedUserOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public CreateUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper, UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<CreatedUserOperationClaimDto> Handle(GetListByUserUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                UserOperationClaim userOperationClaim = new()
                {
                    OperationClaimId = request.OperationClaimId,
                    UserId = request.UserId
                };

                _userOperationClaimBusinessRules.UserOperationClaimMustNotBeExistWhenCreated(userOperationClaim);

                UserOperationClaim addedUserOperationClaim = await _userOperationClaimRepository.AddAsync(userOperationClaim);

                UserOperationClaim? newUserOperationClaim = await _userOperationClaimRepository.GetAsync(x=> x == addedUserOperationClaim,include: x=>x.Include(x=>x.User).Include(x=>x.OperationClaim));

                return _mapper.Map<CreatedUserOperationClaimDto>(newUserOperationClaim);



            }
        }
    }
}
