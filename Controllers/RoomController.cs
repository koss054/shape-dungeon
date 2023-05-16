using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCreateService _roomCreateService;

        public RoomController(
            IRoomService roomService,
            IRoomCreateService roomCreateService)
        {
            _roomService = roomService;
            _roomCreateService = roomCreateService;
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

        [HttpGet]
        public async Task<IActionResult> Left()
        {
            var rightRoomId = await _roomService.GetActiveForEditRoomIdAsync();
            var roomDto =  _roomCreateService.InitializeLeftRoom(rightRoomId);
            return View(roomDto);
        }

        [HttpPost]
        public async Task<IActionResult> Left(RoomCreateDto roomDto)
        {
            var oldRoomId = roomDto.RightRoomId!.Value;
            var newRoomId = await _roomCreateService.CreateRoomAsync(roomDto);

            await _roomCreateService.AddLeftNeighborAsync(oldRoomId, newRoomId);
            await _roomService.ChangeActiveForEditRoomAsync(oldRoomId, newRoomId);

            return RedirectToAction("Create");
        }
    }
}
