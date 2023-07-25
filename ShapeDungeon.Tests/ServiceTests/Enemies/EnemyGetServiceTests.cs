using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Services.Enemies;
using ShapeDungeon.Specifications.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Enemies
{
    public class EnemyGetServiceTests
    {
        private readonly Mock<IEnemyRepository> _repoMock;
        private readonly IEnemyGetService _sut;
        private readonly IFixture _fixture;

        public EnemyGetServiceTests()
        {
            _repoMock = new Mock<IEnemyRepository>();
            _sut = new EnemyGetService(_repoMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetRangeAsync_ValidItemsData))]
        public async Task GetRangeAsync_ShouldReturnExpectedEnemies_WhenEnemiesInDbMatchSpecification(
            int expectedCount, List<Enemy> expectedEnemies, int minLevel, int maxLevel)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetMultipleByAsync(It.IsAny<EnemyLevelRangeSpecification>()))
                .ReturnsAsync(expectedEnemies);

            // Act
            var actualEnemies = await _sut.GetRangeAsync(minLevel, maxLevel);

            // Assert
            actualEnemies.FirstOrDefault().Should().BeOfType<EnemyRangeDto>();
            actualEnemies.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetRangeAsync_InvalidItemsData))]
        public async Task GetRangeAsync_ShouldReturnEmptyList_WhenEnemiesInDbDoNotMatchSpecification(
            int expectedCount, int minLevel, int maxLevel)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetMultipleByAsync(It.IsAny<EnemyLevelRangeSpecification>()))
                .ReturnsAsync(new List<Enemy>());

            // Act
            var actualEnemies = await _sut.GetRangeAsync(minLevel, maxLevel);

            // Assert
            actualEnemies.FirstOrDefault().Should().BeNull();
            actualEnemies.Count().Should().Be(expectedCount);
        }

        [Theory]
        [MemberData(nameof(GetById_ValidItemsData))]
        public async Task GetById_ShouldReturnExpectedEnemy_WhenIdMatchesEnemyInDb(
            Guid enemyId)
        {
            // Arrange
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.Id, enemyId)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyIdSpecification>()))
                .ReturnsAsync(expectedEnemy);

            // Act
            var actualEnemy = await _sut.GetById(enemyId);

            // Assert
            actualEnemy.Should().NotBeNull();
            actualEnemy.Id.Should().Be(enemyId);
        }

        [Fact]
        public async Task GetById_ShouldThrowException_WhenIdDoesNotMatchEnemyInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyToReturn", "No enemy matches provided specification."));

            // Act
            var action = async () => await _sut.GetById(Guid.NewGuid());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }

        [Fact]
        public async Task GetIsActiveForCombat_ShouldReturnExpectedEnemy_WhenThereIsAnActiveForCombatEnemyInDb()
        {
            // Arrange
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.IsActiveForCombat, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedEnemy);

            // Act
            var actualEnemy = await _sut.GetIsActiveForCombat();

            // Assert
            actualEnemy.Should().NotBeNull();
            actualEnemy.IsActiveForCombat.Should().BeTrue();
        }

        [Fact]
        public async Task GetIsActiveForCombat_ShouldThrowException_WhenThereIsNoActiveForCombatEnemyInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyToReturn", "No enemy matches provided specification."));

            // Act
            var action = async () => await _sut.GetIsActiveForCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }

        [Fact]
        public async Task GetActiveForCombatExp_ShouldReturnEnemyDroppedExp_WhenThereIsAnActiveForCombatEnemyInDb()
        {
            // Arrange
            var expectedExp = 64;
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.IsActiveForCombat, true)
                .With(x => x.DroppedExp, expectedExp)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedEnemy);

            // Act
            var actualExp = await _sut.GetActiveForCombatExp();

            // Assert
            actualExp.Should().Be(expectedExp);
        }

        [Fact]
        public async Task GetActiveForCombatExp_ShouldThrowException_WhenThereIsNoActiveForCombatEnemyInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyToReturn", "No enemy matches provided specification."));

            // Act
            var action = async () => await _sut.GetIsActiveForCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }

        public static IEnumerable<object[]> GetRangeAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    3,              // Expected Count
                    new List<Enemy> // Expected Enemies
                    {
                        new Enemy{ Name = "Lol", Level = 5 },
                        new Enemy{ Name = "Bruh", Level = 6 },
                        new Enemy{ Name = "LolBruh", Level = 10 },
                    },                                              
                    5,              // Min Level Range
                    10,             // Max Level Range
                },
                new object[]
                {
                    5,              // Expected Count
                    new List<Enemy> // Expected Enemies
                    {
                        new Enemy{ Name = "Incredibleu", Level = 15 },
                        new Enemy{ Name = "Neveroqtno", Level = 25 },
                        new Enemy{ Name = "Wunderbar", Level = 35 },
                        new Enemy{ Name = "Wunderbar", Level = 45 },
                        new Enemy{ Name = "Wunderbar", Level = 50 },
                    },
                    10,             // Min Level Range
                    50,             // Max Level Range
                }
            };

        public static IEnumerable<object[]> GetRangeAsync_InvalidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    0,  // Expected Count
                    11, // Min Level Range
                    20, // Max Level Range
                },
                new object[]
                {
                    0,  // Expected Count
                    1,  // Min Level Range
                    10, // Max Level Range
                }
            };

        public static IEnumerable<object[]> GetById_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    Guid.NewGuid(), // Enemy Id
                },
                new object[]
                {
                    Guid.NewGuid(), // Enemy Id
                }
            };
    }
}
