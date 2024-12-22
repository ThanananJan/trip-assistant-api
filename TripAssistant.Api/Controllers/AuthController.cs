
using JWTAuthentication.Library.Handlers;
using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Model.Dto;
using JWTAuthentication.Library.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace TripAssistant.Api.Controllers
{
    [AWSAuthorization]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet("token")]
        public async Task<IActionResult> RequestToken([FromServices] JWTAuthentication.Library.Interfaces.IAuthenticationService service, [FromServices] AWSAuthenticationHelper helper)
        {
            var userInfo = await helper.GetUserInfo();
            if (userInfo == null) { return Unauthorized(); }
            try
            {
                return Ok(service.GenerateToken(userInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
            }

        }
        [HttpPost("token/refresh")]
        public IActionResult RequestToken([FromServices] JWTAuthentication.Library.Interfaces.IAuthenticationService service, [FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                return Ok(service.GenerateRefreshToken(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
            }
        }
    }
}
