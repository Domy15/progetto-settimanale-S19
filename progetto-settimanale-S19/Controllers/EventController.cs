using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using progetto_settimanale_S19.DTOs.Artist;
using progetto_settimanale_S19.DTOs.Event;
using progetto_settimanale_S19.Services;

namespace progetto_settimanale_S19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService) 
        { 
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequestDto createEventDto)
        {
            try
            {
                var result = await _eventService.CreateEventAsync(createEventDto);

                if(!result)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "Event successfully created" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();

                if (events == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                int count = events.Events.Count();
                string countstring = count == 1 ? $"{count} event found" : $"{count} events found";

                return Ok(new { message = countstring, AllEvents = events.Events });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var Event = await _eventService.GetEventByIdAsync(id);

                if (Event == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "event found", Event });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent([FromQuery] int id, [FromBody] UpdateEventRequestDto updateEvent)
        {
            try
            {
                var result = await _eventService.UpdateEventAsync(id, updateEvent);

                return result ? Ok(new { Message = "Event updated" }) : BadRequest(new { Message = "Something went wrong" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent([FromQuery] int id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);

                return result ? Ok(new { Message = "Event deleted" }) : BadRequest(new { Message = "Something went wrong" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }
    }
}
