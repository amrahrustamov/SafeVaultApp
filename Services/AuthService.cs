using Microsoft.IdentityModel.Tokens;
using SafeVaultApp.DbModels;
using SafeVaultApp.DbModels.Models;
using SafeVaultApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SafeVaultApp.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public User GetUser(string username, string password)
        {
            return new User { Username = username, Password = password, Role = "Admin" };
            //return _context.Users
            //    .FirstOrDefault(u => u.Username == username && u.Password == password);
        }
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public Response Authenticate(string password, string username)
        {
            var hashedPassword = HashPassword(password);

            //var user = _context.Users
            //    .FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (username == "admin" && password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var jwtKey = _config["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("JWT Key is missing in configuration.");
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                var userInfo = GetUser(username, password);
                Response response = new Response
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = $"Username: {userInfo.Username}, Role: {userInfo.Role}"
                };
                return response;
            }
            return new Response { };
        }
    }

}
