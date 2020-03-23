using System.Threading.Tasks;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/member/")]
    [ApiController]
    public class MemberController: ControllerBase
    {
        private readonly IMemberManager _memberManager;

        public MemberController(IMemberManager memberManager)
        {
            _memberManager = memberManager;
        }

        [HttpGet("{id}", Name = "GetMember")]
        public async Task<IActionResult> GetMember(int memberId)
        {
            return Ok(await _memberManager.GetMember(memberId));
        }
    }
}