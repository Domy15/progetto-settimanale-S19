using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using progetto_settimanale_S19.DTOs.Artist;
using progetto_settimanale_S19.Services;

namespace progetto_settimanale_S19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateArtist([FromBody] CreateArtistDto createArtistDto)
        {
            try
            {
                var result = await _artistService.CreateArtistAsync(createArtistDto);

                if (!result)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "Artist successfully created" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            try
            {
                var artistsList = await _artistService.GetAllArtists();

                if (artistsList == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                int count = artistsList.artists.Count();
                string countstring = count == 1 ? $"{count} artist found" : $"{count} artists found";

                return Ok(new { message = countstring, Artists = artistsList.artists });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(id);

                if (artist == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "Artist found", Artist = artist });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateArtist([FromQuery] int id, [FromBody] UpdateArtistRequestDto artist)
        {
            try
            {
                var result = await _artistService.UpdateArtistAsync(id, artist);

                return result ? Ok(new { Message = "Artist updated" }) : BadRequest(new { Message = "Something went wrong" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArtist([FromQuery] int id)
        {
            try
            {
                var result = await _artistService.DeleteArtistAsync(id);

                return result ? Ok(new { Message = "Artist deleted" }) : BadRequest(new { Message = "Something went wrong" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }
    }
}
