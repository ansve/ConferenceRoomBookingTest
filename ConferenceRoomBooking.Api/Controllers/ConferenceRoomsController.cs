using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/conference-rooms")]
public class ConferenceRoomsController : ControllerBase
{
    private readonly IConferenceRoomService _conferenceRoomService;

    public ConferenceRoomsController(IConferenceRoomService conferenceRoomService)
    {
        _conferenceRoomService = conferenceRoomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlls()
    {
        var rooms = await _conferenceRoomService.GetAllAsync();

        return Ok(rooms);
    }

    [HttpGet("available")]
    public async Task<IActionResult> SearchAvailable([FromQuery] SearchAvailableRoomsRequest request)
    {
        var rooms = await _conferenceRoomService.SearchAvailableAsync(request);

        return Ok(rooms);
    }

    [HttpGet ("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _conferenceRoomService.GetByIdAsync(id);

        if(room is null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateConferenceRoomRequest request)
    {
        var roomId = await _conferenceRoomService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = roomId },
            new { id = roomId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateConferenceRoomRequest request)
    {
        await _conferenceRoomService.UpdateAsync(id, request);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _conferenceRoomService.DeleteAsync(id);

        return NoContent();
    }
}
