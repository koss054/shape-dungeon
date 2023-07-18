#nullable disable
using Moq;
using NUnit.Framework;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Rooms;
using System;
using System.Threading.Tasks;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    internal class RoomTravelServiceTests
    {
        private Mock<IEnemiesRoomsRepository> _mappingRepoMock;
        private Mock<IEnemyRepositoryOld> _enemyRepoMock;
        private Mock<IRoomRepositoryOld> _roomRepoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IRoomTravelService _service;

        [SetUp]
        public void Test_Initialize()
        {
            _mappingRepoMock = new Mock<IEnemiesRoomsRepository>();
            _enemyRepoMock = new Mock<IEnemyRepositoryOld>();
            _roomRepoMock = new Mock<IRoomRepositoryOld>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _service = new RoomTravelService(
                _mappingRepoMock.Object,
                _enemyRepoMock.Object,
                _roomRepoMock.Object, 
                _unitOfWorkMock.Object);
        }

        [Test]
        public void RoomTravel_WithInvalidAction_ThrowsOutOfRangeException()
        {
            // Arrange
            var direction = RoomDirection.Left;
            var action = 54;
            var oldCoordX = 0;
            var oldCoordY = 0;
            var newCoordX = oldCoordX - 1; // Moving to the left.
            var newCoordY = oldCoordY;
            var oldRoom = new Room { IsActiveForMove = true, CoordX = oldCoordX, CoordY = oldCoordY };
            var newRoom = new Room { IsActiveForMove = false, CoordX = newCoordX, CoordY = newCoordY };

            _roomRepoMock
                .Setup(x => x.GetActiveForMove())
                .ReturnsAsync(oldRoom);

            _roomRepoMock
                .Setup(x => x.GetByCoords(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(newRoom);

            // Act
            var travelTask =  _service.RoomTravelAsync(direction, (RoomTravelAction)action);

            // Assert
            Assert.That(async () => await travelTask,
                                    Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RoomTravel_WithInvalidDirection_ThrowsOutOfRangeException()
        {
            // Arrange
            var direction = 54;
            var action = RoomTravelAction.Move;
            var oldCoordX = 0;
            var oldCoordY = 0;
            var oldRoom = new Room { IsActiveForMove = true, CoordX = oldCoordX, CoordY = oldCoordY };

            _roomRepoMock
                .Setup(x => x.GetActiveForMove())
                .ReturnsAsync(oldRoom);

            // Act
            var travelTask = _service.RoomTravelAsync((RoomDirection)direction, action);

            // Assert
            Assert.That(async () => await travelTask,
                                    Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RoomTravel_WithMissingOldRoom_ThrowsNullException()
        {
            // Arrange
            var direction = 54;
            var action = RoomTravelAction.Move;

            // Act
            var travelTask = _service.RoomTravelAsync((RoomDirection)direction, action);

            // Assert
            Assert.That(async () => await travelTask,
                                    Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task ResetScout_WithValidRooms_TogglesActiveForScoutBetweenRooms()
        {
            // Arrange
            var scoutRoom = new Room { IsActiveForScout = true, IsActiveForMove = false };
            var moveRoom = new Room { IsActiveForScout = false, IsActiveForMove = true };

            _roomRepoMock
                .Setup(x => x.GetActiveForScout())
                .ReturnsAsync(scoutRoom);

            _roomRepoMock
                .Setup(x => x.GetActiveForMove())
                .ReturnsAsync(moveRoom);

            // Act
            var isSuccessful = await _service.IsScoutResetAsync();

            // Assert
            Assert.IsTrue(isSuccessful);
        }

        [Test]
        public async Task ResetScout_WithInvalidScoutRoom_SkipsActiveForScoutToggleBetweenRooms()
        {
            // Arrange
            var moveRoom = new Room { IsActiveForScout = false, IsActiveForMove = true };

            _roomRepoMock
                .Setup(x => x.GetActiveForMove())
                .ReturnsAsync(moveRoom);

            // Act
            var isSuccessful = await _service.IsScoutResetAsync();

            // Assert
            Assert.IsFalse(isSuccessful);
        }

        [Test]
        public async Task ResetScout_WithInvalidMoveRoom_SkipsActiveForScoutToggleBetweenRooms()
        {
            // Arrange
            var scoutRoom = new Room { IsActiveForScout = true, IsActiveForMove = false };

            _roomRepoMock
                .Setup(x => x.GetActiveForScout())
                .ReturnsAsync(scoutRoom);

            // Act
            var isSuccessful = await _service.IsScoutResetAsync();

            // Assert
            Assert.IsFalse(isSuccessful);
        }

        [Test]
        public async Task ResetScout_WithInvalidRooms_SkipsActiveForScoutToggleBetweenRooms()
        {
            // Arrange

            // Act
            var isSuccessful = await _service.IsScoutResetAsync();

            // Assert
            Assert.IsFalse(isSuccessful);
        }
    }
}
