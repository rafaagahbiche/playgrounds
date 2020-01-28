using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using System;
using System.Linq;

namespace Auth.Services
{
    public class ManagerEntityAutoMapperProfile : Profile
    {
        public ManagerEntityAutoMapperProfile()
        {
            CreateMap<Member, MemberLoggedInDto>()
                .ForMember(m => m.ProfilePictureUrl, o => o.MapFrom(m => m.ProfilePictures.FirstOrDefault(x => x.Main).Url));
            CreateMap<Member, MemberToLoginDto>().ReverseMap();
            CreateMap<MemberToLoginDto, MemberEntityDto>();
            CreateMap<MemberEntityDto, Member>().ReverseMap();
            CreateMap<MemberEntityDto, MemberLoggedInDto>();
            CreateMap<Member, MemberDto>().ReverseMap();
        }
    }
}