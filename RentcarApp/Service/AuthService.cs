using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RentcarApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace RentcarApp.Service
{
    public class AuthService
    {
        private readonly RentcarContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(RentcarContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.e_mail == username && u.password == password);
            return user != null;
        }

        public string GenerateJwtToken(string username)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
