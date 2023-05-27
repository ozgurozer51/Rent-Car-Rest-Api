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

        public bool ValidateCredentials(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.e_mail == email && u.password == password);

            if (user != null)
            {
                // Token süresini kontrol et
                var tokenExpired = IsTokenExpired(user.token);
                if (tokenExpired)
                {
                    // Token süresi dolmuş, gerekli işlemleri gerçekleştir
                    // Örneğin, yeni bir token almak için kullanıcıyı yönlendir
                    // veya oturumu sonlandırabilirsiniz.
                }

                var jwtToken = GenerateJwtToken(email);

                // Tokeni users tablosundaki token sütununa kaydet
                user.token = jwtToken;
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        private bool IsTokenExpired(string? token)
        {

            if (token == null)
            {
                // Token null ise, süresi dolmuş kabul edilir
                return true;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var expirationDate = jwtToken.ValidTo;
            var currentDate = DateTime.UtcNow;

            return currentDate > expirationDate;
        }

        public string GenerateJwtToken(string e_mail)
        {

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryInMinutes = Convert.ToInt32(_configuration["JwtSettings:ExpiryInMinutes"]);

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
