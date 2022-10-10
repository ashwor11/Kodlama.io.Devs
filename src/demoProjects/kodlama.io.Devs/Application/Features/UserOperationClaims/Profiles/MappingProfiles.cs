using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Profiles
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserOperationClaim, CreatedUserOperationClaimDto>().ForMember(x => x.UserEmail, opt => opt.MapFrom(x => x.User.Email)).ForMember(x => x.OperationClaim, opt => opt.MapFrom(x => x.OperationClaim.Name));
            CreateMap<UserOperationClaim, DeletedUserOperationClaimDto>().ForMember(x => x.UserEmail, opt => opt.MapFrom(x => x.User.Email)).ForMember(x => x.OperationClaim, opt => opt.MapFrom(x => x.OperationClaim.Name));
            CreateMap<IPaginate<UserOperationClaim>, GetListByUserUserOperationClaimModel>().ForMember(x => x.UserEmail, opt => opt.MapFrom(x => x.Items.Select(x => x.User.Email).First())).
                ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Items.Select(x => x.UserId).First())).ForMember(x => x.Claims, opt => opt.MapFrom(x => x.Items));
            CreateMap<UserOperationClaim, GetListByUserUserOperationClaimDto>().ForMember(x=> x.OperationClaim, opt=> opt.MapFrom(x=>x.OperationClaim.Name));

        }
    }
}
