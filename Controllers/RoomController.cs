﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ICheckRoomNeighborsService _checkRoomNeighborsService;

        public RoomController(
            IRoomService roomService,
            IGetRoomService getRoomService,
            IRoomCreateService roomCreateService, 
            ICheckRoomNeighborsService checkRoomNeighborsService)
        {
            _roomService = roomService;
            _getRoomService = getRoomService;
            _roomCreateService = roomCreateService;
            _checkRoomNeighborsService = checkRoomNeighborsService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roomDetails = await _getRoomService.GetActiveForEditAsync();
            var roomNav = await _checkRoomNeighborsService.SetDtoNeighborsAsync(roomDetails.CoordX, roomDetails.CoordY);
            var room = new RoomCreateDto() { Details = roomDetails, Nav = roomNav };
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDetailsDto room)
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
        public async Task<IActionResult> Directional(RoomDetailsDto roomDto)
        {
            var newRoomId = await _roomCreateService.CreateAsync(roomDto);
            await _roomService.ApplyActiveForEditAsync(newRoomId);
            return RedirectToAction("Create");
        }
    }
}
