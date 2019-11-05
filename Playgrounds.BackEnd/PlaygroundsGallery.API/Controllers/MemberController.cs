using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Managers;
using PlaygroundsGallery.API.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PlaygroundsGallery.Helper.Exceptions;
using System;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/member/")]
    [ApiController]
    public class MemberController: ControllerBase
    {
        private readonly IFrontManager _frontManager;

        public MemberController(IFrontManager frontManager)
        {
            _frontManager = frontManager;
        }

        [HttpGet("{id}", Name = "GetMember")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _frontManager.GetMember(id);
            return Ok(member);
        }

        [Authorize]
        [Route("photos")]
        // [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetMemberPhotos()
        {
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memberId = int.Parse(memberIdStr);
            var photos = await _frontManager.GetPhotosByMemberId(memberId);
            return Ok(photos);
        }

        [Authorize]
        [Route("photos/upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]PhotoForCreationDto photo)
        {
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            photo.MemberId = int.Parse(memberIdStr);
            try
            {
                var photoToReturnDto = await _frontManager.UploadPhoto(photo);
                return CreatedAtRoute("GetPhoto", new {controller ="Photo", id = photoToReturnDto.Id}, photoToReturnDto);
            }
            catch (PhotoUploadFileEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PhotoUploadToLibraryException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500); 
            }
        }

        [Authorize]
        [Route("photos/{publicId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePhoto(string publicId)
        {
            try
            {
                var succeeded = await _frontManager.DeletePhoto(publicId, false);
                if (succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (PhotoNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}