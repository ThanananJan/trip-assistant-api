using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripAssistant.Library.Model.Dto.Transaction;
using TripAssistant.Library.Models.Dto;
using TripAssistant.Library.Services;

namespace TripAssistant.Api.Controllers
{
    [Authorize]
    [Route("api/transactions")]
    public class TripTransactionController(ITripService service) : ControllerBase
    {
        [HttpGet("{idTripTransaction}")]
    public IActionResult CreateTransaction(int idTripTransaction)
    {

        return Ok(service.GetTripTransaction(idTripTransaction));

    }
    [HttpPost]
    public IActionResult CreateTransaction([FromBody] TripTransactionCreateRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(service.AddTripTransaction(request));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
    [HttpPut]
    public IActionResult UpdateTransaction([FromBody] TripTransactionUpdateRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(service.UpdateTripTransaction(request));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
    [HttpDelete("{idTripTransaction}")]
    public IActionResult DeleteTripTransaction(int idTripTransaction)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return Ok(service.DeleteTripTransaction(idTripTransaction));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDtoExtension.GetResponseFail(ex.Message));
        }
    }
}
}
