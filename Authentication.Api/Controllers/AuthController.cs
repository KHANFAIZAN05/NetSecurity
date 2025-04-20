using Authentication.Api.Data;
using Authentication.Api.Helpers;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = Authentication.Api.Models.RegisterRequest;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        public AuthController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            if (_db.Users.Any(u => u.UserName == request.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.Username,
                Role = request.Role
            };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request, [FromServices] IConfiguration config)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
            if (result != PasswordVerificationResult.Success) return Unauthorized();

            var accessToken = JwtTokenGenerator.GenerateToken(user, config);
            var refreshToken = JwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _db.SaveChanges();

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });

        }


        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshTokenRequest request, [FromServices] IConfiguration config)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = JwtTokenGenerator.GenerateToken(user, config);
            var newRefreshToken = JwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _db.SaveChanges();

            return Ok(new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

    }
}
