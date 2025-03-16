using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace medicurebackend.Controllers
{
    // Only Admins can access this controller
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // Admin-specific methods

        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            // Logic to return all users or manage the system
            return Ok(new { Message = "List of users (Admin access)" });
        }

        [HttpPost("create-doctor")]
        public IActionResult CreateDoctor([FromBody] Doctor doctor)
        {
            // Logic for creating a new doctor (only Admin can create)
            return Ok(new { Message = "Doctor created successfully!" });
        }
    }
}
