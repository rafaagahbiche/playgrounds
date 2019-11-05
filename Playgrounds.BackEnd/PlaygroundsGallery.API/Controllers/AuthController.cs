using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Managers;
using PlaygroundsGallery.Helper.Exceptions;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/auth/")]
    [ApiController]

    public class AuthController: ControllerBase
    {
        private readonly IFrontManager _frontManager;

        public AuthController(IFrontManager frontManager)
        {
            _frontManager = frontManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(MemberToLoginDto memberToLoginDto)
        {
            try 
            {
                var memberToLogin = await _frontManager.Register(memberToLoginDto);
                if (memberToLogin != null)
                {
                    return CreatedAtRoute("GetMember", new { controller ="Member",  id = memberToLogin.Id}, memberToLogin);
                }
                else 
                {
                    return BadRequest();
                }            
            }
            catch(MemberCreationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(MemberToLoginDto memberToLoginDto)
        {
            try
            {
                var token = await _frontManager.Login(memberToLoginDto);
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(new {
                        token = token
                    });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (MemberLoginException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}