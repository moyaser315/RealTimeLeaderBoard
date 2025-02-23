using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        public IActionResult Game(string name)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var game = context.Games.FirstOrDefault(x => x.Name == name);
            if(game == null){
                return NotFound();
            }
            var scores = context.Scores.Where(x => x.GameID == game.Id).OrderBy(x =>x.Score).ToList();

            return View("Game");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}