﻿using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IGetRoomService _getRoomService;
        private readonly IRoomTravelService _roomTravelService;
        private readonly IPlayerScoutService _playerScoutService;
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public HomeController(
            IPlayerService playerService,
            IGetRoomService getRoomService,
            IRoomTravelService roomTravelService,
            IPlayerScoutService playerScoutService,
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _playerService = playerService;
            _getRoomService = getRoomService;
            _roomTravelService = roomTravelService;
            _playerScoutService = playerScoutService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        public IActionResult Index()
            => RedirectToAction("Active");

        public async Task<IActionResult> Active()
        {
            await _roomTravelService.ResetScoutAsync(); // Doing this if player changes the URL manually.
            await _playerScoutService.UpdateActiveScoutEnergyAsync(PlayerScoutAction.Refill);

            var player = await _playerService.GetPlayerAsync("Squary Lvl.8");
            if (player == null)
            {
            }

            var room = await _getRoomService.GetActiveForMoveAsync();
            if (room == null)
            {
            }

            var roomNav = await _checkRoomNeighborsService.SetDtoNeighborsAsync(room!.CoordX, room!.CoordY);
            if (roomNav == null)
            {
            }

            room = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav!);
            var game = new GameDto() { Player = player!, Room = room! };
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> Scouting()
        {
            var player = await _playerService.GetPlayerAsync("Trizybeca na Poseidon");
            if (player == null)
            {
            }

            var room = await _getRoomService.GetActiveForScoutAsync();
            if (room == null)
            {
            }
            else if (room!.IsActiveForMove)
                return RedirectToAction("Active");

            var roomNav = await _checkRoomNeighborsService.SetDtoNeighborsAsync(room!.CoordX, room!.CoordY);
            if (roomNav == null)
            {
            }

            room = _checkRoomNeighborsService.SetHasNeighborsProperties(room, roomNav!);
            var game = new GameDto() { Player = player!, Room = room! };
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> Move(RoomDirection direction)
        {
            await _roomTravelService.RoomTravelAsync(direction, RoomTravelAction.Move);
            return RedirectToAction("Active");
        }

        [HttpGet]
        public async Task<IActionResult> Scout(RoomDirection direction)
        {
            var energyLeft = await _playerScoutService.UpdateActiveScoutEnergyAsync(PlayerScoutAction.Reduce);

            if (energyLeft != -1)
                await _roomTravelService.RoomTravelAsync(direction, RoomTravelAction.Scout);
            else
                TempData["no-energy"] = "No scouting energy left... Return to active room!";

            return RedirectToAction("Scouting");
        }
    }
}