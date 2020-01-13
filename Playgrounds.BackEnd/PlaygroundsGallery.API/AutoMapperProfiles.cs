using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using System;
using System.Linq;

namespace PlaygroundsGallery.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreatePhotoMap();
            CreateMemberMap();
            CreatePlaygroundMap();
            CreateCheckinMap();
            CreatePostMap();
            CreateMap<Location, LocationDto>().ReverseMap();
        }

        private void CreatePhotoMap()
        {
            CreateMap<Photo, PhotoDto>()
                .ForMember(p => p.PlaygroundLocationStr, o => o.MapFrom(p => $"{p.Playground.Location.City}, {p.Playground.Location.Country}"))
                .ReverseMap();

            CreateMap<Photo, PhotoAsPostDto>()
                .ForMember(p => p.AuthorProfilePictureUrl, 
                    o => o.MapFrom(
                        p => p.Member.ProfilePictures.FirstOrDefault(x => x.Main).Url))
                .ForMember(p => p.AuthorLoginName, 
                    o => o.MapFrom(p => p.Member.LoginName))
                .ReverseMap();
            CreateMap<Photo, PhotoToUpdateDto>().ReverseMap();
            CreateMap<PhotoToInsertDto, Photo>();
            CreateMap<PhotoToUploadDto, PhotoToInsertDto>();
            CreateMap<Photo, PhotoInsertedDto>();
        }
        private void CreateMemberMap()
        {
            CreateMap<Member, MemberLoggedInDto>()
                .ForMember(m => m.ProfilePictureUrl, o => o.MapFrom(m => m.ProfilePictures.FirstOrDefault(x => x.Main).Url));
            CreateMap<Member, MemberToLoginDto>().ReverseMap();
            CreateMap<MemberToLoginDto, MemberEntityDto>();
            CreateMap<MemberEntityDto, Member>().ReverseMap();
            CreateMap<MemberEntityDto, MemberLoggedInDto>();
            CreateMap<Member, MemberDto>().ReverseMap();
        }
        private void CreatePlaygroundMap()
        {
            CreateMap<Playground, PlaygroundDto>()
                .ForMember(p => p.LocationStr, o => o.MapFrom(p => $"{p.Location.City}, {p.Location.Country}"))
                .ForMember(p => p.MainPhotoUrl, o => o.MapFrom(p => p.Photos.FirstOrDefault().Url))
                .ReverseMap();
        }

        private void CreateCheckinMap()
        {
            CreateMap<CheckIn, CheckInForCreationDto>().ReverseMap();
            CreateMap<CheckIn, CheckinDto>()
                .ForMember(ch => ch.MemberProfilePictureUrl, o => o.MapFrom(m => m.Member.ProfilePictures.FirstOrDefault(x => x.Main).Url))
                .ReverseMap();
            
            CreateMap<CheckIn, CheckinAsPostDto>()
                .ForMember(p => p.AuthorProfilePictureUrl, 
                    o => o.MapFrom(c => c.Member.ProfilePictures.FirstOrDefault(x => x.Main).Url))
                .ForMember(c => c.AuthorLoginName, 
                    o => o.MapFrom(c => c.Member.LoginName))
                .ReverseMap();
        }

        private void CreatePostMap()
        {
            CreateMap<PhotoAsPostDto, PostDto>()
                .ForMember(p => p.Url, 
                    o => o.MapFrom(ph => ph.Url == null ? string.Empty : ph.Url));
            CreateMap<CheckinAsPostDto, PostDto>();
        }
    }
}