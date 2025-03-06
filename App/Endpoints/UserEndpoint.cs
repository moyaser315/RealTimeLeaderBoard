using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Database;
using App.Dtos;
using App.Mapping;
using App.Models;
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

            group.MapGet("/{id}/scores", async (int id, ApplicationDbContext context) =>
            {
                var scoreList = await context.Scores
                .Where(s => s.UserId == id)
                .Include(s => s.Game)
                .Select(s => s.ToScoreDto())
                .ToListAsync();
                return scoreList.Count == 0 ? Results.NotFound("User hasn't played yet") : Results.Ok(scoreList);
            });

            group.MapPost("/{uid}/{id}", async (int uid, int id, SubmitScoreDto score, ApplicationDbContext context) =>
            {
                var scoreModel = score.ToScoreModel(uid, id);
                scoreModel.User = context.Users.Find(uid);
                scoreModel.Game = context.Games.Find(id);
                context.Add(scoreModel);
                await context.SaveChangesAsync();
                return Results.Created();

            });

            group.MapPost("/signup", (CreateUserDto user, ApplicationDbContext context) =>
            {
                var newUser = user.ToUserModel();
                context.Add(newUser);
                context.SaveChanges();
                return Results.Created();
            });

            group.MapPost("/login", async (UserLoginDto user, ApplicationDbContext context) =>
            {
                var pass = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                
                if(pass is null){
                    return Results.NotFound("pass or mail maybe wrong");
                }
                
                
                return user.Password == pass!.Password ? Results.Ok("Logged") : Results.NotFound("Password or mail maybe wrong");
            });

            return group;
        }
    }
}