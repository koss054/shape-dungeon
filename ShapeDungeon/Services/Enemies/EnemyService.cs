using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyService : IEnemyService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyService(
            IEnemyRepository enemyRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(EnemyDto eDto)
        {
            var enemy = new Enemy()
            {
                Name = $"{eDto.Name} Lvl.{eDto.Level}",
                Strength = eDto.Strength,
                Vigor = eDto.Vigor,
                Agility = eDto.Agility,
                Level = eDto.Level,
                DroppedExp = eDto.Level * 10,
                CurrentHp = eDto.Vigor + eDto.Strength,
                Shape = eDto.Shape,
            };

            await _unitOfWork.Commit(() =>
            {
                _enemyRepository.AddAsync(enemy);
            });
        }

        public async Task<IEnumerable<EnemyRangeDto>> GetRangeAsync(int minLevel, int maxLevel)
        {
            var enemies = await _enemyRepository.GetRangeAsync(minLevel, maxLevel);
            var enemyDtos = new List<EnemyRangeDto>();

            foreach (var enemy in enemies)
                enemyDtos.Add(enemy);

            return enemyDtos;
        }

        public async Task<Enemy?> GetById(Guid enemyId)
        {
            var enemy = await _enemyRepository.GetById(enemyId);
            return enemy;

        }
    }
}
