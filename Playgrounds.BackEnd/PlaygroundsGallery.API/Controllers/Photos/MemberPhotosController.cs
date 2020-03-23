using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;
using Photos.Services.DTOs;
using Photos.Services.Managers;
using Photos.Services.ThirdPartyManagers;

namespace PlaygroundsGallery.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/member-photos/")]
    public class MemberPhotosController: ControllerBase
    {
        private readonly IPhotoMember _photoMember;
        private readonly IPhotoManager _photoManager;
        private readonly IThirdPartyStorageManager _cloudinaryManager;

        public MemberPhotosController(
            IPhotoManager photoManager, 
            IPhotoMember photoMember, 
            IThirdPartyStorageManager cloudinaryManager)
        {
            _photoMember = photoMember;
            _photoManager = photoManager;
            _cloudinaryManager = cloudinaryManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberPhotos()
        {
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memberId = int.Parse(memberIdStr);
            var photos = await _photoManager.GetPhotosByMemberId(memberId);
            if (photos != null && photos.Any())
            {
                return Ok(photos);
            }
            else
            {
                return NoContent();
            }
        }

        [AllowAnonymous]
        [Route("members/{memberId}")]
        [HttpGet]
        public async Task<IActionResult> GetMemberPhotos(int memberId)
        {
            var photos = await _photoManager.GetPhotosByMemberId(memberId);
            if (photos != null && photos.Any())
            {
                return Ok(photos);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]PhotoToUploadDto photo)
        {
            if (photo != null)
            {
                var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                photo.MemberId = int.Parse(memberIdStr);
                var photoToInsert = this._cloudinaryManager.UploadPhoto(photo);
                if (photoToInsert != null)
                {
                    var uploadedPhoto = await _photoMember.AddPhoto(photoToInsert);
                    return StatusCode(201, uploadedPhoto);
                }

                return StatusCode(500);
            }
            
            return BadRequest();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdatePhoto([FromBody]PhotoToUpdateDto photoToUpdateDto)
        {
            if (photoToUpdateDto != null)
            {
                var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                photoToUpdateDto.MemberId = int.Parse(memberIdStr);
                return Ok(await _photoMember.UpdatePhoto(photoToUpdateDto));
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("mark-as-deleted/{photoId}")]
        [HttpPut]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var succeeded = await _photoMember.DeletePhoto(photoId);
            if (succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("{photoId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePhotoPhysically(int photoId)
        {
            var photoPublicId = await _photoMember.DeletePhotoPhysically(photoId);
            if (!string.IsNullOrEmpty(photoPublicId))
            {
                var succeededCl = _cloudinaryManager.DeletePhoto(photoPublicId);
                if (succeededCl)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            
            return NotFound(); 
        }
    }
}