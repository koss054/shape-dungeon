﻿using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Players
{
    public class PlayerUpdateService : IPlayerUpdateService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerUpdateService(
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IncreaseStat(CharacterStat statToIncrease)
        {
            var activePlayer = await GetActivePlayer();
            if (activePlayer.CurrentSkillpoints > 0)
            {
                await _unitOfWork.Commit(() =>
                {
                    switch (statToIncrease)
                    {
                        case CharacterStat.Strength: activePlayer.Strength++; break;
                        case CharacterStat.Vigor: activePlayer.Vigor++; break;
                        case CharacterStat.Agility: activePlayer.Agility++; break;
                        default: throw new ArgumentOutOfRangeException(statToIncrease.ToString());
                    }

                    activePlayer.CurrentSkillpoints--;
                    activePlayer.Level++;
                });
            }
        }

        // Move to player combat serivce.
        public async Task EnterCombat()
        {
            var activePlayer = await GetActivePlayer();
            await _unitOfWork.Commit(() =>
            {
                activePlayer.IsInCombat = true;
            });
        }

        public async Task LoseCombat()
        {
            var activePlayer = await GetActivePlayer();

            await _unitOfWork.Commit(() =>
            {
                activePlayer.IsInCombat = false;
                activePlayer.CurrentExp = 0;
            });
        }

        public async Task LevelUp()
        {
            var activePlayer = await GetActivePlayer();
            while (activePlayer.CurrentExp >= activePlayer.ExpToNextLevel)
            {
                await _unitOfWork.Commit(() =>
                {
                    activePlayer.CurrentExp -= activePlayer.ExpToNextLevel;
                    activePlayer.CurrentSkillpoints++;
                    activePlayer.ExpToNextLevel += 50;
                });
            }
        }

        private async Task<Player> GetActivePlayer()
        {
            var player = await _playerRepository.GetActive();

            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return player;
        }
    }
}
