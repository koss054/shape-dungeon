﻿using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Specifications.Enemies;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyUpdateService : IEnemyUpdateService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyUpdateService(
            IEnemyRepository enemyRepository, 
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RemoveActiveForCombat()
        {
            var enemy = await _enemyRepository.GetFirstAsync(
                new EnemyActiveForCombatSpecification());

            if (enemy is null) throw new ArgumentNullException(nameof(enemy));

            await _unitOfWork.Commit(() =>
            {
                enemy.IsActiveForCombat = false;
            });
        }
    }
}
