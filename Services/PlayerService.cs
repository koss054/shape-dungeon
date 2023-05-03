using Microsoft.EntityFrameworkCore;

using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Entities.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IDbContext _context;

        public PlayerService(IDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePlayerAsync(string name, PlayerShape shape)
        {
            if (_context.Players.Any(x => x.Name == name))
                return false;

            var player = new Player()
            {
                Name = name,
                Strength = 5,
                Vigor = 10,
                Agility = 5,
                Level = 1,
                CurrentExp = 0,
                ExpToNextLevel = 25,
                CurrentSkillpoints = 1,
                Shape = shape
            };

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            return _context.Players.Any(x => x.Name == name);
        }

        public async Task<PlayerDto?> GetPlayerAsync(string name)
        {
            var player = await _context.Players
                .Where(x => x.Name == name)
                .Select(x => new PlayerDto()
                {
                    Name = x.Name,
                    Strength = x.Strength,
                    Vigor = x.Vigor,
                    Agility = x.Agility,
                    Level = x.Level,
                    CurrentExp = x.CurrentExp,
                    ExpToNextLevel = x.ExpToNextLevel,
                    CurrentSkillpoints = x.CurrentSkillpoints,
                    Shape = x.Shape
                }).FirstOrDefaultAsync();

            return player;
        }
    }
}
