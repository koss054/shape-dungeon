using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IEnemyGetService _enemyGetService;
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomEnemyService _roomEnemyService;
        private readonly IRoomCreateService _roomCreateService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnemiesRoomsService _enemiesRoomsService;
        private readonly IRoomActiveForEditService _roomActiveForEditService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public RoomController(
            IEnemyGetService enemyGetService,
            IGetRoomService getRoomService, 
            IRoomEnemyService roomEnemyService, 
            IRoomCreateService roomCreateService, 
            IHttpContextAccessor httpContextAccessor, 
            IEnemiesRoomsService enemiesRoomsService, 
            IRoomActiveForEditService roomActiveForEditService, 
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _enemyGetService = enemyGetService;
            _getRoomService = getRoomService;
            _roomEnemyService = roomEnemyService;
            _roomCreateService = roomCreateService;
            _httpContextAccessor = httpContextAccessor;
            _enemiesRoomsService = enemiesRoomsService;
            _roomActiveForEditService = roomActiveForEditService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roomDetails = await _getRoomService.GetActiveForEditAsync();
            var roomNav = await _checkRoomNeighborsService
                .SetDtoNeighborsAsync(roomDetails!.CoordX, roomDetails!.CoordY);

            var room = new RoomCreateDto() { Details = roomDetails, Nav = roomNav };

            if (roomDetails.IsEnemyRoom)
            {
                var roomId = await _getRoomService.GetActiveForEditId();
                room.Enemy = await _roomEnemyService.GetEnemy(roomId);
            }

            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDetailsDto roomDto)
        {
            var newRoom = await _roomCreateService.CreateAsync(roomDto);

            if (newRoom.IsEnemyRoom)
            {
                var enemy = await _enemyGetService.GetById(roomDto.EnemyId);

                if (enemy != null)
                    await _enemiesRoomsService.AddEnemyToRoom(newRoom, enemy);
            }

            await _roomActiveForEditService.ApplyActiveForEditAsync(newRoom.Id);
            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> Directional(RoomDirection direction)
        {
            var roomDetails = await _roomCreateService.InitializeRoomAsync(direction);

            if (await _roomCreateService.AreCoordsInUse(roomDetails.CoordX, roomDetails.CoordY))
            {
                TempData["error"] = "bruh, there's already a room with these coords";
                return RedirectToAction("Create");
            }

            var enemyRange = await _enemyGetService.GetRangeAsync(GetLevel("min"), GetLevel("max"));
            var room = new RoomCreateDto() { Details = roomDetails, EnemyRange = enemyRange };
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> Go(int coordX, int coordY)
        {
            await _roomActiveForEditService.MoveActiveForEditAsync(coordX, coordY);
            return RedirectToAction("Create");
        }

        private int GetLevel(string cookieName)
        {
            var level = _httpContextAccessor.HttpContext!.Request.Cookies[cookieName];
            return level != null ? int.Parse(level) : 0; // Shouldn't be null as it's set from js but still.
        }
    }
}
