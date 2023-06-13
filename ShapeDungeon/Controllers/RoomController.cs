using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomCreateService _roomCreateService;
        private readonly IRoomActiveForEditService _roomActiveForEditService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public RoomController(
            IGetRoomService getRoomService,
            IRoomCreateService roomCreateService, 
            IRoomActiveForEditService roomActiveForEditService,
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _getRoomService = getRoomService;
            _roomCreateService = roomCreateService;
            _roomActiveForEditService = roomActiveForEditService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roomDetails = await _getRoomService.GetActiveForEditAsync();
            var roomNav = await _checkRoomNeighborsService.SetDtoNeighborsAsync(roomDetails!.CoordX, roomDetails!.CoordY);
            var room = new RoomCreateDto() { Details = roomDetails, Nav = roomNav };
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDetailsDto roomDto)
        {
            var newRoom = await _roomCreateService.CreateAsync(roomDto);
            await _roomActiveForEditService.ApplyActiveForEditAsync(newRoom.Id);
            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> Directional(RoomDirection direction)
        {
            var roomDetails = await _roomCreateService.InitializeRoomAsync(direction);
            var room = new RoomCreateDto() { Details = roomDetails };
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> Go(int coordX, int coordY)
        {
            await _roomActiveForEditService.MoveActiveForEditAsync(coordX, coordY);
            return RedirectToAction("Create");
        }
    }
}
