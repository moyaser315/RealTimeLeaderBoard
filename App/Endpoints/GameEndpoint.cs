using App.Database;
using App.Dtos;
using App.Mapping;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints
{
    public static class GameEndpoint
    {
        public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games");

            // Get All Games
            group.MapGet("/", async (ApplicationDbContext context) =>
                    {
                        var games= await context.Games.AsNoTracking().ToListAsync();
                        if(games.Count != 0)
                        {
                            List<GamesDto> gameNames = [];
                            foreach(var game in games){
                                gameNames.Add(game.ToDto());
                            }
                            return Results.Ok(gameNames);
                        }
                        return Results.NotFound();
                    }
                );
            
            // Get Game Scores
            group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
            {
                var gameScores = await context.Scores
                .Where(g => g.GameID == id)
                .Include(g => g.User)
                .OrderByDescending(g => g.Score)
                .Select(g => g.ToGameScoreDto())
                .ToListAsync();
                // .Select(g => g.ToGameScoreDto())
                // .OrderByDescending(g => g.Score)
                

                return gameScores.Count == 0 ? 
                Results.NotFound("There's No such Game") :
                Results.Ok(gameScores);
            }
            ).WithName("GetGame");

    
            return group;
        }
    }
}