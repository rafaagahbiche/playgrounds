using PlaygroundsGallery.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IFrontManager
    {
        Task<PhotoToReturnDto> UploadPhoto(PhotoForCreationDto photoForCreationDto);
        Task<bool> DeletePhoto(string publicId, bool physically);
        Task<PhotoToReturnDto> GetPhoto(int id);
        Task<IEnumerable<PhotoToReturnDto>> GetPhotosByMemberId(int id);
        Task<IEnumerable<PhotoToReturnDto>> GetRecentPhotos(int count);
        Task<string> Login(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> Register(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> GetMember(int id);
    }
}
