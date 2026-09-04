using ConferenceRoomBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers
{
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
    }
}
