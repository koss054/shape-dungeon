using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    public class RoomEnemyServiceTests
    {
        private readonly Mock<IEnemyRoomRepository> _repoMock;
        private readonly IRoomEnemyService _sut;
        private readonly IFixture _fixture;

        public RoomEnemyServiceTests()
        {
            _repoMock = new Mock<IEnemyRoomRepository>();
            _sut = new RoomEnemyService(_repoMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetEnemy_ShouldReturnEnemyDto_WhenIdMatchesEnemyRoomInDb()
        {
            // Arrange
            var expectedRoomId = Guid.NewGuid();

            var expectedEnemy = _fixture.Build<Enemy>()
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.RoomId, expectedRoomId)
                .With(x => x.EnemyId, expectedEnemy.Id)
                .With(x => x.Enemy, expectedEnemy)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ReturnsAsync(expectedEnemyRoom);

            // Act
            var actualEnemyDto = await _sut.GetEnemy(expectedRoomId);

            // Assert
            actualEnemyDto.Should().BeOfType<EnemyDto>();
            actualEnemyDto.Name.Should().Be(expectedEnemy.Name);
            actualEnemyDto.DroppedExp.Should().Be(expectedEnemy.DroppedExp);
        }

        [Fact]
        public async Task GetEnemy_ShouldThrowException_WhenIdDoesNotMatchEnemyRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyRoomToReturn", "No enemy room matches provided specification."));

            // Act
            var action = async () => await _sut.GetEnemy(It.IsAny<Guid>());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy room matches provided specification. (Parameter 'enemyRoomToReturn')");
        }

        [Theory]
        [MemberData(nameof(IsEnemyDefeated_ValidItemsData))]
        public async Task IsEnemyDefeated_ShouldReturnExpectedValue_WhenIdMatchesEnemyRoomInDb(
            bool expectedResult, bool isEnemyDefeated)
        {
            // Arrange
            var expectedRoomId = Guid.NewGuid();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.RoomId, expectedRoomId)
                .With(x => x.IsEnemyDefeated, isEnemyDefeated)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ReturnsAsync(expectedEnemyRoom);

            // Act
            var actualResult = await _sut.IsEnemyDefeated(expectedRoomId);

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task IsEnemyDefeated_ShouldThrowException_WhenIdDoesNotMatchEnemyRoomInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyRoomToReturn", "No enemy room matches provided specification."));

            // Act
            var action = async () => await _sut.IsEnemyDefeated(It.IsAny<Guid>());

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy room matches provided specification. (Parameter 'enemyRoomToReturn')");
        }

        public static IEnumerable<object[]> IsEnemyDefeated_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,   // Expected Result
                    true,   // Is Enemy Defeated
                },
                new object[]
                {
                    false,  // Expected Result
                    false,  // Is Enemy Defeated
                },
            };
    }
}
