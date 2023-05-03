using Microsoft.EntityFrameworkCore;

using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Services
{
    public class PlayerService : IPlayerService, IDisposable
    {
        private readonly IDbContext _context;

        public PlayerService(IDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePlayerAsync(PlayerDto pDto)
        {
            if (_context.Players.Any(x => x.Name == pDto.Name))
                return false;

            var player = new Player()
            {
                Name = pDto.Name,
                Strength = pDto.Strength,
                Vigor = pDto.Vigor,
                Agility = pDto.Agility,
                Level = pDto.Level,
                CurrentExp = pDto.CurrentExp,
                ExpToNextLevel = pDto.ExpToNextLevel,
                CurrentSkillpoints = pDto.CurrentSkillpoints,
                Shape = pDto.Shape
            };

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            return _context.Players.Any(x => x.Name == pDto.Name);
        }

        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
            => await _context.Players
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
                }).ToListAsync();

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

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
