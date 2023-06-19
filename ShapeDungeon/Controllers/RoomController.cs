using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Interfaces.Services.RoomsEnemies;

namespace ShapeDungeon.Controllers
{
    public class RoomController : Controller
    {
        private readonly IEnemyService _enemyService;
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomCreateService _roomCreateService;
        private readonly IRoomActiveForEditService _roomActiveForEditService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnemiesRoomsService _enemiesRoomsService;

        public RoomController(
            IGetRoomService getRoomService,
            IRoomCreateService roomCreateService,
            IRoomActiveForEditService roomActiveForEditService,
            ICheckRoomNeighborsService checkRoomNeighborsService,
            IEnemyService enemyService,
            IHttpContextAccessor httpContextAccessor, 
            IEnemiesRoomsService enemiesRoomsService)
        {
            _getRoomService = getRoomService;
            _roomCreateService = roomCreateService;
            _roomActiveForEditService = roomActiveForEditService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
            _enemyService = enemyService;
            _httpContextAccessor = httpContextAccessor;
            _enemiesRoomsService = enemiesRoomsService;
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

            if (newRoom.IsEnemyRoom)
            {
                var enemy = await _enemyService.GetById(roomDto.EnemyId);

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
            var enemyRange = await _enemyService.GetRangeAsync(GetLevel("min"), GetLevel("max"));
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
