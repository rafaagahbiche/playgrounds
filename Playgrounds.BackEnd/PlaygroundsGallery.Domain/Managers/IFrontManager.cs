using PlaygroundsGallery.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IFrontManager
    {
        Task<PhotoInsertedDto> UploadPhoto(PhotoToUploadDto photoToUploadDto);
        Task<PhotoDto> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto);
        Task<bool> DeletePhoto(string publicId, bool physically);
        Task<PhotoDto> GetPhoto(int id);
        Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id);
        Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count);
        Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> Register(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> GetMember(int id);
    }
}
