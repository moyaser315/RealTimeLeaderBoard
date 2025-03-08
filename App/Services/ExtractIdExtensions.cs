using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Services
{
    public static class ExtractIdExtensions
    {
         public static int? GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User.Identity is not { IsAuthenticated: true })
        {
            return null; // User is not authenticated
        }

        var uidClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (uidClaim is null || !int.TryParse(uidClaim.Value, out var id))
        {
            return null; // Invalid token
        }

        return id;
    }
    }
}