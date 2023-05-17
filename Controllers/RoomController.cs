using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomCreateService _roomCreateService;

        public RoomController(
            IRoomService roomService,
            IGetRoomService getRoomService,
            IRoomCreateService roomCreateService)
        {
            _roomService = roomService;
            _getRoomService = getRoomService;
            _roomCreateService = roomCreateService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var room = await _getRoomService.GetActiveForEditAsync();
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

            await _roomCreateService.CreateAsync(room);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Directional(RoomDirection direction)
        {
            var roomDto = await _roomCreateService.InitializeRoomAsync(direction);
            return View(roomDto);
        }

        [HttpPost]
        public async Task<IActionResult> Directional(RoomCreateDto roomDto)
        {
            var newRoomId = await _roomCreateService.CreateAsync(roomDto);
            await _roomService.ApplyActiveForEditAsync(newRoomId);
            return RedirectToAction("Create");
        }
    }
}
