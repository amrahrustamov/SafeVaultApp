using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SafeVaultApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("get-secret-data")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSecretData()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return Forbid();
            }

            return Ok("This is protected admin data.");
        }
    }
}
