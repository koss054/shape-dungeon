using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Services.Rooms;
using ShapeDungeon.Specifications.Enemies;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    public class RoomTravelServiceTests
    {
        private readonly Mock<IEnemyRoomRepository> _enemyRoomRepoMock;
        private readonly Mock<IRoomValidateService> _roomValidateServiceMock;
        private readonly Mock<IRoomRepository> _roomRepoMock;
        private readonly Mock<IEnemyRepository> _enemyRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IRoomTravelService _sut;
        private readonly IFixture _fixture;

        public RoomTravelServiceTests()
        {
            _enemyRoomRepoMock = new Mock<IEnemyRoomRepository>();
            _roomValidateServiceMock = new Mock<IRoomValidateService>();
            _roomRepoMock = new Mock<IRoomRepository>();
            _enemyRepoMock = new Mock<IEnemyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fixture = new Fixture();
            _sut = new RoomTravelService(
                _enemyRoomRepoMock.Object,
                _roomValidateServiceMock.Object,
                _roomRepoMock.Object,
                _enemyRepoMock.Object,
                _unitOfWorkMock.Object);
        }

        [Theory]
        [MemberData(nameof(RoomTravelAsync_MoveAction_ValidItemsData))]
        public async Task RoomTravelAsync_ShouldProceedMoveActionAsExpected_WhenValidDataIsPassed(
            Room expectedOldRoom,
            RoomDirection expectedMoveDirection,
            bool expectedCanEnterRoomFrom,
            int expectedUpdatedCoordX,
            int expectedUpdatedCoordY,
            Room expectedNewRoom,
            bool expectedOldRoomIsActiveForMove,
            bool expectedNewRoomIsActiveForMove,
            bool expectedEnemyActiveForCombat,
            Enemy expectedOldEnemy,
            Enemy expectedNewEnemy,
            bool expectedOldEnemyIsActiveForCombat,
            bool expectedNewEnemyIsActiveForCombat,
            bool expectedIsEnemyDefeated)
        {
            // Arrange
            var expectedActivateEnemyForCombatRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.EnemyId, expectedNewEnemy.Id)
                .With(x => x.Enemy, expectedNewEnemy)
                .With(x => x.IsEnemyDefeated, expectedIsEnemyDefeated)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _roomValidateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(expectedUpdatedCoordX, expectedUpdatedCoordY, It.IsAny<RoomDirection>()))
                .ReturnsAsync(expectedCanEnterRoomFrom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            _enemyRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedEnemyActiveForCombat);

            _enemyRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedOldEnemy);

            _enemyRoomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ReturnsAsync(expectedActivateEnemyForCombatRoom);

            _roomRepoMock.Object.Update(expectedOldRoom);
            _roomRepoMock.Object.Update(expectedNewRoom);

            _enemyRepoMock.Object.Update(expectedOldEnemy);
            _enemyRepoMock.Object.Update(expectedNewEnemy);

            // Act
            await _sut.RoomTravelAsync(expectedMoveDirection, RoomTravelAction.Move);

            var actualOldRoomIsActiveForMove = expectedOldRoom.IsActiveForMove;
            var actualNewRoomIsActiveForMove = expectedNewRoom.IsActiveForMove;

            var actualOldEnemyIsActiveForCombat = expectedOldEnemy.IsActiveForCombat;
            var actualNewEnemyIsActiveForCombat = expectedNewEnemy.IsActiveForCombat;

            // Assert
            actualOldRoomIsActiveForMove.Should().Be(expectedOldRoomIsActiveForMove);
            actualNewRoomIsActiveForMove.Should().Be(expectedNewRoomIsActiveForMove);
            actualOldEnemyIsActiveForCombat.Should().Be(expectedOldEnemyIsActiveForCombat);
            actualNewEnemyIsActiveForCombat.Should().Be(expectedNewEnemyIsActiveForCombat);
        }

        [Theory]
        [MemberData(nameof(RoomTravelAsync_ScoutAction_ValidItemsData))]
        public async Task RoomTravelAsync_ShouldProceedScoutActionAsExpected_WhenValidDataIsPassed(
            Room expectedOldRoom,
            RoomDirection expectedMoveDirection,
            bool expectedCanEnterRoomFrom,
            int expectedUpdatedCoordX,
            int expectedUpdatedCoordY,
            Room expectedNewRoom,
            bool expectedOldRoomIsActiveForScout,
            bool expectedNewRoomisActiveForScout)
        {
            // Arrange
            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _roomValidateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(expectedUpdatedCoordX, expectedUpdatedCoordY, It.IsAny<RoomDirection>()))
                .ReturnsAsync(expectedCanEnterRoomFrom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            _roomRepoMock.Object.Update(expectedOldRoom);
            _roomRepoMock.Object.Update(expectedNewRoom);

            // Act
            await _sut.RoomTravelAsync(expectedMoveDirection, RoomTravelAction.Scout);

            var actualOldRoomIsActiveForScout = expectedOldRoom.IsActiveForScout;
            var actualNewRoomIsActiveForScout = expectedNewRoom.IsActiveForScout;

            // Assert
            actualOldRoomIsActiveForScout.Should().Be(expectedOldRoomIsActiveForScout);
            actualNewRoomIsActiveForScout.Should().Be(expectedNewRoomisActiveForScout);
        }

        [Fact]
        public async Task RoomTravelAsync_ShouldThrowException_WhenInvalidDirection()
        {
            // Arrange
            var expectedDirection = (RoomDirection)22202;
            var expectedTravelAction = RoomTravelAction.Move;

            var expectedRoom = _fixture.Build<Room>()
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var action = async () => await _sut.RoomTravelAsync(expectedDirection, expectedTravelAction);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'direction')");
        }

        [Fact]
        public async Task RoomTravelAsync_ShouldThrowException_WhenInvalidCoordsForNewRoom()
        {
            // Arrange
            var expectedDirection = RoomDirection.Top;
            var expectedTravelAction = RoomTravelAction.Move;

            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.CanGoUp, true)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            _roomValidateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(It.IsAny<int>(), It.IsAny<int>(), expectedDirection))
                .ReturnsAsync(true);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.RoomTravelAsync(expectedDirection, expectedTravelAction);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task RoomTravelAsync_ShouldThrowException_WhenInvalidRoomTravelAction()
        {
            // Arrange
            var expectedDirection = RoomDirection.Top;
            var expectedTravelAction = (RoomTravelAction)54;

            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.CanGoUp, true)
                .Create();

            var expectedNewRoom = _fixture.Build<Room>()
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _roomValidateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(It.IsAny<int>(), It.IsAny<int>(), expectedDirection))
                .ReturnsAsync(true);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            // Act
            var action = async () => await _sut.RoomTravelAsync(expectedDirection, expectedTravelAction);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'action')");
        }

        [Fact]
        public async Task RoomTravelAsync_ShouldThrowException_WhenInvalidEnemyRoomIdPassed()
        {
            // Arrange
            var expectedDirection = RoomDirection.Top;
            var expectedTravelAction = RoomTravelAction.Move;

            var expectedOldRoom = _fixture.Build<Room>()
                .With(x => x.CanGoUp, true)
                .Create();

            var expectedNewRoom = _fixture.Build<Room>()
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedOldRoom);

            _roomValidateServiceMock
                .Setup(x => x.CanEnterRoomFromDirection(It.IsAny<int>(), It.IsAny<int>(), expectedDirection))
                .ReturnsAsync(true);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedNewRoom);

            _enemyRoomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyRoomToReturn", "No enemy room matches provided specification."));

            // Act
            var action = async () => await _sut.RoomTravelAsync(expectedDirection, expectedTravelAction);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy room matches provided specification. (Parameter 'enemyRoomToReturn')");
        }

        [Fact]
        public async Task IsScoutResetAsync_ShouldReturnTrue_WhenScoutResetSuccessfullt()
        {
            // Arrange
            var expectedActiveForScoutRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForScout, true)
                .With(x => x.IsActiveForMove, false)
                .Create();

            var expectedActiveForMoveRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForScout, false)
                .With(x => x.IsActiveForMove, true)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ReturnsAsync(expectedActiveForScoutRoom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedActiveForMoveRoom);

            _roomRepoMock.Object.Update(expectedActiveForScoutRoom);
            _roomRepoMock.Object.Update(expectedActiveForMoveRoom);

            // Act
            var actualResult = await _sut.IsScoutResetAsync();

            // Assert
            _roomRepoMock.Verify(x => x.Update(expectedActiveForScoutRoom), Times.Once);
            _roomRepoMock.Verify(x => x.Update(expectedActiveForMoveRoom), Times.Once);
            actualResult.Should().BeTrue();
            expectedActiveForScoutRoom.IsActiveForScout.Should().BeFalse();
            expectedActiveForMoveRoom.IsActiveForScout.Should().BeTrue();
        }

        [Fact]
        public async Task IsScoutResetAsync_ShouldThrowException_WhenNoActiveForScoutInDb()
        {
            // Arrange
            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.IsScoutResetAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task IsScoutResetAsync_ShouldThrowException_WhenNoActiveForMoveInDb()
        {
            // Arrange
            var expectedActiveForScoutRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForScout, true)
                .With(x => x.IsActiveForMove, false)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ReturnsAsync(expectedActiveForScoutRoom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.IsScoutResetAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        public static IEnumerable<object[]> RoomTravelAsync_MoveAction_ValidItemsData
            => new List<object[]>
            {
                #region NewRoom_NotEnemyRoom_SuccessfulMove
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    { 
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true, 
                        IsActiveForScout = true,
                        CanGoLeft = true,
                    },
                    RoomDirection.Left,         // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the right side.
                    -1,                         // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = -1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoRight = true,
                    },
                    RoomDirection.Right,        // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the left side.
                    1,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoUp = true,
                    },
                    RoomDirection.Top,          // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    1,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoDown = true,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    -1,                         // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = -1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                #endregion
                #region NewRoom_NotEnemyRoom_UnsuccessfulMove_DueToDeadEndOrVoid
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoLeft = false,
                    },
                    RoomDirection.Left,         // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the right side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoRight = false,
                    },
                    RoomDirection.Right,        // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the left side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoUp = false,
                    },
                    RoomDirection.Top,          // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoDown = false,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                #endregion
                #region NewRoom_ActiveEnemyRoom_SuccessfulMove
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoLeft = true,
                    },
                    RoomDirection.Left,         // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the right side.
                    -1,                         // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = -1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    true,                       // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = true,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    true,                       // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoRight = true,
                    },
                    RoomDirection.Right,        // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the left side.
                    1,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    true,                       // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = true,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    true,                       // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoUp = true,
                    },
                    RoomDirection.Top,          // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    1,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    true,                       // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = true,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    true,                       // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoDown = true,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    -1,                         // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = -1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    true,                       // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = true,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    true,                       // Expected New Enemy IsActiveForCombat
                    false,                      // Expected Is Enemy Defeated (false since curr room is not enemy)
                },
                #endregion
                #region NewRoom_DefeatedEnemyRoom_SuccessfulMove
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoLeft = true,
                    },
                    RoomDirection.Left,         // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the right side.
                    -1,                         // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = -1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    true,                       // Expected Is Enemy Defeated
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoRight = true,
                    },
                    RoomDirection.Right,        // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the left side.
                    1,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    true,                       // Expected Is Enemy Defeated
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoUp = true,
                    },
                    RoomDirection.Top,          // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    1,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    true,                       // Expected Is Enemy Defeated
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoDown = true,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    -1,                         // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = -1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = true,
                    },
                    false,                      // Expected Old Room IsActiveForMoveValue
                    true,                       // Expected New Room IsActiveForMoveValue
                    false,                      // Is Any Enemy In Db Active For Combat
                    new Enemy                   // Placeholder Old Enemy
                    {
                        IsActiveForCombat = false,
                    },
                    new Enemy                   // Placeholder New Enemy
                    {
                        Id = Guid.NewGuid(),
                        IsActiveForCombat = false,
                    },
                    false,                      // Expected Old Enemy IsActiveForCombat
                    false,                      // Expected New Enemy IsActiveForCombat
                    true,                       // Expected Is Enemy Defeated
                },
                #endregion
            };

        public static IEnumerable<object[]> RoomTravelAsync_ScoutAction_ValidItemsData
            => new List<object[]>
            {
                #region NewRoom_SuccessfulScout
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoLeft = true,
                    },
                    RoomDirection.Left,         // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the right side.
                    -1,                         // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = -1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoRight = true,
                    },
                    RoomDirection.Right,        // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the left side.
                    1,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 1,
                        CoordY = 0,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoUp = true,
                    },
                    RoomDirection.Top,          // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    1,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        CanGoDown = true,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    true,                       // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    -1,                         // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = -1,
                        IsActiveForMove = false,
                        IsActiveForScout = false,
                        IsEnemyRoom = false,
                    },
                    false,                      // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                #endregion
                #region NewRoom_UnsuccessfulScout_DueToDeadEndOrVoid
                // Move to the left from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoLeft = false,
                    },
                    RoomDirection.Left,         // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the right side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move to the right from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoRight = false,
                    },
                    RoomDirection.Right,        // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the left side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move up from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoUp = false,
                    },
                    RoomDirection.Top,          // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the bottom side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                // Move down from the first room.
                new object[]
                {
                    new Room                    // Old Room - from which the player moves
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                        CanGoDown = false,
                    },
                    RoomDirection.Bottom,       // Move Direction
                    false,                      // Can Enter Room Value - if the new room can be entered from the top side.
                    0,                          // Expected Coord X After Move
                    0,                          // Expected Coord Y After Move
                    new Room                    // New Room - which the player moves to
                    {
                        Id = Guid.NewGuid(),
                        CoordX = 0,
                        CoordY = 0,
                        IsActiveForMove = true,
                        IsActiveForScout = true,
                        IsEnemyRoom = false,
                    },
                    true,                       // Expected Old Room IsActiveForScoutValue
                    true,                       // Expected New Room IsActiveForScoutValue
                },
                #endregion
            };
    }
}
