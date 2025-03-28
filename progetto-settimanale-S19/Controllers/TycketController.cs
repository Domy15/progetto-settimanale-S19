using System.Net.Sockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using progetto_settimanale_S19.Data;
using progetto_settimanale_S19.DTOs.Tycket;
using progetto_settimanale_S19.Models;

namespace progetto_settimanale_S19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TycketController : ControllerBase
    {
        private readonly TycketService _tycketService;

        public TycketController(TycketService tycketService, UserManager<ApplicationUser> userManager)
        {
            _tycketService = tycketService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTycket([FromBody] CreateTycketDto createTycketDto)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                var userEmail = user.Value;

                var result = await _tycketService.CreateTycketAsync(createTycketDto, userEmail);

                if (!result)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "Tycket successfully purchased" });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTyckets()
        {
            try
            {
                var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (user == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                var userEmail = user.Value;

                var tyckets = await _tycketService.GetAllTycketsAsync(userEmail);

                if (tyckets == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                int count = tyckets.tyckets.Count();
                string countstring = count == 1 ? $"{count} event found" : $"{count} events found";

                return Ok(new { message = countstring, Tyckets = tyckets.tyckets });
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetTycketById(int id)
        {
            try
            {
                var tycket = await _tycketService.GetTycketByIdAsync(id);

                if (tycket == null)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "tycket found", Tycket = tycket });
            }
            catch
            {
                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTycket([FromQuery] int id)
        {
            try
            {
                var result = await _tycketService.DeleteTycketAsync(id);

                if (!result)
                {
                    return BadRequest(new { message = "Something went wrong" });
                }

                return Ok(new { message = "tycket successfully deleted" });
            }
            catch
            {
                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }
    }
}
