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
                    var uploadedPhoto = await _photoManager.AddPhoto(photoToInsert);
                    return StatusCode(201, uploadedPhoto);
                }

                return StatusCode(500);
            }
            
            return BadRequest();
        }

        [Authorize]
        [Route("markPhotoAsDeleted/{publicId}")]
        [HttpPut]
        public async Task<IActionResult> DeletePhoto(string publicId)
        {
            var succeeded = await _photoManager.DeletePhoto(publicId);
            if (succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [Route("deletephoto/{publicId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePhotoPhysically(string publicId)
        {
            var succeededDb = await _photoManager.DeletePhotoPhysically(publicId);
            var succeededCl = _cloudinaryManager.DeletePhoto(publicId);
            if (succeededDb && succeededCl)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
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
                return Ok(await _photoManager.UpdatePhoto(photoToUpdateDto));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}