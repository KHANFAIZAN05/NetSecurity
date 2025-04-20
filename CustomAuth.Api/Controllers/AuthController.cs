using CustomAuth.Api.Models;
using CustomAuth.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomAuth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserStore _userStore;

        public AuthController(UserStore userStore)
        {
            _userStore = userStore;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = _userStore.ValidateUser(request.UserName, request.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            return Ok($"Welcome {user.UserName}, you are logged in.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok("Logged out!");
        }

        [HttpGet("unauthorized")]
        public IActionResult UnauthorizedAccess()
        {
            return Unauthorized("You must log in first.");
        }

        [HttpGet("denied")]
        public IActionResult AccessDenied()
        {
            return Forbid("Access Denied!");
        }
    }
}
