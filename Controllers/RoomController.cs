using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDto room)
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
