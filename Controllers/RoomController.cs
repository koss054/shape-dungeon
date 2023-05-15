using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var room = await _roomService.GetActiveForEditRoomAsync();
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomCreateDto room)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid room!";
                return View(room);
            }

            await _roomService.CreateRoomAsync(room);
            return RedirectToAction("Index", "Home");
        }
    }
}
