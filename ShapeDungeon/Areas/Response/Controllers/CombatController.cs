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
        private readonly IEnemyUpdateService _enemyUpdateService;
        private readonly IPlayerCombatService _playerCombatService;
        private readonly IPlayerUpdateService _playerUpdateService;

        public CombatController(
            ICombatService combatService,
            IEnemyGetService enemyGetService,
            IEnemyUpdateService enemyUpdateService,
            IPlayerCombatService playerCombatService, 
            IPlayerUpdateService playerUpdateService)
        {
            _combatService = combatService;
            _enemyGetService = enemyGetService;
            _enemyUpdateService = enemyUpdateService;
            _playerCombatService = playerCombatService;
            _playerUpdateService = playerUpdateService;
        }

        [HttpGet]
        [Route("End")]
        public async Task<IActionResult> End()
        {
            var isWinValid = await _combatService.HasPlayerWon();
            if (isWinValid)
            {
                var gainedExp = await _enemyGetService.GetActiveForCombatExp();
                await _enemyUpdateService.RemoveActiveForCombat();
                await _playerCombatService.GainExp(gainedExp);
                await _playerUpdateService.LevelUp();
                await _playerCombatService.ExitCombat();
            }

            return Json(isWinValid);
        }
    }
}
