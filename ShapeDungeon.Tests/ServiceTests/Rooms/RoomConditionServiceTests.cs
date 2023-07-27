using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    public class RoomConditionServiceTests
    {
        private readonly Mock<IRoomRepository> _roomRepoMock;
        private readonly Mock<IEnemyRoomRepository> _enemyRoomRepoMock;
        private readonly IRoomConditionService _sut;
        private readonly IFixture _fixture;

        public RoomConditionServiceTests()
        {
            _roomRepoMock = new Mock<IRoomRepository>();
            _enemyRoomRepoMock = new Mock<IEnemyRoomRepository>();
            _sut = new RoomConditionService(_roomRepoMock.Object, _enemyRoomRepoMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(IsCurrentRoomActiveEnemyRoom_ValidItemsData))]
        public async Task IsCurrentRoomActiveEnemyRoom_ShouldReturnExpectedValue_WhenActiveForMoveRoomIsInDb(
            bool expectedResult, bool isEnemyRoom, bool isEnemyDefeated)
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsEnemyRoom, isEnemyRoom)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            _enemyRoomRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyRoomDefeatedSpecification>()))
                .ReturnsAsync(isEnemyDefeated);

            // Act
            var actualResult = await _sut.IsCurrentRoomActiveEnemyRoom();

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task IsCurrentRoomActiveEnemyRoom_ShouldThrowException_WhenNoActiveForMoveRoomInDb()
        {
            // Arrange
            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            _enemyRoomRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyRoomDefeatedSpecification>()))
                .ReturnsAsync(It.IsAny<bool>());

            // Act
            var action = async () => await _sut.IsCurrentRoomActiveEnemyRoom();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        public static IEnumerable<object[]> IsCurrentRoomActiveEnemyRoom_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,   // Expected Result
                    true,   // Is Room Enemy Room
                    false,  // Is Enemy Defeated
                },
                new object[]
                {
                    false,  // Expected Result
                    true,   // Is Room Enemy Room
                    true,   // Is Enemy Defeated
                },
                new object[]
                {
                    false,  // Expected Result
                    false,  // Is Room Enemy Room
                    true,   // Is Enemy Defeated
                },
            };
    }
}
