using System.Linq;
using AutoMapper;
using models = PlaygroundsGallery.DataEF.Models;

namespace Photos.Services.DTOs
{
    public class PhotoEntityAutoMapperProfile : Profile
    {
        public PhotoEntityAutoMapperProfile()
        {
            CreateMap<models.Photo, PhotoDto>()
                .ForMember(p => p.PlaygroundLocationStr, o => o.MapFrom(p => $"{p.Playground.Location.City}, {p.Playground.Location.Country}"))
                .ReverseMap();

            CreateMap<models.Photo, PhotoAsPostDto>()
                .ForMember(p => p.AuthorProfilePictureUrl, 
                    o => o.MapFrom(
                        p => p.Member.ProfilePictures.FirstOrDefault(x => x.Main).Url))
                .ForMember(p => p.AuthorLoginName, 
                    o => o.MapFrom(p => p.Member.LoginName))
                .ReverseMap();
            CreateMap<models.Photo, PhotoToUpdateDto>().ReverseMap();
            CreateMap<PhotoToInsertDto, models.Photo>();
            CreateMap<PhotoToUploadDto, PhotoToInsertDto>();
            CreateMap<models.Photo, PhotoInsertedDto>();

            // CreateMap<PhotoAsPostDto, PostDto>()
            //     .ForMember(p => p.Url, 
            //         o => o.MapFrom(ph => ph.Url == null ? string.Empty : ph.Url));
            // CreateMap<CheckinAsPostDto, PostDto>();
        }
    }
}