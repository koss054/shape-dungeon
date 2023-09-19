using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Areas.Response.Controllers
{
    [Route("Response/Room/")]
    public class RoomController : Controller
    {
        private readonly IGetRoomService _roomGetService;
        private readonly IRoomEnemyService _roomEnemyService;
        private readonly IRoomTravelService _roomTravelService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public RoomController(
            IGetRoomService roomGetService,
            IRoomEnemyService roomEnemyService,
            IRoomTravelService roomTravelService, 
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _roomGetService = roomGetService;
            _roomEnemyService = roomEnemyService;
            _roomTravelService = roomTravelService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        [Route("Partial/Style")]
        public async Task<IActionResult> GetRoomStyleView()
            => PartialView("~/Views/Home/_RoomStyle.cshtml",
                await _roomGetService.GetActiveForMoveDtoAsync());

        [Route("Partial/Active")]
        public async Task<IActionResult> GetRoomWithNavView()
        {
            Room room = await _roomGetService.GetActiveForMoveAsync();
            RoomDto roomDto = room;

            var roomNav = await _checkRoomNeighborsService
                .SetDtoNeighborsAsync(roomDto.CoordX, roomDto.CoordY);

            roomDto = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav);

            if (room.IsEnemyRoom)
            {
                roomDto.Enemy = await _roomEnemyService.GetEnemy(room.Id);
                roomDto.IsEnemyDefeated = await _roomEnemyService.IsEnemyDefeated(room.Id);
            }

            return PartialView("~/Views/Home/_RoomWithNav.cshtml", roomDto);
        }

        [Route("Move/Up")]
        public async Task MoveUp()
            => await _roomTravelService.RoomTravelAsync(RoomDirection.Top, RoomTravelAction.Move);

        [Route("Details/Move")]
        public async Task<IActionResult> GetActiveForMoveDetails()
        {
            var roomDetails = await _roomGetService.GetActiveForMoveDtoAsync();
            return Json(roomDetails);
        }

        [Route("Details/Scout")]
        public async Task<IActionResult> GetActiveForScoutDetails()
        {
            var roomDetails = await _roomGetService.GetActiveForScoutAsync();
            return Json(roomDetails);
        }

        [Route("Details/Edit")]
        public async Task<IActionResult> GetActiveForEditDetails()
        {
            var roomDetails = await _roomGetService.GetActiveForEditAsync();
            return Json(roomDetails);
        }
    }
}
