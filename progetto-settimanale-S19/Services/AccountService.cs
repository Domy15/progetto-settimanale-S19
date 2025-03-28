using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using progetto_settimanale_S19.Models;
using progetto_settimanale_S19.Settings;
using progetto_settimanale_S19.DTOs.Account;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace progetto_settimanale_S19.Services
{
    public class AccountService
    {
        private readonly Jwt _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;


        public AccountService(IOptions<Jwt> jwtOptions,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountService> logger)
        {
            _jwtSettings = jwtOptions.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<bool> Register(RegisterRequestDto registerRequest)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    return false;
                }

                var newUser = new ApplicationUser()
                {
                    Email = registerRequest.Email,
                    UserName = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName
                };

                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

                if (!result.Succeeded)
                {
                    return false;
                }

                await _userManager.AddToRoleAsync(newUser, "User");

                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<(bool, string)> Login(LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (user == null)
                {
                    return (false, "Invalid email or password");
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

                if (!result.Succeeded)
                {
                    return (false, "Invalid email or password");
                }

                var roles = await _signInManager.UserManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes);
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: expiry, signingCredentials: creds);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return (true, tokenString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (false, "An error occurred while processing the request");
            }
        }
    }
}
