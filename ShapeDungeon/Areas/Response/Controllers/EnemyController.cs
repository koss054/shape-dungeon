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

        // TODO: Make controller smaller by moving methods and only returning JSON.
        [HttpPatch]
        [Route("Attack")]
        public async Task<IActionResult> Attack([FromBody]CharacterHpResponse response)
        {
            var activeCombat = await _combatService.GetActiveCombat();
            if (activeCombat.IsPlayerAttacking)
                throw new BadHttpRequestException("Enemy not able to attack, bruh.");

            var updatedPlayerHp = await _combatService.UpdateHealthAfterAttack(
                response.HpToReduce, (int)CombatCharacterType.Player);

            await _combatService.ToggleIsPlayerAttackingInActiveCombat();
            var isPlayerAttacking = await _combatService
                .IsPlayerAttackingInActiveCombat();

            var characterResponse = new CharacterDefendResponse
            {
                UpdatedCharacterHp = updatedPlayerHp,
                IsPlayerAttacking = isPlayerAttacking,
            };

            return Json(characterResponse);
        }

        // TODO: Make controller smaller by moving methods and only returning JSON.
        [HttpPatch]
        [Route("Defend")]
        public async Task<IActionResult> Defend([FromBody]CharacterHpResponse response)
        {
            var activeCombat = await _combatService.GetActiveCombat();
            if (!activeCombat.IsPlayerAttacking)
                throw new BadHttpRequestException("Player not able to attack, bruh.");

            var updatedEnemyHp = await _combatService.UpdateHealthAfterAttack(
                response.HpToReduce, (int)CombatCharacterType.Enemy);

            await _combatService.ToggleIsPlayerAttackingInActiveCombat();
            var isPlayerAttacking = await _combatService
                .IsPlayerAttackingInActiveCombat();

            var characterResponse = new CharacterDefendResponse
            {
                UpdatedCharacterHp = updatedEnemyHp,
                IsPlayerAttacking = isPlayerAttacking,
            };

            return Json(characterResponse);
        }
    }
}
