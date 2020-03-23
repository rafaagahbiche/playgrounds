using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;
using Photos.Services.DTOs;
using Photos.Services.Managers;
using Photos.Services.ThirdPartyManagers;

namespace Photo.Api.Controllers
{
    [Route("api/photos/")]
    [ApiController]
    public class PhotosMemberController: ControllerBase
    {
        private readonly IPhotoManager _photoManager;
        private readonly IPhotoMember _photoMember;
        // private readonly IMemberManager _memberManager;
        private readonly IThirdPartyStorageManager _cloudinaryManager;

        public PhotosMemberController(
            IPhotoManager photoManager, 
            IThirdPartyStorageManager cloudinaryManager)
        {
            this._photoManager = photoManager;
            this._cloudinaryManager = cloudinaryManager; 
        }
        
        [Authorize]
        [Route("photos")]
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

        [Authorize]
        [Route("photos/upload")]
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

        [Authorize]
        [Route("markPhotoAsDeleted/{publicId}")]
        [HttpPut]
        public async Task<IActionResult> DeletePhoto(int publicId)
        {
            var succeeded = await _photoMember.DeletePhoto(publicId);
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


        [Authorize]
        [Route("photos/update")]
        [HttpPut]
        public async Task<IActionResult> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto)
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
    }
}