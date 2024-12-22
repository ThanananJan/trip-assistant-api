using JWTAuthentication.Library.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripAssistant.Library.Services;

namespace TripAssistant.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo([FromServices] JwtAuthenticationHelper helper)
        {
            var userInfo = helper.GetUserInfoByClaimsIdentity();
            return Ok(userInfo);

        }
    }
}
