using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Combat/")]
    public class CombatController : Controller
    {
        private readonly ICombatService _combatService;

        public CombatController(ICombatService combatService)
        {
            _combatService = combatService;
        }

        [HttpGet]
        [Route("Win")]
        public async Task<IActionResult> Win()
        {
            var isWinValid = await _combatService.HasPlayerWon();
            return Json(isWinValid);
        }
    }
}
