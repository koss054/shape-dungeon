using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Room;
using ShapeDungeon.Models;
using System.Diagnostics;

namespace ShapeDungeon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IRoomService _roomService;

        public HomeController(
            IPlayerService playerService,
            IRoomService roomService)
        {
            _playerService = playerService;
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _playerService.GetPlayerAsync("Nov Kryg Homiesss");
            var room = await _roomService.GetActiveRoomAsync();

            if (player == null)
            {
            }

            if (room == null)
            {
            }

            var game = new GameDto() { Player = player, Room = room };
            return View(game);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}