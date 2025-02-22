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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Profile(int Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var info = context.Users.FirstOrDefault(x => x.Id == Id);
            var scores = context.Scores.Where(x => x.UserId == Id).GroupBy(x => x.GameID);
            ViewBag["Info"]= info;
            ViewBag["Scores"] = scores;
            return View("Profile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}