using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Managers;
using PlaygroundsGallery.API.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/member/")]
    [ApiController]
    public class MemberController: ControllerBase
    {
        private readonly IFrontManager _frontManager;
        private readonly IMemberManager _memberManager;
        private readonly IThirdPartyStorageManager _cloudinaryManager;

        public MemberController(
            IFrontManager frontManager, 
            IMemberManager memberManager,
            IThirdPartyStorageManager cloudinaryManager)
        {
            _frontManager = frontManager;
            _memberManager = memberManager;
            _cloudinaryManager = cloudinaryManager;
        }

        [HttpGet("{id}", Name = "GetMember")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _memberManager.GetMember(id);
            return Ok(member);
        }

        [Authorize]
        [Route("photos")]
        [HttpGet]
        public async Task<IActionResult> GetMemberPhotos()
        {
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memberId = int.Parse(memberIdStr);
            var photos = await _frontManager.GetPhotosByMemberId(memberId);
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
                    var uploadedPhoto = await _frontManager.AddPhoto(photoToInsert);
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
            var succeeded = await _frontManager.DeletePhoto(publicId);
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
            var succeededDb = await _frontManager.DeletePhotoPhysically(publicId);
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
                return Ok(await _frontManager.UpdatePhoto(photoToUpdateDto));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}