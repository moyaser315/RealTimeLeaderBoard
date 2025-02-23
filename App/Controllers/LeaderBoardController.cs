using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    [Route("[controller]")]
    public class LeaderBoardController : Controller
    {
        private readonly ILogger<LeaderBoardController> _logger;

        public LeaderBoardController(ILogger<LeaderBoardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ApplicationDbContext context =  new ApplicationDbContext();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}