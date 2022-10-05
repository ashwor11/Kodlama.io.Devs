using Application.Features.SocialMedias.Commands.CreateSocialMedia;
using Application.Features.SocialMedias.Dtos;
using Application.Features.SocialMedias.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Profiles
{
    public class Profiles:Profile
    {
        public Profiles()
        {
            CreateMap<CreateSocialMediaCommand, SocialMedia>().ReverseMap();
            CreateMap<SocialMedia, CreatedSocialMediaDto>().ForMember(c=>c.DeveloperEmail, opt => opt.MapFrom(c=>c.Developer.Email)).ReverseMap();
            CreateMap<SocialMedia, GetByIdSocialMediaDto>().ForMember(c => c.DeveloperEmail, opt => opt.MapFrom(c => c.Developer.Email)).ReverseMap();    
            CreateMap<SocialMedia, GetByUserIdSocialMediaDto>().ForMember(c => c.DeveloperEmail, opt => opt.MapFrom(c => c.Developer.Email)).ReverseMap(); 
            CreateMap<SocialMedia, GetListSocialMediaDto>().ForMember(c => c.DeveloperEmail, opt => opt.MapFrom(c => c.Developer.Email)).ReverseMap();
            CreateMap<IPaginate<SocialMedia>, SocialMediaByUserListModel>().ReverseMap();
            CreateMap<IPaginate<SocialMedia>, SocialMediaListModel>().ReverseMap();
        }
    }
}
