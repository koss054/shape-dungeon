using Microsoft.AspNetCore.Mvc;
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
    }
}
