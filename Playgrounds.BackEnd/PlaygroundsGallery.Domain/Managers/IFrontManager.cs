using PlaygroundsGallery.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IFrontManager
    {
        Task<PhotoDto> UploadPhoto(PhotoForCreationDto photoForCreationDto);
        Task<PhotoDto> UpdatePhoto(PhotoDto photoToUpdateDto);
        Task<bool> DeletePhoto(string publicId, bool physically);
        Task<PhotoDto> GetPhoto(int id);
        Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id);
        Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count);
        Task<string> Login(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> Register(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> GetMember(int id);
    }
}
