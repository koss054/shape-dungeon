﻿using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Interfaces.Services.Enemies;

namespace ShapeDungeon.Controllers
{
    public class EnemyController : Controller
    {
        private readonly IEnemyCreateService _enemyService;

        public EnemyController(IEnemyCreateService enemyService)
        {
            _enemyService = enemyService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Level = 8 because the selected on page load square shape stats add up to this number.
            var model = new EnemyDto() { Level = 8 };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EnemyDto enemy)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Name can't be empty";
                return RedirectToAction("Create");
            }

            await _enemyService.CreateAsync(enemy);
            return RedirectToAction("Active", "Home");
        }
    }
}
