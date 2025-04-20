using CustomAuth.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
                return Unauthorized("Not logged in.. you need to login..");
            var user = HttpContext.User.Identity?.Name;
            return Ok($"Hello {user}, this is a secure endpoint!, and You are Authorized to View This Resource..");
        }

        [HttpGet("about")]
        public IActionResult About()
        {           
            return Ok($"Hello {User.Identity!.Name},Welcome To About Us..");
        }

     
    }
}
