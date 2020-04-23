using AutoMapper;
using System.Linq;
using PlaygroundsGallery.DataEF.Models;
namespace Playgrounds.Services
{
    public class PlaygroundEntityAutoMapperProfile : Profile
    {
        public PlaygroundEntityAutoMapperProfile()
        {
            CreateMap<Playground, PlaygroundDto>()
                .ForMember(p => p.LocationStr, o => o.MapFrom(p => $"{p.Location.City}, {p.Location.Country}"))
                .ForMember(p => p.MainPhotoUrl, o => o.MapFrom(p => p.Photos.FirstOrDefault().Url))
                .ReverseMap();

             CreateMap<Location, LocationDto>();

             CreateMap<Location, LocationDto>();
        }
    }
}