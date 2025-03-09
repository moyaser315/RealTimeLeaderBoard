using App.Database;
using App.Dtos;
using App.Filters;
using App.Mapping;
using App.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints
{
    public static class UserEndpoint
    {
        public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("user").RequireAuthorization().WithTags("Users");

            // Get user info
            group.MapGet("/", async (HttpContext httpContext, ApplicationDbContext context) =>
            {
                var id = httpContext.GetUserId();
                if (id == null)
                {
                    return Results.Unauthorized();
                }
                var user = await context.Users.FindAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user.ToUserDto());
            });


            // Get user scores
            group.MapGet("/scores", async (HttpContext httpContext,
            [FromQuery(Name = "sort")] int? order,
            [FromQuery(Name = "start")] DateTime? startDate,
            [FromQuery(Name = "end")] DateTime? endDate,
             ApplicationDbContext context) =>
            {
                var uid = httpContext.GetUserId();
                if (uid == null)
                {
                    return Results.Unauthorized();
                }
                var scoreList = await context.Scores
                .Where(s => s.UserId == uid)
                .Include(s => s.Game)
                .Select(s => s.ToScoreDto())
                .ToListAsync();
                if (scoreList.Count == 0)
                {
                    Results.NotFound("User hasn't played yet");
                }
                if (order is not null)
                {
                    scoreList = order == 1 ? [.. scoreList.OrderByDescending(s => s.Score)] : [.. scoreList.OrderByDescending(s => s.TimeStamp)];
                }
                if (startDate is not null)
                {
                    if (endDate is not null)
                    {
                        scoreList = [.. scoreList
                                        .Where(x => x.TimeStamp >= startDate.Value
                                        && x.TimeStamp <= endDate.Value)
                                    ];
                    }
                    else{
                        scoreList = [.. scoreList.Where(time => time.TimeStamp>= startDate.Value)];
                    }

                }


                return Results.Ok(scoreList);
            });


            // Get user scores in a specific game
            group.MapPost("/{id}", async (HttpContext httpContext, int id, SubmitScoreDto score, ApplicationDbContext context) =>
            {
                var uid = httpContext.GetUserId();
                if (uid == null)
                {
                    return Results.Unauthorized();
                }
                var scoreModel = score.ToScoreModel((int)uid, id);
                scoreModel.User = context.Users.Find(uid);
                scoreModel.Game = context.Games.Find(id);
                context.Add(scoreModel);
                await context.SaveChangesAsync();
                return Results.Created();

            });

            group.MapDelete("score/{id}", async (int id, HttpContext httpContext, ApplicationDbContext context) =>
            {
                var uid = httpContext.GetUserId();
                if (uid == null)
                {
                    return Results.Unauthorized();
                }
                Console.WriteLine("reached here");
                await context.Scores.Where(s => s.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });

            // Login and signup
            group.MapPost("/signup", async (CreateUserDto user, IAuthenicationService authService, ApplicationDbContext context) =>
            {
                var hash = authService.HashPassword(user.Password);
                var newUser = user.ToUserModel(hash);

                await context.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Results.Created();
            }).AllowAnonymous().AddEndpointFilter<ValidationFilter<CreateUserDto>>();

            group.MapPost("/login", async (UserLoginDto user, IAuthenicationService authService, ApplicationDbContext context) =>
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser is null)
                {
                    return Results.NotFound("password or email maybe wrong");
                }
                if (authService.VerifyPassword(user.Password, existingUser.Password))
                {
                    var token = authService.GenerateJwtToken(existingUser.Id, existingUser.UserName);
                    return Results.Ok(new { Token = token });
                }


                return Results.NotFound("password or email maybe wrong");
            }).AllowAnonymous();

            return group;
        }
    }
}