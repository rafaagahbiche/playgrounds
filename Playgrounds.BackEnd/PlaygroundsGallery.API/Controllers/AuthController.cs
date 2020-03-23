using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Auth.Services;

namespace PlaygroundsGallery.API.Controllers
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody]MemberToLoginDto memberToLoginDto)
        {
            if (memberToLoginDto != null)
            {
                var newMember = await _memberManager.Register(memberToLoginDto);
                if (newMember != null)
                {
                    return CreatedAtRoute("GetMember", new { controller = "Member",  id = newMember?.Id}, newMember);
                }

                return BadRequest("Login or email address already exist.");

            }
            else 
            {
                return BadRequest("Empty member object");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]MemberToLoginDto memberToLoginDto)
        {
            if (memberToLoginDto != null)
            {
                var loggedInMember = await _memberManager.Login(memberToLoginDto);
                if (loggedInMember != null)
                {
                    return Ok(await _memberManager.Login(memberToLoginDto));
                }

                return BadRequest("Wrong Login/Password");
            }
            else
            {
                return BadRequest("Empty member object");
            }
        }
    }
}