using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace App.Services
{
    public class AuthenicationService : IAuthenicationService
    {
        private readonly string _jwtSecret;
        private readonly int _jwtExpirationHours;

        public AuthenicationService(IConfiguration config)
        {
            _jwtSecret = config["JwtSettings:SignKey"] ?? throw new ArgumentNullException("JWT secret is missing in config");
            _jwtExpirationHours = int.TryParse(config["JwtSettings:ExpirationHours"], out var hours) ? hours : 12;
        }
        public string GenerateJwtToken(int userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: "Leaderboard",
                audience: "Users",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtExpirationHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}