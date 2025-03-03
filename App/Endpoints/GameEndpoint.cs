using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Database;
using Microsoft.EntityFrameworkCore;
namespace App.Endpoints
{
    public static class GameEndpoint
    {
        public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games");

            group.MapGet("/", async (ApplicationDbContext context) =>
                    await context.Games.AsNoTracking().ToListAsync());

            group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
            {
                var game = await context.Games.Where(g => g.Id == id).Include(x => x.Scores).FirstOrDefaultAsync();
                return game is null ? Results.NotFound("There's No such Game") :
                Results.Ok(game);
            }
            ).WithName("GetGame");



            return group;
        }
    }
}