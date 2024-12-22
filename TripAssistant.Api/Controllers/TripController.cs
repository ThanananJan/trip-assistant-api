using JWTAuthentication.Library.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripAssistant.Library.Model.Dto.Trip;
using TripAssistant.Library.Models.Dto;
using TripAssistant.Library.Services;

namespace TripAssistant.Api.Controllers
{
    [Authorize]
    [Route("api/trips")]
    public class TripController([FromServices] ITripService service) : ControllerBase
    {
        [HttpGet]
    public IActionResult GetTrips([FromServices] IJwtAuthenticationHelper helper)
    {
        var userInfo = helper.GetUserInfoByClaimsIdentity();
        if (userInfo == null) return Unauthorized();
        return Ok(service.GetTrips(userInfo.IdUser));
    }

    [HttpGet("{idTrip}")]
    public IActionResult GetTripById(int idTrip)
    {
        return Ok(service.GetTripById(idTrip));
    }
    [HttpGet("{idTrip}/summary")]
    public IActionResult GetTripSummary(int idTrip)
    {
        return Ok(service.GetTripSummary(idTrip));
    }
    [HttpGet("{idTrip}/transactions")]
    public IActionResult GetTripTransactions(int idTrip)
    {
        return Ok(service.GetTripTransactions(idTrip));
    }
    [HttpPost]
    public IActionResult CreateTrip([FromBody] TripCreateRequestDto request, [FromServices] IJwtAuthenticationHelper helper)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userInfo = helper.GetUserInfoByClaimsIdentity();
        if (userInfo == null) { return Unauthorized(); }
        try
        {
            return Ok(service.AddTrip(request, userInfo.IdUser));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }

    }
    [HttpPut]
    public IActionResult UpdateTrip([FromBody] TripUpdateRequestDto request, [FromServices] IJwtAuthenticationHelper helper)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userInfo = helper.GetUserInfoByClaimsIdentity();
        if (userInfo == null) { return Unauthorized(); }
        try
        {
            return Ok(service.UpdateTrip(request, userInfo.IdUser));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
    [HttpDelete("{idTrip}")]
    public IActionResult DeleteTrip(int idTrip)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(service.DeleteTrip(idTrip));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
    [HttpPost("{idTrip}/share")]
    public IActionResult AssignTrip(int idTrip, [FromServices] IJwtAuthenticationHelper helper)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userInfo = helper.GetUserInfoByClaimsIdentity();
        if (userInfo == null) return Unauthorized();
        try
        {
            return Ok(service.AssignTrip(idTrip, userInfo.IdUser));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
}
}
