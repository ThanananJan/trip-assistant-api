using JWTAuthentication.Library.Model.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TripAssistant.Library.Model.DB;

namespace TripAssistant.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[Controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus([FromServices] JwtAuthDbContext db, [FromServices] IConfiguration config)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            return Ok(new
            {
                db = db.Database.CanConnect(),
                dtmLocal = DateTime.Now,
                version = assembly.GetName().Version,
                lastModified = lastModified
            });

        }
    }
}
