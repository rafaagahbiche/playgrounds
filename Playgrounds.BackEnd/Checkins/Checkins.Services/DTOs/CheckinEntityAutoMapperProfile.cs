using System.Linq;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Services
{
    public class CheckinEntityAutoMapperProfile : Profile
    {
        public CheckinEntityAutoMapperProfile()
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
    }
}