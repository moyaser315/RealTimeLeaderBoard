using System.Security.Claims;
using App.Database;
using App.Dtos;
using App.Filters;
using App.Mapping;
using App.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints
{
    public static class UserEndpoint
    {
        public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("user");

            group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
            {
                var user = await context.Users.FindAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user.ToUserDto());
            });

            group.MapGet("/scores", async (HttpContext httpContext, [FromQuery(Name = "sort")] int order, ApplicationDbContext context) =>
            {
                if (httpContext.User.Identity is not { IsAuthenticated: true })
                {
                    return Results.Unauthorized();
                }
                var uidClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (uidClaim is null || !int.TryParse(uidClaim.Value, out var id))
                {
                    return Results.BadRequest("Invalid token.");
                }
                var scoreList = await context.Scores
                .Where(s => s.UserId == id)
                .Include(s => s.Game)
                .Select(s => s.ToScoreDto())
                .ToListAsync();
                if (scoreList.Count == 0)
                {
                    Results.NotFound("User hasn't played yet");
                }

                scoreList = order == 1 ? [.. scoreList.OrderByDescending(s => s.Score)] : [.. scoreList.OrderByDescending(s => s.TimeStamp)];

                return Results.Ok(scoreList);
            }).RequireAuthorization();

            group.MapPost("/{uid}/{id}", async (int uid, int id, SubmitScoreDto score, ApplicationDbContext context) =>
            {
                var scoreModel = score.ToScoreModel(uid, id);
                scoreModel.User = context.Users.Find(uid);
                scoreModel.Game = context.Games.Find(id);
                context.Add(scoreModel);
                await context.SaveChangesAsync();
                return Results.Created();

            }).RequireAuthorization(); ;

            group.MapPost("/signup", async (CreateUserDto user, IAuthenicationService authService, ApplicationDbContext context) =>
            {
                var (hash, salt) = authService.HashPassword(user.Password);
                var newUser = user.ToUserModel(hash, salt);

                await context.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Results.Created();
            }).AddEndpointFilter<ValidationFilter<CreateUserDto>>();

            group.MapPost("/login", async (UserLoginDto user, IAuthenicationService authService, ApplicationDbContext context) =>
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser is null)
                {
                    return Results.NotFound("password or email maybe wrong");
                }
                if (authService.VerifyPassword(user.Password, existingUser.Password, existingUser.Salt))
                {
                    var token = authService.GenerateJwtToken(existingUser.Id, existingUser.UserName);
                    return Results.Ok(new { Token = token });
                }


                return Results.NotFound("password or email maybe wrong");
            });

            return group;
        }
    }
}