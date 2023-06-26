using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerCreateService _playerCreateService;
        private readonly IPlayerGetService _playerGetService;

        public PlayerController(
            IPlayerCreateService playerCreateService, 
            IPlayerGetService playerGetService)
        {
            _playerCreateService = playerCreateService;
            _playerGetService = playerGetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var playerList = await _playerGetService.GetAllPlayersAsync();
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

            var isCreated = await _playerCreateService.CreatePlayerAsync(player);

            if (!isCreated)
            {
                TempData["duplicate"] = "Player with this name already exists! Try again.";
                return View(player);
            }

            return RedirectToAction("Active", "Home");
        }
    }
}
