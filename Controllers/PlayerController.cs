using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var playerList = await _playerService.GetAllPlayersAsync();
            if (playerList.Count() == 0)
                return RedirectToAction("Create");

            return View(playerList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PlayerDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerDto player)
        {
            await _playerService.CreatePlayerAsync(player);
            return RedirectToAction("Index");
        }
    }
}
