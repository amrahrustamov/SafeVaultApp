using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeVaultApp.DbModels.Models;
using SafeVaultApp.Models;
using SafeVaultApp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeVaultApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public AuthController(IConfiguration config, AuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate user (replace with real validation)
            var response =  _authService.Authenticate(login.Password,login.Username);

            return Ok(response);
        }
    }
}
