using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Responses.Enemies;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Enemy/")]
    public class EnemyController : Controller
    {
        private readonly IEnemyGetService _enemyGetService;

        public EnemyController(IEnemyGetService enemyGetService)
        {
            _enemyGetService = enemyGetService;
        }

        [Route("Stats")]
        public async Task<IActionResult> GetCurrentEnemyStats()
        {
            var enemy = await _enemyGetService.GetIsActiveForCombat();
            EnemyStatResponse enemyStats = enemy;
            return Json(enemyStats);
        }
    }
}
