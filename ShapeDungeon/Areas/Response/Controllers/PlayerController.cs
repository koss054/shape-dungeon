using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Player/")]
    public class PlayerController : Controller
    {
        private readonly IPlayerGetService _playerGetService;

        public PlayerController(IPlayerGetService playerGetService)
        {
            _playerGetService = playerGetService;
        }

        [Route("Stats")]
        public async Task<IActionResult> GetActivePlayerStats()
        {
            var playerStats = await _playerGetService.GetActivePlayerStats();
            return Json(playerStats);
        }
    }
}
