using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Models;
using System.Diagnostics;

namespace ShapeDungeon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IGetRoomService _getRoomService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public HomeController(
            IPlayerService playerService,
            IGetRoomService getRoomService, 
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _playerService = playerService;
            _getRoomService = getRoomService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _playerService.GetPlayerAsync("Nov Kryg Homiesss");
            if (player == null)
            {
            }

            var room = await _getRoomService.GetActiveAsync();
            if (room == null)
            {
            }

            var roomNav = await _checkRoomNeighborsService.SetDtoNeighborsAsync(room!.CoordX, room!.CoordY);
            if (roomNav == null)
            {
            }

            room = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav!);
            var game = new GameDto() { Player = player!, Room = room! };
            return View(game);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}