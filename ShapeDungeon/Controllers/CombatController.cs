using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Interfaces.Services.Enemies;

namespace ShapeDungeon.Controllers
{
    public class CombatController : Controller
    {
        private readonly ICombatService _combatService;

        public CombatController(ICombatService combatService)
        {
            _combatService = combatService;
        }

        [HttpGet]
        public async Task<IActionResult> Action()
        {
            var combatDto = await _combatService.GetActiveCombat();
            return View(combatDto);
        }

        [HttpPatch]
        public async Task<IActionResult> Test([FromBody]TestResponse response)
        {
            await _combatService.Test(response.Hp);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Test2()
        {
            var currEnemyHp = await _combatService.Test2();
            var model = new
            {
                hp = currEnemyHp,
            };
            return Json(model);
        }
    }
}
