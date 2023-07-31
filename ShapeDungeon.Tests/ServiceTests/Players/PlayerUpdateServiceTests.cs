using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Specifications.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    public class PlayerUpdateServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IPlayerUpdateService _sut;
        private readonly IFixture _fixture;

        public PlayerUpdateServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new PlayerUpdateService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(IncreaseStat_ValidItemsData))]
        public async Task IncreaseStat_ShouldIncreaseSpecifiedStat_WhenAnActivePlayerWithAvailableSkillpointsIsInDb(
            int expectedNewStatValue, 
            int expectedLeftoverSkillpoints, 
            int expectedNewLevel, 
            Player expectedPlayer, 
            CharacterStat expectedStatToIncrease)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            await _sut.IncreaseStat(expectedStatToIncrease);

            var actualNewStatValue =
                expectedStatToIncrease == CharacterStat.Strength ? expectedPlayer.Strength
                : expectedStatToIncrease == CharacterStat.Vigor ? expectedPlayer.Vigor
                : expectedPlayer.Agility;

            var actualLeftoverSkillpoints = expectedPlayer.CurrentSkillpoints;
            var actualNewLevel = expectedPlayer.Level;

            // Assert
            _repoMock.Verify(x => x.Update(expectedPlayer), Times.Once());
            actualNewStatValue.Should().Be(expectedNewStatValue);
            actualLeftoverSkillpoints.Should().Be(expectedLeftoverSkillpoints);
            actualNewLevel.Should().Be(expectedNewLevel);
        }

        [Theory]
        [MemberData(nameof(IncreaseStat_NoAvailableSkillpointsItemsData))]
        public async Task IncreaseStat_ShouldDoNoUpdatesToActivePlayer_WhenActivePlayerHasNoAvailableSkillpoints(
            int expectedStrength, 
            int expectedVigor, 
            int expectedAgility, 
            int expectedLevel, 
            Player expectedPlayer, 
            CharacterStat expectedStatToIncrease)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            await _sut.IncreaseStat(expectedStatToIncrease);

            var actualStrength = expectedPlayer.Strength;
            var actualVigor = expectedPlayer.Vigor;
            var actualAgility = expectedPlayer.Agility;
            var actualLevel = expectedPlayer.Level;

            // Assert
            _repoMock.Verify(x => x.Update(expectedPlayer), Times.Never());
            actualStrength.Should().Be(expectedStrength);
            actualVigor.Should().Be(expectedVigor);
            actualAgility.Should().Be(expectedAgility);
            actualLevel.Should().Be(expectedLevel);
        }

        [Theory]
        [MemberData(nameof(IncreaseStat_NoActivePlayerItemsData))]
        public async Task IncreaseStat_ShouldThrowException_WhenNoActivePlayerInDb(
            CharacterStat expectedStatToIncrease)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.IncreaseStat(expectedStatToIncrease);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Fact]
        public async Task IncreaseStat_ShouldThrowException_WhenStatToIncreaseIsNotValid()
        {
            // Arrange
            var expectedStatToIncrease = (CharacterStat)10;
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var action = async () => await _sut.IncreaseStat(expectedStatToIncrease);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage($"Specified argument was out of the range of valid values. (Parameter '{expectedStatToIncrease}')");
        }

        [Fact]
        public async Task EnterCombat_ShouldPutPlayerInCombat_WhenAnActivePlayerIsInDb()
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .With(x => x.IsInCombat, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            await _sut.EnterCombat();

            // Assert
            _repoMock.Verify(x => x.Update(expectedPlayer), Times.Once);
            expectedPlayer.IsInCombat.Should().BeTrue();
        }

        [Fact]
        public async Task EnterCombat_ShouldThrowException_WhenNoActivePlayerIsInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.EnterCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Theory]
        [MemberData(nameof(LevelUp_ValidItemsData))]
        public async Task LevelUp_ShouldUpdatePlayer_WhenActivePlayerIsInDb(
            int expectedCurrentExp,
            int expectedCurrentSkillpoints,
            int expectedExpToNextLevel,
            Player expectedPlayer)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            await _sut.LevelUp();

            var actualCurrentExp = expectedPlayer.CurrentExp;
            var actualCurrentSkillpoints = expectedPlayer.CurrentSkillpoints;
            var actualExpToNextLevel = expectedPlayer.ExpToNextLevel;

            // Assert
            _repoMock.Verify(x => x.Update(expectedPlayer), Times.Once);
            actualCurrentExp.Should().Be(expectedCurrentExp);
            actualCurrentSkillpoints.Should().Be(expectedCurrentSkillpoints);
            actualExpToNextLevel.Should().Be(expectedExpToNextLevel);
        }

        [Fact]
        public async Task LevelUp_ShouldThrowException_WhenNoActivePlayerIsInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.LevelUp();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        public static IEnumerable<object[]> IncreaseStat_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    6,                          // Expected Updated Stat Value (Strength)
                    1,                          // Expected Leftover Skillpoints
                    18,                         // Expected New Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 2,
                        Level = 17,
                    },
                    CharacterStat.Strength,     // Expected Stat to Increase
                },
                new object[]
                {
                    11,                         // Expected Updated Stat Value (Vigor)
                    0,                          // Expected Leftover Skillpoints
                    18,                         // Expected New Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 1,
                        Level = 17,
                    },
                    CharacterStat.Vigor,        // Expected Stat to Increase
                },
                new object[]
                {
                    3,                          // Expected Updated Stat Value (Agility)
                    1,                          // Expected Leftover Skillpoints
                    18,                         // Expected New Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 2,
                        Level = 17,
                    },
                    CharacterStat.Agility,      // Expected Stat to Increase
                },
            };

        public static IEnumerable<object[]> IncreaseStat_NoAvailableSkillpointsItemsData
            => new List<object[]>
            {
                new object[]
                {
                    5,                          // Expected Strength)
                    10,                         // Expected Vigor
                    2,                          // Expected Agility
                    17,                         // Expected Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 0,
                        Level = 17,
                    },
                    CharacterStat.Strength,     // Expected Stat to Increase
                },
                new object[]
                {
                    5,                          // Expected Strength)
                    10,                         // Expected Vigor
                    2,                          // Expected Agility
                    17,                         // Expected Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 0,
                        Level = 17,
                    },
                    CharacterStat.Vigor,        // Expected Stat to Increase
                },
                new object[]
                {
                    5,                          // Expected Strength)
                    10,                         // Expected Vigor
                    2,                          // Expected Agility
                    17,                         // Expected Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        Strength = 5,
                        Vigor = 10,
                        Agility = 2,
                        CurrentSkillpoints = 0,
                        Level = 17,
                    },
                    CharacterStat.Agility,      // Expected Stat to Increase
                },
            };

        public static IEnumerable<object[]> IncreaseStat_NoActivePlayerItemsData
            => new List<object[]>
            {
                new object[]
                {
                    CharacterStat.Strength,     // Expected Stat to Increase
                },
                new object[]
                {
                    CharacterStat.Vigor,        // Expected Stat to Increase
                },
                new object[]
                {
                    CharacterStat.Agility,      // Expected Stat to Increase
                },
            };

        public static IEnumerable<object[]> LevelUp_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    0,                          // Expected Current Exp
                    2,                          // Expected Current Skillpoints
                    200,                        // Expected Exp to Next Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        CurrentExp = 250,
                        CurrentSkillpoints = 0,
                        ExpToNextLevel = 100,
                    },
                },
                new object[]
                {
                    50,                         // Expected Current Exp
                    2,                          // Expected Current Skillpoints
                    200,                        // Expected Exp to Next Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        CurrentExp = 300,
                        CurrentSkillpoints = 0,
                        ExpToNextLevel = 100,
                    },
                },
                new object[]
                {
                    10,                         // Expected Current Exp
                    0,                          // Expected Current Skillpoints
                    100,                        // Expected Exp to Next Level
                    new Player                  // Expected Player
                    {
                        IsActive = true,
                        CurrentExp = 10,
                        CurrentSkillpoints = 0,
                        ExpToNextLevel = 100,
                    },
                },
            };
    }
}
