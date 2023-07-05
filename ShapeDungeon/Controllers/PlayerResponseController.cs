using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Controllers
{
    public class PlayerResponseController : Controller
    {
        private readonly IPlayerGetService _playerGetService;

        public PlayerResponseController(IPlayerGetService playerGetService)
        {
            _playerGetService = playerGetService;
        }

        public async Task<IActionResult> GetStats()
        {
            var playerStats = await _playerGetService.GetActivePlayerStats();
            return Json(playerStats);
        }
    }
}
