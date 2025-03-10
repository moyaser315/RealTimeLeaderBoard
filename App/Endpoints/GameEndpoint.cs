using App.Database;
using App.Dtos;
using App.Mapping;
using App.Services;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints
{
    public static class GameEndpoint
    {
        public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games").WithTags("Games");;

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
            group.MapGet("/{id}", async (int id, ApplicationDbContext context, IRedisCacheService cache) =>
            {
                string cacheKey = $"game_scores_{id}";
                var cachedScores = await cache.GetCacheAsync<List<GameScoreDto>>(cacheKey);

                if (cachedScores is not null) return Results.Ok(cachedScores);

                var gameScores = await context.Scores
                    .Where(g => g.GameID == id)
                    .Include(g => g.User)
                    .OrderByDescending(g => g.Score)
                    .Select(g => g.ToGameScoreDto())
                    .ToListAsync();

                if (gameScores.Count == 0) return Results.NotFound("There's No such Game");


                await cache.SetCacheAsync(cacheKey, gameScores, TimeSpan.FromMinutes(5));

                return Results.Ok(gameScores);
            }
            ).WithName("GetGame");

    
            return group;
        }
    }
}