using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Combat/")]
    public class CombatController : Controller
    {
        private readonly ICombatService _combatService;
        private readonly IEnemyGetService _enemyGetService;
        private readonly IPlayerCombatService _playerCombatService;

        public CombatController(
            ICombatService combatService, 
            IEnemyGetService enemyGetService, 
            IPlayerCombatService playerCombatService)
        {
            _combatService = combatService;
            _enemyGetService = enemyGetService;
            _playerCombatService = playerCombatService;
        }

        [HttpGet]
        [Route("Win")]
        public async Task<IActionResult> Win()
        {
            var isWinValid = await _combatService.HasPlayerWon();
            if (isWinValid)
            {
                var gainedExp = await _enemyGetService.GetActiveForCombatExp();
                await _playerCombatService.GainExp(gainedExp);
                await _playerCombatService.ExitCombat();
            }

            return Json(isWinValid);
        }
    }
}
