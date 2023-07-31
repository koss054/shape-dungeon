using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomEnemyService _roomEnemyService;
        private readonly IRoomTravelService _roomTravelService;
        private readonly IRoomConditionService _roomConditionService;
        private readonly IPlayerCombatService _playerCombatService;
        private readonly IPlayerGetService _playerGetService;
        private readonly IPlayerScoutService _playerScoutService;
        private readonly IPlayerUpdateService _playerUpdateService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public HomeController(
            IGetRoomService getRoomService,
            IRoomEnemyService roomEnemyService,
            IRoomTravelService roomTravelService,
            IRoomConditionService roomConditionService,
            IPlayerCombatService playerCombatService,
            IPlayerGetService playerGetService,
            IPlayerScoutService playerScoutService,
            IPlayerUpdateService playerUpdateService,
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _getRoomService = getRoomService;
            _roomEnemyService = roomEnemyService;
            _roomTravelService = roomTravelService;
            _roomConditionService = roomConditionService;
            _playerCombatService = playerCombatService;
            _playerGetService = playerGetService;
            _playerScoutService = playerScoutService;
            _playerUpdateService = playerUpdateService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        public IActionResult Index()
            => RedirectToAction("Active");

        public async Task<IActionResult> Active()
        {
            await _roomTravelService.IsScoutResetAsync(); // Doing this if player changes the URL manually.
            await _playerScoutService.UpdateActiveScoutEnergyAsync(PlayerScoutAction.Refill);

            var player = await _playerGetService.GetActivePlayer();
            if (player.IsInCombat)
                return RedirectToAction("Action", "Combat");

            var room = await _getRoomService.GetActiveForMoveAsync();
            var roomNav = await _checkRoomNeighborsService
                .SetDtoNeighborsAsync(room.CoordX, room.CoordY);

            room = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav);
            if (room.IsEnemyRoom)
            {
                var roomId = await _getRoomService.GetActiveForMoveId();
                room.Enemy = await _roomEnemyService.GetEnemy(roomId);
                room.IsEnemyDefeated = await _roomEnemyService.IsEnemyDefeated(roomId);
            }

            var game = new GameDto() { Player = player, Room = room };
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> Scouting()
        {
            var player = await _playerGetService.GetActivePlayer();
            if (player.IsInCombat)
                return RedirectToAction("Action", "Combat");

            var room = await _getRoomService.GetActiveForScoutAsync();
            if (room.IsActiveForMove)
                return RedirectToAction("Active");

            var roomNav = await _checkRoomNeighborsService
                .SetDtoNeighborsAsync(room.CoordX, room.CoordY);

            room = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav);
            if (room.IsEnemyRoom)
            {
                var roomId = await _getRoomService.GetActiveForScoutId();
                room.Enemy = await _roomEnemyService.GetEnemy(roomId);
                room.IsEnemyDefeated = await _roomEnemyService.IsEnemyDefeated(roomId);
            }

            var game = new GameDto() { Player = player, Room = room };
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> Move(RoomDirection direction)
        {
            var activePlayer = await _playerGetService.GetActivePlayer();
            if (activePlayer.IsInCombat) 
                return RedirectToAction("Action", "Combat");

            await _roomTravelService.RoomTravelAsync(direction, RoomTravelAction.Move);

            if (await _roomConditionService.IsCurrentRoomActiveEnemyRoom())
            {
                await _playerUpdateService.EnterCombat();
                return RedirectToAction("Action", "Combat");
            }

            await _playerCombatService.ExitCombat();
            return RedirectToAction("Active");
        }

        [HttpGet]
        public async Task<IActionResult> Scout(RoomDirection direction)
        {
            var activePlayer = await _playerGetService.GetActivePlayer();
            if (activePlayer.IsInCombat)
                return RedirectToAction("Action", "Combat");

            var energyLeft = await _playerScoutService
                .UpdateActiveScoutEnergyAsync(PlayerScoutAction.Reduce);

            if (energyLeft != -1)
                await _roomTravelService.RoomTravelAsync(direction, RoomTravelAction.Scout);
            else
                TempData["no-energy"] = "No scouting energy left... Return to active room!";

            return RedirectToAction("Scouting");
        }

        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 454)
                return RedirectToAction("Index");

            return View();
        }
    }
}