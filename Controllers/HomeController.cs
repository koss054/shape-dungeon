using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Entities.Enums;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Models;
using System.Diagnostics;

namespace ShapeDungeon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerService;

        public HomeController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _playerService.GetPlayerAsync("test");

            if (player == null)
            {
                bool isCreated = await _playerService.CreatePlayerAsync("test", PlayerShape.Square);

                if (isCreated)
                    player = await _playerService.GetPlayerAsync("test");
            }

            return View(player);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}