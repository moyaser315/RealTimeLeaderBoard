using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IAuthenicationService
    {
        (string Hash, string salt) HashPassword(string password);
        bool VerifyPassword(string password, string hash, string salt);
        string GenerateJwtToken(int userId, string username);

    }
}