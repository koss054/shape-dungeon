using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Player;
using ShapeDungeon.Interfaces.Services.Players;

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
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Name can't be empty";
                return RedirectToAction("Create");
            }

            var isCreated = await _playerService.CreatePlayerAsync(player);

            if (!isCreated)
            {
                TempData["duplicate"] = "Player with this name already exists! Try again.";
                return View(player);
            }

            return RedirectToAction("Active", "Home");
        }
    }
}
