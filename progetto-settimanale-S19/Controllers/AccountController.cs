using Microsoft.AspNetCore.Mvc;
using progetto_settimanale_S19.DTOs.Account;
using progetto_settimanale_S19.Services;


namespace progetto_settimanale_S19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                var success = await _accountService.Register(registerRequest);
                if (success)
                {
                    return Ok(new { message = "Account successfully registered!" });
                }
                else
                {
                    return BadRequest(new { message = "Email is already registered or something went wrong." });
                }
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var (success, result) = await _accountService.Login(loginRequest);

                if (success)
                {
                    return Ok(new { token = result });
                }
                else
                {
                    return Unauthorized(new { message = result });
                }
            }
            catch
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }
    }
}
