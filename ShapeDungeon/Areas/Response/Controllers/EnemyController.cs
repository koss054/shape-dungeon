using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Responses;
using ShapeDungeon.Responses.Enemies;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Enemy/")]
    public class EnemyController : Controller
    {
        private readonly IEnemyGetService _enemyGetService;
        private readonly ICombatService _combatService;

        public EnemyController(
            IEnemyGetService enemyGetService, 
            ICombatService combatService)
        {
            _enemyGetService = enemyGetService;
            _combatService = combatService;
        }

        [HttpGet]
        [Route("Stats")]
        public async Task<IActionResult> GetCurrentEnemyStats()
        {
            var enemy = await _enemyGetService.GetIsActiveForCombat();
            EnemyStatResponse enemyStats = enemy;
            return Json(enemyStats);
        }

        [HttpPatch]
        [Route("Reduce-Health")]
        public async Task<IActionResult> ReduceEnemyHealth([FromBody]CharacterHpResponse response)
        {
            var updatedEnemyHp = await _combatService.UpdateHealthAfterAttack(
                response.HpToReduce, (int)CombatCharacterType.Enemy);
            return Json(updatedEnemyHp);
        }
    }
}
