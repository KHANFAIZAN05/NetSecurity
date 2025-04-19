using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public() => Ok("This is open to everyone.");

        [Authorize]
        [HttpGet("authenticated")]
        public IActionResult Authenticated() =>
            Ok($"Hello {User.Identity?.Name}, you're authenticated!");

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly() =>
            Ok("You are an Admin and can access this route.");
    }

}