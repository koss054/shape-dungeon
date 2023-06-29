using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerCreateService _playerCreateService;
        private readonly IPlayerGetService _playerGetService;
        private readonly IPlayerSelectService _playerSelectService;
        private readonly IPlayerUpdateService _playerUpdateService;

        public PlayerController(
            IPlayerCreateService playerCreateService,
            IPlayerGetService playerGetService,
            IPlayerSelectService playerSelectService, 
            IPlayerUpdateService playerUpdateService)
        {
            _playerCreateService = playerCreateService;
            _playerGetService = playerGetService;
            _playerSelectService = playerSelectService;
            _playerUpdateService = playerUpdateService;
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

        [HttpGet]
        public async Task<IActionResult> Current()
        {
            var activePlayer = await _playerGetService.GetActivePlayer();
            return View(activePlayer);
        }

        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var playerList = await _playerGetService.GetAllPlayersAsync();
            return View(playerList);
        }

        [HttpGet]
        public async Task<IActionResult> Switch(Guid newId)
        {
            await _playerSelectService.UpdateActivePlayer(newId);
            return RedirectToAction("Select");
        }

        [HttpGet]
        public async Task<IActionResult> Increase(CharacterStat statToIncrease)
        {
            await _playerUpdateService.IncreaseStat(statToIncrease);
            return RedirectToAction("Current");
        }
    }
}
