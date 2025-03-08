using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        public (string Hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            string saltedPassword = salt + password;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = new byte[sha256.HashSize / 8];
                SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword), hashBytes);
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            string saltedPassword = salt + password;

            byte[] hashBytes = new byte[32];
            SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword), hashBytes);

            string computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == hash;
        }
    }
}