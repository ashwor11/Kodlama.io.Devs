using Application.Features.Users.Commands.Register;
using Application.Features.Users.Dtos;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Profiles
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<AccessToken, AccessTokenDto>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
        }
    }
}
