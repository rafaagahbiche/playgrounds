using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auth.Services;

namespace PlaygroundsGallery.Identity.Api.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IMemberManager _memberManager;

        public AuthController(IMemberManager memberManager)
        {
            _memberManager = memberManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(MemberToLoginDto memberToLoginDto)
        {
            if (memberToLoginDto != null)
            {
                var newMember = await _memberManager.Register(memberToLoginDto);
                return CreatedAtRoute("GetMember", new { controller ="Member",  id = newMember?.Id}, newMember);
            }
            else 
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(MemberToLoginDto memberToLoginDto)
        {
            if (memberToLoginDto != null)
            {
                return Ok(await _memberManager.Login(memberToLoginDto));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}