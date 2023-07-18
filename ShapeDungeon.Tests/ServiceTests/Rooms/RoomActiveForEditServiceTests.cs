#nullable disable
using Moq;
using NUnit.Framework;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Rooms;
using System;
using System.Threading.Tasks;

namespace ShapeDungeon.Tests.ServiceTests.Rooms
{
    internal class RoomActiveForEditServiceTests
    {
        private Mock<IRoomRepositoryOld> _repoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IRoomActiveForEditService _service;

        [SetUp]
        public void Test_Initialize()
        {
            _repoMock = new Mock<IRoomRepositoryOld>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new RoomActiveForEditService(_repoMock.Object, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task ApplyActiveForEdit_WithValidRooms_TogglesActiveForEditBetweenRooms()
        {
            // Arrange
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var newRoom = new Room { Id = guid, IsActiveForEdit = false };
            var oldRoom = new Room { IsActiveForEdit = true };

            _repoMock
                .Setup(x => x.GetActiveForEdit())
                .ReturnsAsync(oldRoom);

            _repoMock
                .Setup(x => x.GetById(guid))
                .ReturnsAsync(newRoom);

            // Act
            await _service.ApplyActiveForEditAsync(guid);

            // Assert
            Assert.IsFalse(oldRoom.IsActiveForEdit);
            Assert.IsTrue(newRoom.IsActiveForEdit);
        }

        [Test]
        public async Task ApplyActiveForEdit_WithInvalidNewRoom_SkipsActiveForEditToggle()
        {
            // Arrange
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var oldRoom = new Room { IsActiveForEdit = true };

            _repoMock
                .Setup(x => x.GetActiveForEdit())
                .ReturnsAsync(oldRoom);

            // Act
            await _service.ApplyActiveForEditAsync(guid);

            // Assert
            Assert.IsTrue(oldRoom.IsActiveForEdit);
        }

        [Test]
        public async Task ApplyActiveForEdit_WithInvalidOldRoom_SkipsActiveForEditToggle()
        {
            // Arrange
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var newRoom = new Room { Id = guid, IsActiveForEdit = false };

            _repoMock
                .Setup(x => x.GetById(guid))
                .ReturnsAsync(newRoom);

            // Act
            await _service.ApplyActiveForEditAsync(guid);

            // Assert
            Assert.IsFalse(newRoom.IsActiveForEdit);
        }

        [Test]
        public async Task MoveActiveForEdit_WithValidRooms_TogglesActiveForEditBetweenRooms()
        {
            // Arrange
            var coordX = 1;
            var coordY = 2;
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var oldRoom = new Room { IsActiveForEdit = true };
            var newRoom = new Room
            {
                Id = guid,
                IsActiveForEdit = false,
                CoordX = coordX,
                CoordY = coordY
            };

            _repoMock
                .Setup(x => x.GetActiveForEdit())
                .ReturnsAsync(oldRoom);

            _repoMock
                .Setup(x => x.GetByCoords(coordX, coordY))
                .ReturnsAsync(newRoom);

            // Act
            await _service.MoveActiveForEditAsync(coordX, coordY);

            // Assert
            Assert.IsFalse(oldRoom.IsActiveForEdit);
            Assert.IsTrue(newRoom.IsActiveForEdit);
        }

        [Test]
        public async Task MoveActiveForEdit_WithInvalidNewRoom_SkipsActiveForEditToggle()
        {
            // Arrange
            var coordX = 1;
            var coordY = 2;
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var oldRoom = new Room { IsActiveForEdit = true };

            _repoMock
                .Setup(x => x.GetActiveForEdit())
                .ReturnsAsync(oldRoom);

            // Act
            await _service.MoveActiveForEditAsync(coordX, coordY);

            // Assert
            Assert.IsTrue(oldRoom.IsActiveForEdit);
        }

        [Test]
        public async Task MoveActiveForEdit_WithInvalidOldRoom_SkipsActiveForEditToggle()
        {
            // Arrange
            var coordX = 1;
            var coordY = 2;
            var guid = new Guid("9c77d525-85c8-4a6c-98fd-45c5f2039cdc");
            var newRoom = new Room
            {
                Id = guid,
                IsActiveForEdit = false,
                CoordX = coordX,
                CoordY = coordY
            };

            _repoMock
                .Setup(x => x.GetByCoords(coordX, coordY))
                .ReturnsAsync(newRoom);

            // Act
            await _service.MoveActiveForEditAsync(coordX, coordY);

            // Assert
            Assert.IsFalse(newRoom.IsActiveForEdit);
        }
    }
}
