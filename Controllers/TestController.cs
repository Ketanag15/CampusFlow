using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace CampusFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint. No JWT required.");
        }

        // Secured endpoint (JWT required)
        [Authorize]
        [HttpGet("private")]
        public IActionResult PrivateEndpoint()
        {
            return Ok("This is a private endpoint. You are authorized!");
        }

        // Secured endpoint with role (e.g., Admin)
        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok("This is an admin-only endpoint. You are authorized as Admin!");
        }
    }
}
