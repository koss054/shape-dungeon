using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services;

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

        public async Task<IActionResult> Win()
        {
            var isWinValid = await _combatService.HasPlayerWon();
            if (isWinValid) return RedirectToAction("Index", "Active");
            return RedirectToAction("Action");
        }
    }
}
